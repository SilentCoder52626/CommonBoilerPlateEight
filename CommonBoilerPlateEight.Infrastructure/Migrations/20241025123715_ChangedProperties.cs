using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisement_Bookings_BookingId",
                table: "CelebrityAdvertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisement_CelebritySchedules_CelebrityScheduleId",
                table: "CelebrityAdvertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisement_CompanyTypes_CompanyTypeId",
                table: "CelebrityAdvertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisement_Countries_CountryId",
                table: "CelebrityAdvertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertismentQuestions_CelebrityAdvertisement_AdId",
                table: "CelebrityAdvertismentQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CelebrityAdvertisement",
                table: "CelebrityAdvertisement");

            migrationBuilder.RenameTable(
                name: "CelebrityAdvertisement",
                newName: "CelebrityAdvertisements");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisement_CountryId",
                table: "CelebrityAdvertisements",
                newName: "IX_CelebrityAdvertisements_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisement_CompanyTypeId",
                table: "CelebrityAdvertisements",
                newName: "IX_CelebrityAdvertisements_CompanyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisement_CelebrityScheduleId",
                table: "CelebrityAdvertisements",
                newName: "IX_CelebrityAdvertisements_CelebrityScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisement_BookingId",
                table: "CelebrityAdvertisements",
                newName: "IX_CelebrityAdvertisements_BookingId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "CelebrityReviews",
                type: "decimal(3,1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "CelebrityAdvertisements",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "AdPrice",
                table: "CelebrityAdvertisements",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CelebrityAdvertisements",
                table: "CelebrityAdvertisements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisements_Bookings_BookingId",
                table: "CelebrityAdvertisements",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisements_CelebritySchedules_CelebrityScheduleId",
                table: "CelebrityAdvertisements",
                column: "CelebrityScheduleId",
                principalTable: "CelebritySchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisements_CompanyTypes_CompanyTypeId",
                table: "CelebrityAdvertisements",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisements_Countries_CountryId",
                table: "CelebrityAdvertisements",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertismentQuestions_CelebrityAdvertisements_AdId",
                table: "CelebrityAdvertismentQuestions",
                column: "AdId",
                principalTable: "CelebrityAdvertisements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_Bookings_BookingId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_CelebritySchedules_CelebrityScheduleId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_CompanyTypes_CompanyTypeId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_Countries_CountryId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertismentQuestions_CelebrityAdvertisements_AdId",
                table: "CelebrityAdvertismentQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CelebrityAdvertisements",
                table: "CelebrityAdvertisements");

            migrationBuilder.RenameTable(
                name: "CelebrityAdvertisements",
                newName: "CelebrityAdvertisement");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisements_CountryId",
                table: "CelebrityAdvertisement",
                newName: "IX_CelebrityAdvertisement_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisements_CompanyTypeId",
                table: "CelebrityAdvertisement",
                newName: "IX_CelebrityAdvertisement_CompanyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisements_CelebrityScheduleId",
                table: "CelebrityAdvertisement",
                newName: "IX_CelebrityAdvertisement_CelebrityScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdvertisements_BookingId",
                table: "CelebrityAdvertisement",
                newName: "IX_CelebrityAdvertisement_BookingId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "CelebrityReviews",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "CelebrityAdvertisement",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<decimal>(
                name: "AdPrice",
                table: "CelebrityAdvertisement",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CelebrityAdvertisement",
                table: "CelebrityAdvertisement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisement_Bookings_BookingId",
                table: "CelebrityAdvertisement",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisement_CelebritySchedules_CelebrityScheduleId",
                table: "CelebrityAdvertisement",
                column: "CelebrityScheduleId",
                principalTable: "CelebritySchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisement_CompanyTypes_CompanyTypeId",
                table: "CelebrityAdvertisement",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisement_Countries_CountryId",
                table: "CelebrityAdvertisement",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertismentQuestions_CelebrityAdvertisement_AdId",
                table: "CelebrityAdvertismentQuestions",
                column: "AdId",
                principalTable: "CelebrityAdvertisement",
                principalColumn: "Id");
        }
    }
}
