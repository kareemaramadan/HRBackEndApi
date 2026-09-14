using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDBWithAuthandAuthorizationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LookUps");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CountryName_ar = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                schema: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GradeName_ar = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    percentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "Auth",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModuleImage = table.Column<byte[]>(type: "varbinary(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "Auth",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PermissionCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActivatedAccount = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governorate",
                schema: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GovName_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GovName_ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GovCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Country_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Governorate_Country_Country_Id",
                        column: x => x.Country_Id,
                        principalSchema: "LookUps",
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModulePages",
                schema: "Auth",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Auth",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                schema: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gov_Id = table.Column<int>(type: "int", nullable: false),
                    CityName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CityName_ar = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Governorate_Gov_Id",
                        column: x => x.Gov_Id,
                        principalSchema: "LookUps",
                        principalTable: "Governorate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePagePermissions",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePagePermissions", x => new { x.RoleId, x.PageId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePagePermissions_ModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "ModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePagePermissions_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePagePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPagePermissions",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPagePermissions", x => new { x.UserId, x.PageId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPagePermissions_ModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "ModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPagePermissions_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPagePermissions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompName_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompName_ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City_Id = table.Column<int>(type: "int", nullable: false),
                    Address_en = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address_ar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompLogo = table.Column<byte[]>(type: "image", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_City_City_Id",
                        column: x => x.City_Id,
                        principalSchema: "LookUps",
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_CityName_ar",
                schema: "LookUps",
                table: "City",
                column: "CityName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_CityName_en",
                schema: "LookUps",
                table: "City",
                column: "CityName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_Gov_Id",
                schema: "LookUps",
                table: "City",
                column: "Gov_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_City_Id",
                schema: "LookUps",
                table: "Company",
                column: "City_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompName_ar",
                schema: "LookUps",
                table: "Company",
                column: "CompName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompName_en",
                schema: "LookUps",
                table: "Company",
                column: "CompName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Country_ar",
                schema: "LookUps",
                table: "Country",
                column: "CountryName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Country_en",
                schema: "LookUps",
                table: "Country",
                column: "CountryName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorate_Country_Id",
                schema: "LookUps",
                table: "Governorate",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Governorate_GovName_ar",
                schema: "LookUps",
                table: "Governorate",
                column: "GovName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorate_GovName_en",
                schema: "LookUps",
                table: "Governorate",
                column: "GovName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grade_GradeName_ar",
                schema: "LookUps",
                table: "Grade",
                column: "GradeName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grade_GradeName_en",
                schema: "LookUps",
                table: "Grade",
                column: "GradeName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePage_PageName",
                schema: "Auth",
                table: "ModulePages",
                column: "PageName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulePages_ModuleId",
                schema: "Auth",
                table: "ModulePages",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Module_ModuleName",
                schema: "Auth",
                table: "Modules",
                column: "ModuleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PermissionName",
                schema: "Auth",
                table: "Permission",
                column: "PermissionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Auth",
                table: "RoleClaims",
                column: "RoleId");

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

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Auth",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Auth",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Auth",
                table: "UserLogins",
                column: "UserId");

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
                name: "IX_UserPagePermissions_PermissionId",
                schema: "Auth",
                table: "UserPagePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Auth",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Auth",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Auth",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company",
                schema: "LookUps");

            migrationBuilder.DropTable(
                name: "Grade",
                schema: "LookUps");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "RolePagePermissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserPagePermissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "City",
                schema: "LookUps");

            migrationBuilder.DropTable(
                name: "ModulePages",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Governorate",
                schema: "LookUps");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "LookUps");
        }
    }
}
