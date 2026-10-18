using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAuthTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Role_SubModulePages",
                schema: "Auth");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowed",
                schema: "Auth",
                table: "Role_Page_Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RevokedTokens",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jti = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RevokedAtUtc = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokedTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevokedToken_Jti",
                schema: "Auth",
                table: "RevokedTokens",
                column: "Jti",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevokedTokens",
                schema: "Auth");

            migrationBuilder.DropColumn(
                name: "IsAllowed",
                schema: "Auth",
                table: "Role_Page_Permissions");

            migrationBuilder.CreateTable(
                name: "Role_SubModulePages",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_SubModulePages", x => new { x.RoleId, x.PageId });
                    table.ForeignKey(
                        name: "FK_Role_SubModulePages_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_SubModulePages_SubModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "SubModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_ModulePages_RoleId_PageId",
                schema: "Auth",
                table: "Role_SubModulePages",
                columns: new[] { "RoleId", "PageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_SubModulePages_PageId",
                schema: "Auth",
                table: "Role_SubModulePages",
                column: "PageId");
        }
    }
}
