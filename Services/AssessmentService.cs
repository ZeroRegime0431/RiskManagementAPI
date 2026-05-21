using Microsoft.EntityFrameworkCore;
using RiskManagementAPI.Data;
using RiskManagementAPI.Dtos;
using RiskManagementAPI.Models;

namespace RiskManagementAPI.Services;

public class AssessmentService
{
    private readonly AppDbContext _context;
    
    public AssessmentService(AppDbContext context)
    {
        _context = context;
    }
    
    // GET: All assessments for a risk (ordered by year, then quarter)
    public async Task<List<AssessmentResponseDto>> GetAssessmentsByRiskIdAsync(int riskId)
    {
        // Check if risk exists
        var risk = await _context.Risks.FindAsync(riskId);
        if (risk == null)
        {
            throw new KeyNotFoundException($"Risk with ID {riskId} not found");
        }
        
        var assessments = await _context.QuarterlyAssessments
            .Where(a => a.RiskId == riskId)
            .OrderByDescending(a => a.Year)
            .ThenByDescending(a => a.Quarter)
            .ToListAsync();
        
        return assessments.Select(a => MapToResponseDto(a, risk.InherentRiskScore)).ToList();
    }
    
    // GET: Latest assessment for a risk
    public async Task<AssessmentResponseDto?> GetLatestAssessmentAsync(int riskId)
    {
        var risk = await _context.Risks.FindAsync(riskId);
        if (risk == null)
        {
            throw new KeyNotFoundException($"Risk with ID {riskId} not found");
        }
        
        var assessment = await _context.QuarterlyAssessments
            .Where(a => a.RiskId == riskId)
            .OrderByDescending(a => a.Year)
            .ThenByDescending(a => a.Quarter)
            .FirstOrDefaultAsync();
        
        if (assessment == null) return null;
        
        return MapToResponseDto(assessment, risk.InherentRiskScore);
    }
    
    // POST: Create a new assessment
    public async Task<AssessmentResponseDto> CreateAssessmentAsync(int riskId, CreateAssessmentDto createDto)
    {
        // Check if risk exists
        var risk = await _context.Risks.FindAsync(riskId);
        if (risk == null)
        {
            throw new KeyNotFoundException($"Risk with ID {riskId} not found");
        }
        
        // BUSINESS RULE: Risk must be Approved
        if (risk.Status != RiskStatus.Approved)
        {
            throw new InvalidOperationException(
                $"Risk must be in 'Approved' status before an assessment can be filed. " +
                $"Current status: '{risk.Status}'");
        }
        
        // Validate Quarter
        if (!Enum.TryParse<Quarter>(createDto.Quarter, true, out var quarter))
        {
            throw new ArgumentException(
                $"Invalid quarter: {createDto.Quarter}. Allowed: Q1, Q2, Q3, Q4");
        }
        
        // BUSINESS RULE: No duplicate assessment for same quarter/year
        var existingAssessment = await _context.QuarterlyAssessments
            .FirstOrDefaultAsync(a => a.RiskId == riskId 
                && a.Year == createDto.Year 
                && a.Quarter == quarter);
        
        if (existingAssessment != null)
        {
            throw new InvalidOperationException(
                $"An assessment for {quarter} {createDto.Year} already exists for this risk. " +
                $"Only one assessment allowed per quarter per year.");
        }
        
        // Create assessment (ResidualRiskScore computed automatically via property)
        var assessment = new QuarterlyAssessment
        {
            RiskId = riskId,
            Quarter = quarter,
            Year = createDto.Year,
            ResidualLikelihood = createDto.ResidualLikelihood,
            ResidualImpact = createDto.ResidualImpact,
            Notes = createDto.Notes,
            SubmittedAt = DateTime.UtcNow
        };
        
        _context.QuarterlyAssessments.Add(assessment);
        await _context.SaveChangesAsync();
        
        return MapToResponseDto(assessment, risk.InherentRiskScore);
    }
    
    // Mapping method: Entity to DTO
    private AssessmentResponseDto MapToResponseDto(QuarterlyAssessment assessment, int inherentRiskScore)
    {
        return new AssessmentResponseDto
        {
            Id = assessment.Id,
            Quarter = assessment.Quarter.ToString(),
            Year = assessment.Year,
            ResidualLikelihood = assessment.ResidualLikelihood,
            ResidualImpact = assessment.ResidualImpact,
            ResidualRiskScore = assessment.ResidualRiskScore,
            RiskReduction = inherentRiskScore - assessment.ResidualRiskScore,
            Notes = assessment.Notes,
            SubmittedAt = assessment.SubmittedAt
        };
    }
}