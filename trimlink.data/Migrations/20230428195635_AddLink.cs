using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trimlink.data.Migrations
{
    /// <inheritdoc />
    public partial class AddLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtcDateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtcDateExpires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsNeverExpires = table.Column<bool>(type: "bit", nullable: false),
                    IsMarkedForDeletion = table.Column<bool>(type: "bit", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RedirectToUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_UtcDateExpires",
                table: "Links",
                column: "UtcDateExpires");

            migrationBuilder.CreateIndex(
                name: "UX_Links_Token",
                table: "Links",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");
        }
    }
}
