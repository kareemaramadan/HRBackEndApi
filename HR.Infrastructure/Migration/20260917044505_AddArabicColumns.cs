using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModuleName",
                schema: "Auth",
                table: "Modules",
                newName: "ModuleName_en");

            migrationBuilder.RenameIndex(
                name: "IX_Module_ModuleName",
                schema: "Auth",
                table: "Modules",
                newName: "IX_Module_ModuleName_en");

            migrationBuilder.RenameColumn(
                name: "PageName",
                schema: "Auth",
                table: "ModulePages",
                newName: "PageName_en");

            migrationBuilder.RenameIndex(
                name: "IX_ModulePage_PageName",
                schema: "Auth",
                table: "ModulePages",
                newName: "IX_ModulePage_PageName_en");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ModuleImage",
                schema: "Auth",
                table: "Modules",
                type: "varbinary(MAX)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "ModuleName_ar",
                schema: "Auth",
                table: "Modules",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PageName_ar",
                schema: "Auth",
                table: "ModulePages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Module_ModuleName_ar",
                schema: "Auth",
                table: "Modules",
                column: "ModuleName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePage_PageName_ar",
                schema: "Auth",
                table: "ModulePages",
                column: "PageName_ar",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Module_ModuleName_ar",
                schema: "Auth",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_ModulePage_PageName_ar",
                schema: "Auth",
                table: "ModulePages");

            migrationBuilder.DropColumn(
                name: "ModuleName_ar",
                schema: "Auth",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "PageName_ar",
                schema: "Auth",
                table: "ModulePages");

            migrationBuilder.RenameColumn(
                name: "ModuleName_en",
                schema: "Auth",
                table: "Modules",
                newName: "ModuleName");

            migrationBuilder.RenameIndex(
                name: "IX_Module_ModuleName_en",
                schema: "Auth",
                table: "Modules",
                newName: "IX_Module_ModuleName");

            migrationBuilder.RenameColumn(
                name: "PageName_en",
                schema: "Auth",
                table: "ModulePages",
                newName: "PageName");

            migrationBuilder.RenameIndex(
                name: "IX_ModulePage_PageName_en",
                schema: "Auth",
                table: "ModulePages",
                newName: "IX_ModulePage_PageName");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ModuleImage",
                schema: "Auth",
                table: "Modules",
                type: "varbinary(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(MAX)");
        }
    }
}
