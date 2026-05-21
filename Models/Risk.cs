using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskManagementAPI.Models;

public class Risk
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public RiskCategory Category { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int LikelihoodScore { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int ImpactScore { get; set; }
    
    // This is calculated, not stored in database
    [NotMapped]
    public int InherentRiskScore => LikelihoodScore * ImpactScore;
    
    [Required]
    public RiskStatus Status { get; set; } = RiskStatus.Draft;
    
    [Required]
    [MaxLength(100)]
    public string OwnerId { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum RiskCategory
{
    Operational,
    Financial,
    Compliance,
    Strategic
}

public enum RiskStatus
{
    Draft,
    UnderReview,
    Approved,
    Closed
}