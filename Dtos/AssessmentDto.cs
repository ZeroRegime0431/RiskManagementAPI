using System.ComponentModel.DataAnnotations;

namespace RiskManagementAPI.Dtos;

// Used when CREATING a new assessment
public class CreateAssessmentDto
{
    [Required(ErrorMessage = "Quarter is required")]
    public string Quarter { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Year is required")]
    [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100")]
    public int Year { get; set; }
    
    [Required(ErrorMessage = "ResidualLikelihood is required")]
    [Range(1, 5, ErrorMessage = "ResidualLikelihood must be between 1 and 5")]
    public int ResidualLikelihood { get; set; }
    
    [Required(ErrorMessage = "ResidualImpact is required")]
    [Range(1, 5, ErrorMessage = "ResidualImpact must be between 1 and 5")]
    public int ResidualImpact { get; set; }
    
    public string Notes { get; set; } = string.Empty;
}

// Used for assessment responses
public class AssessmentResponseDto
{
    public int Id { get; set; }
    public string Quarter { get; set; } = string.Empty;
    public int Year { get; set; }
    public int ResidualLikelihood { get; set; }
    public int ResidualImpact { get; set; }
    public int ResidualRiskScore { get; set; }
    public int RiskReduction { get; set; }  // InherentRiskScore - ResidualRiskScore
    public string Notes { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}