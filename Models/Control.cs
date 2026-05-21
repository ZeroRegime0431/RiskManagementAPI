using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskManagementAPI.Models;

public class Control
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public ControlType Type { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int EffectivenessScore { get; set; }
    
    // Foreign key - links to Risk
    public int RiskId { get; set; }
    
    // Navigation property - allows access to parent Risk
    public virtual Risk Risk { get; set; } = null!;
}

public enum ControlType
{
    Preventive,   // Stops risk from happening (e.g., firewall, approval process)
    Detective,    // Finds risk after it happens (e.g., monitoring, audit)
    Corrective    // Fixes risk after it happens (e.g., backup, incident response)
}