using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createRoleModulesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleModules",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModules", x => new { x.RoleId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_RoleModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Auth",
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleModules_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleModules_ModuleId",
                schema: "Auth",
                table: "RoleModules",
                column: "ModuleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleModules_RoleId",
                schema: "Auth",
                table: "RoleModules",
                column: "RoleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleModules",
                schema: "Auth");
        }
    }
}
