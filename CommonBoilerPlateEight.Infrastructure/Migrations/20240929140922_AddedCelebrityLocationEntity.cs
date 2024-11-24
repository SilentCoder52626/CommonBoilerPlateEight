using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCelebrityLocationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CelebrityLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,15)", precision: 18, scale: 15, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Block = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    GooglePlusCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: false, computedColumnSql: "Geography::Point(Latitude, Longitude, 4326)", stored: true),
                    CelebrityId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebrityLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CelebrityLocations_Celebrities_CelebrityId",
                        column: x => x.CelebrityId,
                        principalTable: "Celebrities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityLocations_CelebrityId",
                table: "CelebrityLocations",
                column: "CelebrityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CelebrityLocations");
        }
    }
}
