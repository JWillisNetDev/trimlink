using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trimlink.data.Migrations
{
    /// <inheritdoc />
    public partial class LinkMakeUtcDateExpiresNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNeverExpires",
                table: "Links");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UtcDateExpires",
                table: "Links",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UtcDateExpires",
                table: "Links",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNeverExpires",
                table: "Links",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
