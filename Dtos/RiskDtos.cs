using System.ComponentModel.DataAnnotations;

namespace RiskManagementAPI.Dtos;

// Used when CREATING a new risk
public class CreateRiskDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "LikelihoodScore is required")]
    [Range(1, 5, ErrorMessage = "LikelihoodScore must be between 1 and 5")]
    public int LikelihoodScore { get; set; }
    
    [Required(ErrorMessage = "ImpactScore is required")]
    [Range(1, 5, ErrorMessage = "ImpactScore must be between 1 and 5")]
    public int ImpactScore { get; set; }
    
    [Required(ErrorMessage = "OwnerId is required")]
    public string OwnerId { get; set; } = string.Empty;
}

// Used when UPDATING an existing risk (all fields optional)
public class UpdateRiskDto
{
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? Category { get; set; }
    
    [Range(1, 5, ErrorMessage = "LikelihoodScore must be between 1 and 5")]
    public int? LikelihoodScore { get; set; }
    
    [Range(1, 5, ErrorMessage = "ImpactScore must be between 1 and 5")]
    public int? ImpactScore { get; set; }
    
    public string? OwnerId { get; set; }
}

// Used for LIST responses (lightweight)
public class RiskSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int InherentRiskScore { get; set; }
    public string Status { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

// Used for DETAIL response (includes controls - for Phase 3)
public class RiskDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int LikelihoodScore { get; set; }
    public int ImpactScore { get; set; }
    public int InherentRiskScore { get; set; }
    public string Status { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ControlCount { get; set; }
    public List<ControlResponseDto> Controls { get; set; } = new();
}

// Used for status update
public class UpdateStatusDto
{
    [Required(ErrorMessage = "Status is required")]
    public string Status { get; set; } = string.Empty;
}