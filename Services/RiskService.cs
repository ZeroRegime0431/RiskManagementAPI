using Microsoft.EntityFrameworkCore;
using RiskManagementAPI.Data;
using RiskManagementAPI.Dtos;
using RiskManagementAPI.Models;

namespace RiskManagementAPI.Services;

public class RiskService
{
    private readonly AppDbContext _context;
    
    public RiskService(AppDbContext context)
    {
        _context = context;
    }
    
    // GET all risks - returns DTOs, not entities
    public async Task<List<RiskSummaryDto>> GetAllRisksAsync()
    {
        var risks = await _context.Risks
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        
        return risks.Select(r => MapToSummaryDto(r)).ToList();
    }
    
    // GET single risk
    public async Task<RiskDetailDto?> GetRiskByIdAsync(int id)
    {
        var risk = await _context.Risks.Include(r => r.Controls) // Include controls for detail view
            .FirstOrDefaultAsync(r => r.Id == id);
        if (risk == null) return null;
        
        return MapToDetailDto(risk);
    }
    
    // CREATE new risk
    public async Task<RiskDetailDto> CreateRiskAsync(CreateRiskDto createDto)
    {
        // Validate category
        if (!Enum.TryParse<RiskCategory>(createDto.Category, true, out var category))
        {
            throw new ArgumentException($"Invalid category: {createDto.Category}");
        }
        
        var risk = new Risk
        {
            Title = createDto.Title,
            Description = createDto.Description,
            Category = category,
            LikelihoodScore = createDto.LikelihoodScore,
            ImpactScore = createDto.ImpactScore,
            OwnerId = createDto.OwnerId,
            Status = RiskStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Risks.Add(risk);
        await _context.SaveChangesAsync();
        
        return MapToDetailDto(risk);
    }
    
    // UPDATE risk
    public async Task<RiskDetailDto?> UpdateRiskAsync(int id, UpdateRiskDto updateDto)
    {
        var risk = await _context.Risks.FindAsync(id);
        if (risk == null) return null;
        
        if (updateDto.Title != null) risk.Title = updateDto.Title;
        if (updateDto.Description != null) risk.Description = updateDto.Description;
        if (updateDto.Category != null && Enum.TryParse<RiskCategory>(updateDto.Category, true, out var category))
            risk.Category = category;
        if (updateDto.LikelihoodScore.HasValue) risk.LikelihoodScore = updateDto.LikelihoodScore.Value;
        if (updateDto.ImpactScore.HasValue) risk.ImpactScore = updateDto.ImpactScore.Value;
        if (updateDto.OwnerId != null) risk.OwnerId = updateDto.OwnerId;
        
        await _context.SaveChangesAsync();
        
        return MapToDetailDto(risk);
    }
    
    // DELETE risk
    public async Task<bool> DeleteRiskAsync(int id)
    {
        var risk = await _context.Risks.FindAsync(id);
        if (risk == null) return false;
        
        if (risk.Status != RiskStatus.Draft)
        {
            throw new InvalidOperationException(
                $"Cannot delete risk with status '{risk.Status}'. Only Draft risks can be deleted.");
        }
        
        _context.Risks.Remove(risk);
        await _context.SaveChangesAsync();
        return true;
    }
    
    // UPDATE risk status
    public async Task<bool> UpdateRiskStatusAsync(int id, string newStatusString)
    {
        var risk = await _context.Risks.FindAsync(id);
        if (risk == null) return false;
        
        if (!Enum.TryParse<RiskStatus>(newStatusString, true, out var newStatus))
        {
            throw new ArgumentException($"Invalid status: {newStatusString}");
        }
        
        // Allowed transitions
        bool isValid = (risk.Status, newStatus) switch
        {
            (RiskStatus.Draft, RiskStatus.UnderReview) => true,
            (RiskStatus.UnderReview, RiskStatus.Approved) => true,
            (RiskStatus.UnderReview, RiskStatus.Draft) => true,
            (RiskStatus.Approved, RiskStatus.Closed) => true,
            (RiskStatus.Closed, _) => false,
            _ => false
        };
        
        if (!isValid)
        {
            throw new InvalidOperationException(
                $"Cannot transition from {risk.Status} to {newStatus}");
        }
        
        risk.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }
    
    // MAPPING METHODS (Convert Entity to DTO)
    private RiskSummaryDto MapToSummaryDto(Risk risk)
    {
        return new RiskSummaryDto
        {
            Id = risk.Id,
            Title = risk.Title,
            Category = risk.Category.ToString(),
            InherentRiskScore = risk.InherentRiskScore,
            Status = risk.Status.ToString(),
            OwnerId = risk.OwnerId,
            CreatedAt = risk.CreatedAt
        };
    }
    
    private RiskDetailDto MapToDetailDto(Risk risk)
    {
        return new RiskDetailDto
        {
            Id = risk.Id,
            Title = risk.Title,
            Description = risk.Description,
            Category = risk.Category.ToString(),
            LikelihoodScore = risk.LikelihoodScore,
            ImpactScore = risk.ImpactScore,
            InherentRiskScore = risk.InherentRiskScore,
            Status = risk.Status.ToString(),
            OwnerId = risk.OwnerId,
            CreatedAt = risk.CreatedAt,
            ControlCount = risk.Controls?.Count ?? 0,
            Controls = new List<ControlResponseDto>() 
        };
    }


    // GET: Score summary with trend analysis 
public async Task<ScoreSummaryDto?> GetScoreSummaryAsync(int id)
{
    // Load risk with its controls and assessments
    var risk = await _context.Risks
        .Include(r => r.Controls)
        .Include(r => r.QuarterlyAssessments)
        .FirstOrDefaultAsync(r => r.Id == id);
    
    if (risk == null) return null;
    
    // Get assessments ordered by date (most recent first)
    var assessments = risk.QuarterlyAssessments
        .OrderByDescending(a => a.Year)
        .ThenByDescending(a => a.Quarter)
        .ToList();
    
    var latestAssessment = assessments.FirstOrDefault();
    var previousAssessment = assessments.Skip(1).FirstOrDefault();
    
    // Calculate trend
    string trend = "NotEnoughData";
    if (assessments.Count >= 2 && previousAssessment != null)
    {
        int latestScore = latestAssessment!.ResidualRiskScore;
        int previousScore = previousAssessment.ResidualRiskScore;
        
        if (latestScore < previousScore)
            trend = "Improving";
        else if (latestScore > previousScore)
            trend = "Worsening";
        else
            trend = "Stable";
    }
    
    // Calculate risk reduction
    int? riskReduction = null;
    int? latestResidualScore = null;
    
    if (latestAssessment != null)
    {
        latestResidualScore = latestAssessment.ResidualRiskScore;
        riskReduction = risk.InherentRiskScore - latestResidualScore.Value;
    }
    
    return new ScoreSummaryDto
    {
        RiskId = risk.Id,
        Title = risk.Title,
        InherentRiskScore = risk.InherentRiskScore,
        LatestResidualScore = latestResidualScore,
        RiskReduction = riskReduction,
        ControlCount = risk.Controls.Count,
        LastAssessedOn = latestAssessment?.SubmittedAt,
        Trend = trend
    };
}
}