using Microsoft.EntityFrameworkCore;
using RiskManagementAPI.Models;

namespace RiskManagementAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }
    
    public DbSet<Risk> Risks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed 10 risks for testing
        SeedRisks(modelBuilder);
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