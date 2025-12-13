using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Election.DATA.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedToCandidate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Candidates");
        }
    }
}
