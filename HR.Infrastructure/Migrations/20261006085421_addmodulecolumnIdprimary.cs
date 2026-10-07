using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmodulecolumnIdprimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePagePermissions",
                schema: "Auth",
                table: "RolePagePermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions",
                columns: new[] { "UserId", "PageId", "PermissionId", "ModuleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePagePermissions",
                schema: "Auth",
                table: "RolePagePermissions",
                columns: new[] { "RoleId", "PageId", "PermissionId", "ModuleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePagePermissions",
                schema: "Auth",
                table: "RolePagePermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPagePermissions",
                schema: "Auth",
                table: "UserPagePermissions",
                columns: new[] { "UserId", "PageId", "PermissionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePagePermissions",
                schema: "Auth",
                table: "RolePagePermissions",
                columns: new[] { "RoleId", "PageId", "PermissionId" });
        }
    }
}
