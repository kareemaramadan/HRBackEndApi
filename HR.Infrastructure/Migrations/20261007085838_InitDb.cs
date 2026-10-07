using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
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
                    ModuleName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ModuleName_ar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModuleImage = table.Column<byte[]>(type: "varbinary(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Auth",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                name: "SubModules",
                schema: "Auth",
                columns: table => new
                {
                    SubModuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubModuleName_en = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SubModuleName_ar = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModules", x => x.SubModuleId);
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
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
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
                name: "Module_SubModules",
                schema: "Auth",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    SubModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module_SubModules", x => new { x.ModuleId, x.SubModuleId });
                    table.ForeignKey(
                        name: "FK_Module_SubModules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Auth",
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Module_SubModules_SubModuleId",
                        column: x => x.SubModuleId,
                        principalSchema: "Auth",
                        principalTable: "SubModules",
                        principalColumn: "SubModuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role_SubModules",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    SubModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_SubModules", x => new { x.RoleId, x.ModuleId, x.SubModuleId });
                    table.ForeignKey(
                        name: "FK_Role_SubModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "Auth",
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_SubModules_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_SubModules_SubModules_SubModuleId",
                        column: x => x.SubModuleId,
                        principalSchema: "Auth",
                        principalTable: "SubModules",
                        principalColumn: "SubModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubModulePages",
                schema: "Auth",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubModuleId = table.Column<int>(type: "int", nullable: false),
                    PageName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PageName_ar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModulePages", x => x.PageId);
                    table.ForeignKey(
                        name: "FK_SubModulePages_SubModules_SubModuleId",
                        column: x => x.SubModuleId,
                        principalSchema: "Auth",
                        principalTable: "SubModules",
                        principalColumn: "SubModuleId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Role_Page_Permissions",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Page_Permissions", x => new { x.RoleId, x.PageId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_Role_Page_Permissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permissions",
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

            migrationBuilder.CreateTable(
                name: "User_Page_Permissions",
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
                    table.PrimaryKey("PK_User_Page_Permissions", x => new { x.UserId, x.PageId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_User_Page_Permissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Auth",
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Page_Permissions_SubModulePages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Auth",
                        principalTable: "SubModulePages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Page_Permissions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Module_SubModules_ModuleId_SubModuleId",
                schema: "Auth",
                table: "Module_SubModules",
                columns: new[] { "ModuleId", "SubModuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Module_SubModules_SubModuleId",
                schema: "Auth",
                table: "Module_SubModules",
                column: "SubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Module_ModuleName_ar",
                schema: "Auth",
                table: "Modules",
                column: "ModuleName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Module_ModuleName_en",
                schema: "Auth",
                table: "Modules",
                column: "ModuleName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PermissionName",
                schema: "Auth",
                table: "Permissions",
                column: "PermissionName",
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

            migrationBuilder.CreateIndex(
                name: "IX_Role_SubModules_ModuleId",
                schema: "Auth",
                table: "Role_SubModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_SubModules_SubModuleId",
                schema: "Auth",
                table: "Role_SubModules",
                column: "SubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleModules_RoleId_ModuleId_SubModuleId",
                schema: "Auth",
                table: "Role_SubModules",
                columns: new[] { "RoleId", "ModuleId", "SubModuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Auth",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Auth",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_SubModulePages_SubModuleId",
                schema: "Auth",
                table: "SubModulePages",
                column: "SubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubModules_SubModuleName_ar",
                schema: "Auth",
                table: "SubModules",
                column: "SubModuleName_ar",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubModules_SubModuleName_en",
                schema: "Auth",
                table: "SubModules",
                column: "SubModuleName_en",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Page_Permissions_PageId",
                schema: "Auth",
                table: "User_Page_Permissions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Page_Permissions_PermissionId",
                schema: "Auth",
                table: "User_Page_Permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPagePermissions_Unique",
                schema: "Auth",
                table: "User_Page_Permissions",
                columns: new[] { "UserId", "PageId", "PermissionId" },
                unique: true);

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
                name: "Module_SubModules",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role_Page_Permissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role_SubModulePages",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role_SubModules",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "User_Page_Permissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserLogins",
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
                name: "Modules",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "SubModulePages",
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
                name: "SubModules",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "LookUps");
        }
    }
}
