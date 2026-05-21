using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Risk_Intake___Assessment_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Risks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    LikelihoodScore = table.Column<int>(type: "int", nullable: false),
                    ImpactScore = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Risks",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "ImpactScore", "LikelihoodScore", "OwnerId", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Risk of sensitive customer data being exposed through third-party vendor", 4, 3, "owner-001", 2, "Vendor Data Breach" },
                    { 2, 0, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Key microchip supplier could face production delays", 5, 2, "owner-002", 1, "Supply Chain Disruption" },
                    { 3, 1, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Heavy exposure to EUR/USD exchange rate fluctuations", 3, 4, "owner-003", 0, "Currency Exchange Rate Volatility" },
                    { 4, 3, new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Expanding into Japan and South Korea markets", 4, 3, "owner-001", 2, "Asia-Pacific Market Entry Failure" },
                    { 5, 2, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Potential non-compliance with GDPR requirements", 5, 2, "owner-004", 3, "GDPR Compliance Gaps" },
                    { 6, 0, new DateTime(2024, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Senior architect has critical knowledge of legacy systems", 4, 3, "owner-002", 1, "Loss of Key Technical Leadership" },
                    { 7, 1, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "$50M in variable-rate debt tied to SOFR", 3, 4, "owner-005", 0, "Rising Interest Rates Impact on Loans" },
                    { 8, 0, new DateTime(2024, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Increasing ransomware attacks in the industry", 5, 3, "owner-001", 2, "Ransomware Attack on Production Systems" },
                    { 9, 3, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "New startup with AI-driven solution could disrupt our pricing model", 4, 4, "owner-003", 0, "AI-Powered Competitor Entering Market" },
                    { 10, 2, new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Proposed legislation would require 40% reduction in carbon emissions", 4, 3, "owner-004", 1, "New Carbon Emission Regulations" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Risks");
        }
    }
}
