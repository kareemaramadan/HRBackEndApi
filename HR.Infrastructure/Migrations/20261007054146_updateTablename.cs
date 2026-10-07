using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_ModulePages_SubModulePages_PageId",
                schema: "Auth",
                table: "Module_ModulePages");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_ModulePages_SubModulePages_PageId",
                schema: "Auth",
                table: "Role_ModulePages");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Page_Permissions_SubModulePages_PageId",
                schema: "Auth",
                table: "Role_Page_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPagePermissions_SubModulePages_PageId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubModulePages",
                schema: "Auth",
                table: "SubModulePages");

            migrationBuilder.RenameTable(
                name: "SubModulePages",
                schema: "Auth",
                newName: "ModulePages",
                newSchema: "Auth");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModulePages",
                schema: "Auth",
                table: "ModulePages",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_ModulePages_ModulePages_PageId",
                schema: "Auth",
                table: "Module_ModulePages",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "ModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_ModulePages_ModulePages_PageId",
                schema: "Auth",
                table: "Role_ModulePages",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "ModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Page_Permissions_ModulePages_PageId",
                schema: "Auth",
                table: "Role_Page_Permissions",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "ModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_ModulePages_ModulePages_PageId",
                schema: "Auth",
                table: "Module_ModulePages");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_ModulePages_ModulePages_PageId",
                schema: "Auth",
                table: "Role_ModulePages");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Page_Permissions_ModulePages_PageId",
                schema: "Auth",
                table: "Role_Page_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPagePermissions_ModulePages_PageId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModulePages",
                schema: "Auth",
                table: "ModulePages");

            migrationBuilder.RenameTable(
                name: "ModulePages",
                schema: "Auth",
                newName: "SubModulePages",
                newSchema: "Auth");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubModulePages",
                schema: "Auth",
                table: "SubModulePages",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_ModulePages_SubModulePages_PageId",
                schema: "Auth",
                table: "Module_ModulePages",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "SubModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_ModulePages_SubModulePages_PageId",
                schema: "Auth",
                table: "Role_ModulePages",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "SubModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Page_Permissions_SubModulePages_PageId",
                schema: "Auth",
                table: "Role_Page_Permissions",
                column: "PageId",
                principalSchema: "Auth",
                principalTable: "SubModulePages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Restrict);

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
    }
}
