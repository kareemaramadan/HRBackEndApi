using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateadministrationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPagePermissions_ModulePages_PageId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropTable(
                name: "RolePagePermissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "ModulePages",
                schema: "Auth");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPagePermission_PageId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPagePermission_UserId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions",
                columns: new[] { "UserId", "PageId", "PermissionId" });

            migrationBuilder.CreateTable(
                name: "SubModulePages",
                schema: "Auth",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PageName_ar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModulePages", x => x.PageId);
                });

            migrationBuilder.CreateTable(
                name: "Module_ModulePages",
                schema: "Auth",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module_ModulePages", x => new { x.ModuleId, x.PageId });
                    table.ForeignKey(
                        name: "FK_Module_ModulePages_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Auth",
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Module_ModulePages_SubModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "SubModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role_ModulePages",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_ModulePages", x => new { x.RoleId, x.PageId });
                    table.ForeignKey(
                        name: "FK_Role_ModulePages_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_ModulePages_SubModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "SubModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role_Page_Permissions",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Page_Permissions", x => new { x.RoleId, x.PageId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_Role_Page_Permissions_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Page_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Page_Permissions_SubModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "SubModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPagePermissions_PageId",
                schema: "Auth",
                table: "UserPagePermissions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPagePermissions_Unique",
                schema: "Auth",
                table: "UserPagePermissions",
                columns: new[] { "UserId", "PageId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Module_ModulePages_ModulePageId_ModuleId",
                schema: "Auth",
                table: "Module_ModulePages",
                columns: new[] { "PageId", "ModuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_ModulePages_PageId",
                schema: "Auth",
                table: "Role_ModulePages",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ModulePages_RoleId_PageId",
                schema: "Auth",
                table: "Role_ModulePages",
                columns: new[] { "RoleId", "PageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Page_Permissions_PageId",
                schema: "Auth",
                table: "Role_Page_Permissions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Page_Permissions_PermissionId",
                schema: "Auth",
                table: "Role_Page_Permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePagePermission_RoleId",
                schema: "Auth",
                table: "Role_Page_Permissions",
                columns: new[] { "RoleId", "PageId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePage_PageName_ar",
                schema: "Auth",
                table: "SubModulePages",
                column: "PageName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePage_PageName_en",
                schema: "Auth",
                table: "SubModulePages",
                column: "PageName_en",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPagePermissions_SubModulePages_PageId",
                schema: "Auth",
                table: "UserPagePermissions",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "SubModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPagePermissions_SubModulePages_PageId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropTable(
                name: "Module_ModulePages",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role_ModulePages",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role_Page_Permissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "SubModulePages",
                schema: "Auth");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPagePermissions_PageId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPagePermissions_Unique",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                schema: "Auth",
                table: "UserPagePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions",
                columns: new[] { "UserId", "PageId", "PermissionId", "ModuleId" });

            migrationBuilder.CreateTable(
                name: "ModulePages",
                schema: "Auth",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PageName_ar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PageName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePages", x => x.PageId);
                    table.ForeignKey(
                        name: "FK_ModulePages_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Auth",
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePagePermissions",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePagePermissions", x => new { x.RoleId, x.PageId, x.PermissionId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_RolePagePermissions_ModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "ModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePagePermissions_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePagePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPagePermission_PageId",
                schema: "Auth",
                table: "UserPagePermissions",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPagePermission_UserId",
                schema: "Auth",
                table: "UserPagePermissions",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePage_PageName_ar",
                schema: "Auth",
                table: "ModulePages",
                column: "PageName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePage_PageName_en",
                schema: "Auth",
                table: "ModulePages",
                column: "PageName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePages_ModuleId",
                schema: "Auth",
                table: "ModulePages",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePagePermission_PageId",
                schema: "Auth",
                table: "RolePagePermissions",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePagePermission_RoleId",
                schema: "Auth",
                table: "RolePagePermissions",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePagePermissions_PermissionId",
                schema: "Auth",
                table: "RolePagePermissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPagePermissions_ModulePages_PageId",
                schema: "Auth",
                table: "UserPagePermissions",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "ModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
