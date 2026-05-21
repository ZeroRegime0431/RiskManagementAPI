using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Risk_Intake___Assessment_API.Migrations
{
    /// <inheritdoc />
    public partial class AddQuarterlyAssessmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuarterlyAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskId = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ResidualLikelihood = table.Column<int>(type: "int", nullable: false),
                    ResidualImpact = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuarterlyAssessments_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "QuarterlyAssessments",
                columns: new[] { "Id", "Notes", "Quarter", "ResidualImpact", "ResidualLikelihood", "RiskId", "SubmittedAt", "Year" },
                values: new object[,]
                {
                    { 1, "Vendor has implemented new security controls. Risk reduced but still requires monitoring.", 0, 3, 2, 1, new DateTime(2024, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2025 },
                    { 2, "Initial assessment - vendor security needs improvement.", 3, 4, 3, 1, new DateTime(2024, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2024 },
                    { 3, "Local partnerships established in Japan. Early signs positive.", 3, 3, 2, 4, new DateTime(2024, 9, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2024 },
                    { 4, "New backup systems implemented. Recovery time reduced to 48 hours.", 0, 4, 2, 8, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2025 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UniqueAssessmentPerQuarter",
                table: "QuarterlyAssessments",
                columns: new[] { "RiskId", "Year", "Quarter" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuarterlyAssessments");
        }
    }
}
