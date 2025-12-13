using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Election.DATA.Migrations
{
    /// <inheritdoc />
    public partial class AddElectionSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectionSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsElectionActive = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ElectionTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VotingOpen = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationOpen = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ElectionSettings",
                columns: new[] { "Id", "CreatedDate", "ElectionTitle", "EndTime", "IsElectionActive", "RegistrationOpen", "StartTime", "UpdatedDate", "VotingOpen" },
                values: new object[] { 1, new DateTime(2025, 12, 13, 18, 30, 26, 220, DateTimeKind.Utc).AddTicks(7571), "General Election 2024", null, false, true, null, null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectionSettings");
        }
    }
}
