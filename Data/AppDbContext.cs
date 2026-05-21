using Microsoft.EntityFrameworkCore;
using RiskManagementAPI.Models;

namespace RiskManagementAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }
    
    public DbSet<Risk> Risks { get; set; }
    public DbSet<Control> Controls { get; set; }
    public DbSet<QuarterlyAssessment> QuarterlyAssessments { get; set; }  // ← ADD THIS
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure Control - Risk relationship
        modelBuilder.Entity<Control>()
            .HasOne(c => c.Risk)
            .WithMany(r => r.Controls)
            .HasForeignKey(c => c.RiskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure Assessment - Risk relationship
        modelBuilder.Entity<QuarterlyAssessment>()
            .HasOne(a => a.Risk)
            .WithMany(r => r.QuarterlyAssessments)
            .HasForeignKey(a => a.RiskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Add unique constraint: One assessment per quarter per year per risk
        modelBuilder.Entity<QuarterlyAssessment>()
            .HasIndex(a => new { a.RiskId, a.Year, a.Quarter })
            .IsUnique()
            .HasDatabaseName("IX_UniqueAssessmentPerQuarter");
        
        // Seed sample assessments
        SeedAssessments(modelBuilder);
        
        // Seed data
        SeedRisks(modelBuilder);
        SeedControls(modelBuilder);
    }
    
    private void SeedAssessments(ModelBuilder modelBuilder)
    {
        var fixedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<QuarterlyAssessment>().HasData(
            new QuarterlyAssessment
            {
                Id = 1,
                RiskId = 1,  // Vendor Data Breach
                Quarter = Quarter.Q1,
                Year = 2025,
                ResidualLikelihood = 2,
                ResidualImpact = 3,
                Notes = "Vendor has implemented new security controls. Risk reduced but still requires monitoring.",
                SubmittedAt = fixedDate.AddDays(-45)
            },
            new QuarterlyAssessment
            {
                Id = 2,
                RiskId = 1,  // Vendor Data Breach
                Quarter = Quarter.Q4,
                Year = 2024,
                ResidualLikelihood = 3,
                ResidualImpact = 4,
                Notes = "Initial assessment - vendor security needs improvement.",
                SubmittedAt = fixedDate.AddDays(-100)
            },
            new QuarterlyAssessment
            {
                Id = 3,
                RiskId = 4,  // Asia-Pacific Market Entry
                Quarter = Quarter.Q4,
                Year = 2024,
                ResidualLikelihood = 2,
                ResidualImpact = 3,
                Notes = "Local partnerships established in Japan. Early signs positive.",
                SubmittedAt = fixedDate.AddDays(-100)
            },
            new QuarterlyAssessment
            {
                Id = 4,
                RiskId = 8,  // Ransomware Attack
                Quarter = Quarter.Q1,
                Year = 2025,
                ResidualLikelihood = 2,
                ResidualImpact = 4,
                Notes = "New backup systems implemented. Recovery time reduced to 48 hours.",
                SubmittedAt = fixedDate.AddDays(-30)
            }
        );
    }
    
    private void SeedControls(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Control>().HasData(
            new Control
            {
                Id = 1,
                Title = "Quarterly Vendor Security Audit",
                Description = "Perform comprehensive security audits of all vendors handling sensitive data",
                Type = ControlType.Preventive,
                EffectivenessScore = 4,
                RiskId = 1  // Belongs to "Vendor Data Breach" risk
            },
            new Control
            {
                Id = 2,
                Title = "Vendor Contract Termination Clause",
                Description = "Include right-to-audit and termination for security breach clauses in all vendor contracts",
                Type = ControlType.Preventive,
                EffectivenessScore = 3,
                RiskId = 1
            },
            new Control
            {
                Id = 3,
                Title = "Multi-Source Procurement Strategy",
                Description = "Maintain at least two qualified suppliers for all critical components",
                Type = ControlType.Corrective,
                EffectivenessScore = 3,
                RiskId = 2  // Belongs to "Supply Chain Disruption" risk
            },
            new Control
            {
                Id = 4,
                Title = "Real-time Currency Hedging Program",
                Description = "Automated hedging for foreign currency exposure above $5M",
                Type = ControlType.Preventive,
                EffectivenessScore = 4,
                RiskId = 3  // Belongs to "Currency Exchange" risk
            },
            new Control
            {
                Id = 5,
                Title = "Weekly Currency Position Monitoring",
                Description = "Daily monitoring of currency exposure with alerts for thresholds",
                Type = ControlType.Detective,
                EffectivenessScore = 4,
                RiskId = 3
            },
            new Control
            {
                Id = 6,
                Title = "Local Partnership Program",
                Description = "Establish strategic partnerships with local firms in target markets",
                Type = ControlType.Preventive,
                EffectivenessScore = 3,
                RiskId = 4  // Belongs to "Market Entry" risk
            },
            new Control
            {
                Id = 7,
                Title = "24/7 Security Monitoring",
                Description = "Real-time threat detection and response system",
                Type = ControlType.Detective,
                EffectivenessScore = 4,
                RiskId = 8  // Belongs to "Ransomware" risk
            },
            new Control
            {
                Id = 8,
                Title = "Offline Backups",
                Description = "Daily encrypted backups stored offline for ransomware recovery",
                Type = ControlType.Corrective,
                EffectivenessScore = 3,
                RiskId = 8
            }
        );
    }
    
    private void SeedRisks(ModelBuilder modelBuilder)
{
    // Use FIXED dates instead of DateTime.UtcNow
    var baseDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    // Risk 1
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 1,
        Title = "Vendor Data Breach",
        Description = "Risk of sensitive customer data being exposed through third-party vendor",
        Category = RiskCategory.Compliance,
        LikelihoodScore = 3,
        ImpactScore = 4,
        Status = RiskStatus.Approved,
        OwnerId = "owner-001",
        CreatedAt = baseDate.AddDays(-180)  // Fixed: 180 days before base date
    });
    
    // Risk 2
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 2,
        Title = "Supply Chain Disruption",
        Description = "Key microchip supplier could face production delays",
        Category = RiskCategory.Operational,
        LikelihoodScore = 2,
        ImpactScore = 5,
        Status = RiskStatus.UnderReview,
        OwnerId = "owner-002",
        CreatedAt = baseDate.AddDays(-120)
    });
    
    // Risk 3
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 3,
        Title = "Currency Exchange Rate Volatility",
        Description = "Heavy exposure to EUR/USD exchange rate fluctuations",
        Category = RiskCategory.Financial,
        LikelihoodScore = 4,
        ImpactScore = 3,
        Status = RiskStatus.Draft,
        OwnerId = "owner-003",
        CreatedAt = baseDate.AddDays(-30)
    });
    
    // Risk 4
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 4,
        Title = "Asia-Pacific Market Entry Failure",
        Description = "Expanding into Japan and South Korea markets",
        Category = RiskCategory.Strategic,
        LikelihoodScore = 3,
        ImpactScore = 4,
        Status = RiskStatus.Approved,
        OwnerId = "owner-001",
        CreatedAt = baseDate.AddDays(-90)
    });
    
    // Risk 5
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 5,
        Title = "GDPR Compliance Gaps",
        Description = "Potential non-compliance with GDPR requirements",
        Category = RiskCategory.Compliance,
        LikelihoodScore = 2,
        ImpactScore = 5,
        Status = RiskStatus.Closed,
        OwnerId = "owner-004",
        CreatedAt = baseDate.AddDays(-300)
    });
    
    // Risk 6
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 6,
        Title = "Loss of Key Technical Leadership",
        Description = "Senior architect has critical knowledge of legacy systems",
        Category = RiskCategory.Operational,
        LikelihoodScore = 3,
        ImpactScore = 4,
        Status = RiskStatus.UnderReview,
        OwnerId = "owner-002",
        CreatedAt = baseDate.AddDays(-45)
    });
    
    // Risk 7
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 7,
        Title = "Rising Interest Rates Impact on Loans",
        Description = "$50M in variable-rate debt tied to SOFR",
        Category = RiskCategory.Financial,
        LikelihoodScore = 4,
        ImpactScore = 3,
        Status = RiskStatus.Draft,
        OwnerId = "owner-005",
        CreatedAt = baseDate.AddDays(-15)
    });
    
    // Risk 8
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 8,
        Title = "Ransomware Attack on Production Systems",
        Description = "Increasing ransomware attacks in the industry",
        Category = RiskCategory.Operational,
        LikelihoodScore = 3,
        ImpactScore = 5,
        Status = RiskStatus.Approved,
        OwnerId = "owner-001",
        CreatedAt = baseDate.AddDays(-60)
    });
    
    // Risk 9
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 9,
        Title = "AI-Powered Competitor Entering Market",
        Description = "New startup with AI-driven solution could disrupt our pricing model",
        Category = RiskCategory.Strategic,
        LikelihoodScore = 4,
        ImpactScore = 4,
        Status = RiskStatus.Draft,
        OwnerId = "owner-003",
        CreatedAt = baseDate.AddDays(-20)
    });
    
    // Risk 10
    modelBuilder.Entity<Risk>().HasData(new Risk
    {
        Id = 10,
        Title = "New Carbon Emission Regulations",
        Description = "Proposed legislation would require 40% reduction in carbon emissions",
        Category = RiskCategory.Compliance,
        LikelihoodScore = 3,
        ImpactScore = 4,
        Status = RiskStatus.UnderReview,
        OwnerId = "owner-004",
        CreatedAt = baseDate.AddDays(-75)
    });
    }
}