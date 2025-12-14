using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Election.DATA.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ElectionSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 14, 20, 6, 10, 503, DateTimeKind.Utc).AddTicks(5352));

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId_Unique",
                table: "Votes",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_UserId_Unique",
                table: "Votes");

            migrationBuilder.UpdateData(
                table: "ElectionSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 12, 14, 18, 31, 55, 979, DateTimeKind.Utc).AddTicks(6164));
        }
    }
}
