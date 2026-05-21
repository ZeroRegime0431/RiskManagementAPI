using System.ComponentModel.DataAnnotations;

namespace RiskManagementAPI.Dtos;

public class CreateControlDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string Type { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 5)]
    public int EffectivenessScore { get; set; }
}

public class UpdateControlDto
{
    [MaxLength(200)]
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? Type { get; set; }
    
    [Range(1, 5)]
    public int? EffectivenessScore { get; set; }
}

public class ControlResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int EffectivenessScore { get; set; }
    public int RiskId { get; set; }
    public string RiskTitle { get; set; } = string.Empty;
}