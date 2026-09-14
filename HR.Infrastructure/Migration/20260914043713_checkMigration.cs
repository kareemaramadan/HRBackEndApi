using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class checkMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ModuleImage",
                schema: "Auth",
                table: "Modules",
                type: "varbinary(MAX)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ModuleImage",
                schema: "Auth",
                table: "Modules",
                type: "varbinary",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)");
        }
    }
}
