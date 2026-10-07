using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmodulecolumnId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                schema: "Auth",
                table: "UserPagePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                schema: "Auth",
                table: "RolePagePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "Auth",
                table: "UserPagePermissions");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "Auth",
                table: "RolePagePermissions");
        }
    }
}
