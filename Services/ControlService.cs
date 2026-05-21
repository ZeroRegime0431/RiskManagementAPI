using Microsoft.EntityFrameworkCore;
using RiskManagementAPI.Data;
using RiskManagementAPI.Dtos;
using RiskManagementAPI.Models;

namespace RiskManagementAPI.Services;

public class ControlService
{
    private readonly AppDbContext _context;
    
    public ControlService(AppDbContext context)
    {
        _context = context;
    }
    
    // GET: All controls for a specific risk
    public async Task<List<ControlResponseDto>> GetControlsByRiskIdAsync(int riskId)
    {
        // First check if risk exists
        var risk = await _context.Risks.FindAsync(riskId);
        if (risk == null)
        {
            throw new KeyNotFoundException($"Risk with ID {riskId} not found");
        }
        
        var controls = await _context.Controls
            .Where(c => c.RiskId == riskId)
            .ToListAsync();
        
        return controls.Select(c => MapToResponseDto(c, risk.Title)).ToList();
    }
    
    // POST: Create a new control for a risk
    public async Task<ControlResponseDto> CreateControlAsync(int riskId, CreateControlDto createDto)
    {
        // Check if risk exists
        var risk = await _context.Risks.FindAsync(riskId);
        if (risk == null)
        {
            throw new KeyNotFoundException($"Risk with ID {riskId} not found");
        }
        
        // Business rule: Cannot add controls to Closed risk
        if (risk.Status == RiskStatus.Closed)
        {
            throw new InvalidOperationException(
                $"Cannot add controls to a Closed risk. Risk '{risk.Title}' is already closed.");
        }
        
        // Validate Control Type
        if (!Enum.TryParse<ControlType>(createDto.Type, true, out var controlType))
        {
            throw new ArgumentException(
                $"Invalid control type: {createDto.Type}. Allowed: Preventive, Detective, Corrective");
        }
        
        var control = new Control
        {
            Title = createDto.Title,
            Description = createDto.Description,
            Type = controlType,
            EffectivenessScore = createDto.EffectivenessScore,
            RiskId = riskId
        };
        
        _context.Controls.Add(control);
        await _context.SaveChangesAsync();
        
        return MapToResponseDto(control, risk.Title);
    }
    
    // PUT: Update an existing control
    public async Task<ControlResponseDto> UpdateControlAsync(int riskId, int controlId, UpdateControlDto updateDto)
    {
        // Find control and include its Risk
        var control = await _context.Controls
            .Include(c => c.Risk)
            .FirstOrDefaultAsync(c => c.Id == controlId && c.RiskId == riskId);
        
        if (control == null)
        {
            throw new KeyNotFoundException(
                $"Control with ID {controlId} not found for risk {riskId}");
        }
        
        // Business rule: Cannot update controls on Closed risk
        if (control.Risk.Status == RiskStatus.Closed)
        {
            throw new InvalidOperationException(
                $"Cannot update controls on a Closed risk. Risk '{control.Risk.Title}' is closed.");
        }
        
        // Update only provided fields
        if (updateDto.Title != null) control.Title = updateDto.Title;
        if (updateDto.Description != null) control.Description = updateDto.Description;
        if (updateDto.Type != null && Enum.TryParse<ControlType>(updateDto.Type, true, out var type))
            control.Type = type;
        if (updateDto.EffectivenessScore.HasValue) 
            control.EffectivenessScore = updateDto.EffectivenessScore.Value;
        
        await _context.SaveChangesAsync();
        
        return MapToResponseDto(control, control.Risk.Title);
    }
    
    // DELETE: Remove a control
    public async Task<bool> DeleteControlAsync(int riskId, int controlId)
    {
        var control = await _context.Controls
            .Include(c => c.Risk)
            .FirstOrDefaultAsync(c => c.Id == controlId && c.RiskId == riskId);
        
        if (control == null) return false;
        
        // Business rule: Cannot delete controls from Closed risk
        if (control.Risk.Status == RiskStatus.Closed)
        {
            throw new InvalidOperationException(
                $"Cannot delete controls from a Closed risk. Risk '{control.Risk.Title}' is closed.");
        }
        
        _context.Controls.Remove(control);
        await _context.SaveChangesAsync();
        return true;
    }
    
    // Mapping method: Entity to DTO
    private ControlResponseDto MapToResponseDto(Control control, string riskTitle)
    {
        return new ControlResponseDto
        {
            Id = control.Id,
            Title = control.Title,
            Description = control.Description,
            Type = control.Type.ToString(),
            EffectivenessScore = control.EffectivenessScore,
            RiskId = control.RiskId,
            RiskTitle = riskTitle
        };
    }
}