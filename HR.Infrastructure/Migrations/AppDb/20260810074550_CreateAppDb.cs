using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class CreateAppDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LookUps");

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
                name: "Governorate",
                schema: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country_Id = table.Column<int>(type: "int", nullable: false),
                    GovName_en = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GovName_ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GovCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
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
                name: "City",
                schema: "LookUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country_Id = table.Column<int>(type: "int", nullable: false),
                    Gov_Id = table.Column<int>(type: "int", nullable: false),
                    CityName_en = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CityName_ar = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Country_Country_Id",
                        column: x => x.Country_Id,
                        principalSchema: "LookUps",
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_City_Governorate_Gov_Id",
                        column: x => x.Gov_Id,
                        principalSchema: "LookUps",
                        principalTable: "Governorate",
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
                    Country_Id = table.Column<int>(type: "int", nullable: false),
                    Gov_Id = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Company_Country_Country_Id",
                        column: x => x.Country_Id,
                        principalSchema: "LookUps",
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Governorate_Gov_Id",
                        column: x => x.Gov_Id,
                        principalSchema: "LookUps",
                        principalTable: "Governorate",
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
                name: "IX_City_Country_Id",
                schema: "LookUps",
                table: "City",
                column: "Country_Id");

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
                name: "IX_Company_Country_Id",
                schema: "LookUps",
                table: "Company",
                column: "Country_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Gov_Id",
                schema: "LookUps",
                table: "Company",
                column: "Gov_Id");

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
                name: "City",
                schema: "LookUps");

            migrationBuilder.DropTable(
                name: "Governorate",
                schema: "LookUps");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "LookUps");
        }
    }
}
