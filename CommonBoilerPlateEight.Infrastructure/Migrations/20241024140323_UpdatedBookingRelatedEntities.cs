using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBookingRelatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CelebrityTypes_CelebrityTypeId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityReviews_Celebrities_CelebrityId",
                table: "CelebrityReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityReviews_Customers_CustomerId",
                table: "CelebrityReviews");

            migrationBuilder.DropTable(
                name: "BookingToAds");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityReviews_CelebrityId",
                table: "CelebrityReviews");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityReviews_CustomerId",
                table: "CelebrityReviews");

            migrationBuilder.DropColumn(
                name: "CelebrityId",
                table: "CelebrityReviews");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CelebrityReviews");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Bookings",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "CelebrityTypeId",
                table: "Bookings",
                newName: "CelebrityId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CompanyId",
                table: "Bookings",
                newName: "IX_Bookings_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CelebrityTypeId",
                table: "Bookings",
                newName: "IX_Bookings_CelebrityId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdType",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedByAdmin",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CelebrityAdvertisement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CelebrityScheduleId = table.Column<int>(type: "int", nullable: false),
                    CompanyTypeId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManagerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DeliveryType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebrityAdvertisement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CelebrityAdvertisement_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CelebrityAdvertisement_CelebritySchedules_CelebrityScheduleId",
                        column: x => x.CelebrityScheduleId,
                        principalTable: "CelebritySchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CelebrityAdvertisement_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CelebrityAdvertisement_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CelebrityAdvertismentQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    TextAnswer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SelectedOptionId = table.Column<int>(type: "int", nullable: true),
                    DateAnswer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberAnswer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebrityAdvertismentQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CelebrityAdvertismentQuestions_CelebrityAdvertisement_AdId",
                        column: x => x.AdId,
                        principalTable: "CelebrityAdvertisement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CelebrityAdvertismentQuestions_QuestionSettings_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisement_BookingId",
                table: "CelebrityAdvertisement",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisement_CelebrityScheduleId",
                table: "CelebrityAdvertisement",
                column: "CelebrityScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisement_CompanyTypeId",
                table: "CelebrityAdvertisement",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisement_CountryId",
                table: "CelebrityAdvertisement",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertismentQuestions_AdId",
                table: "CelebrityAdvertismentQuestions",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertismentQuestions_QuestionId",
                table: "CelebrityAdvertismentQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedBy",
                table: "Events",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CustomerId",
                table: "Events",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_CreatedBy",
                table: "Bookings",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Celebrities_CelebrityId",
                table: "Bookings",
                column: "CelebrityId",
                principalTable: "Celebrities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_CreatedBy",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Celebrities_CelebrityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "CelebrityAdvertismentQuestions");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "CelebrityAdvertisement");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "IsCreatedByAdmin",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Bookings",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "CelebrityId",
                table: "Bookings",
                newName: "CelebrityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_EventId",
                table: "Bookings",
                newName: "IX_Bookings_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CelebrityId",
                table: "Bookings",
                newName: "IX_Bookings_CelebrityTypeId");

            migrationBuilder.AddColumn<int>(
                name: "CelebrityId",
                table: "CelebrityReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CelebrityReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdType",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CelebrityId = table.Column<int>(type: "int", nullable: false),
                    AdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisements_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisements_Celebrities_CelebrityId",
                        column: x => x.CelebrityId,
                        principalTable: "Celebrities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingToAds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingToAds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingToAds_Advertisements_AdId",
                        column: x => x.AdId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingToAds_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingToAds_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityReviews_CelebrityId",
                table: "CelebrityReviews",
                column: "CelebrityId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityReviews_CustomerId",
                table: "CelebrityReviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_BookingId",
                table: "Advertisements",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CelebrityId",
                table: "Advertisements",
                column: "CelebrityId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingToAds_AdId",
                table: "BookingToAds",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingToAds_BookingId",
                table: "BookingToAds",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingToAds_CompanyId",
                table: "BookingToAds",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyTypeId",
                table: "Companies",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CustomerId",
                table: "Companies",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CelebrityTypes_CelebrityTypeId",
                table: "Bookings",
                column: "CelebrityTypeId",
                principalTable: "CelebrityTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Companies_CompanyId",
                table: "Bookings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityReviews_Celebrities_CelebrityId",
                table: "CelebrityReviews",
                column: "CelebrityId",
                principalTable: "Celebrities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityReviews_Customers_CustomerId",
                table: "CelebrityReviews",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
