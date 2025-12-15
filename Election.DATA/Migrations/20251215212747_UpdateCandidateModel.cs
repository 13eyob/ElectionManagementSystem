using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Election.DATA.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCandidateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminRemarks",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "Candidates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShortBio",
                table: "Candidates",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VotesReceived",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ElectionSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 15, 21, 27, 46, 152, DateTimeKind.Utc).AddTicks(7924));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRemarks",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ShortBio",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "VotesReceived",
                table: "Candidates");

            migrationBuilder.UpdateData(
                table: "ElectionSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 14, 20, 6, 10, 503, DateTimeKind.Utc).AddTicks(5352));
        }
    }
}
