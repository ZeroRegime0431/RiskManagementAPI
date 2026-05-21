using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskManagementAPI.Models;

public class QuarterlyAssessment
{
    [Key]
    public int Id { get; set; }
    
    // Foreign key
    public int RiskId { get; set; }
    
    // Navigation property
    public virtual Risk Risk { get; set; } = null!;
    
    [Required]
    public Quarter Quarter { get; set; }
    
    [Required]
    [Range(2000, 2100)]
    public int Year { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int ResidualLikelihood { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int ResidualImpact { get; set; }
    
    // Computed property - NOT stored in database
    [NotMapped]
    public int ResidualRiskScore => ResidualLikelihood * ResidualImpact;
    
    public string Notes { get; set; } = string.Empty;
    
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

public enum Quarter
{
    Q1,  // January - March
    Q2,  // April - June
    Q3,  // July - September
    Q4   // October - December
}