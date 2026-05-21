using System.ComponentModel.DataAnnotations;

namespace RiskManagementAPI.Dtos;

public class CreateAssessmentDto
{
    [Required]
    public string Quarter { get; set; } = string.Empty;
    
    [Required]
    [Range(2000, 2100)]
    public int Year { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int ResidualLikelihood { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int ResidualImpact { get; set; }
    
    public string Notes { get; set; } = string.Empty;
}
 
public class AssessmentResponseDto
{
    public int Id { get; set; }
    public string Quarter { get; set; } = string.Empty;
    public int Year { get; set; }
    public int ResidualLikelihood { get; set; }
    public int ResidualImpact { get; set; }
    public int ResidualRiskScore { get; set; }
    public int RiskReduction { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}