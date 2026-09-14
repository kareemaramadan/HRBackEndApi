using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeisactiveanddeletioncolumnsfromRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "Auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Auth",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "Image",
                schema: "Auth",
                table: "Users",
                newName: "ProfilePicture");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                schema: "Auth",
                table: "Users",
                newName: "Image");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "Auth",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Auth",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Auth",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
