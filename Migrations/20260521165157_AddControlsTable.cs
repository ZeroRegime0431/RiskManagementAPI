using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Risk_Intake___Assessment_API.Migrations
{
    /// <inheritdoc />
    public partial class AddControlsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Controls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    EffectivenessScore = table.Column<int>(type: "int", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Controls_Risks_RiskId",
                        column: x => x.RiskId,
                        principalTable: "Risks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Controls",
                columns: new[] { "Id", "Description", "EffectivenessScore", "RiskId", "Title", "Type" },
                values: new object[,]
                {
                    { 1, "Perform comprehensive security audits of all vendors handling sensitive data", 4, 1, "Quarterly Vendor Security Audit", 0 },
                    { 2, "Include right-to-audit and termination for security breach clauses in all vendor contracts", 3, 1, "Vendor Contract Termination Clause", 0 },
                    { 3, "Maintain at least two qualified suppliers for all critical components", 3, 2, "Multi-Source Procurement Strategy", 2 },
                    { 4, "Automated hedging for foreign currency exposure above $5M", 4, 3, "Real-time Currency Hedging Program", 0 },
                    { 5, "Daily monitoring of currency exposure with alerts for thresholds", 4, 3, "Weekly Currency Position Monitoring", 1 },
                    { 6, "Establish strategic partnerships with local firms in target markets", 3, 4, "Local Partnership Program", 0 },
                    { 7, "Real-time threat detection and response system", 4, 8, "24/7 Security Monitoring", 1 },
                    { 8, "Daily encrypted backups stored offline for ransomware recovery", 3, 8, "Offline Backups", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Controls_RiskId",
                table: "Controls",
                column: "RiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Controls");
        }
    }
}
