using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedBaseEntityOnHistoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHistory_Bookings_BookingId",
                table: "BookingHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdHistory_CelebrityAdvertisements_AdId",
                table: "CelebrityAdHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CelebrityAdHistory",
                table: "CelebrityAdHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingHistory",
                table: "BookingHistory");

            migrationBuilder.RenameTable(
                name: "CelebrityAdHistory",
                newName: "CelebrityAdHistories");

            migrationBuilder.RenameTable(
                name: "BookingHistory",
                newName: "BookingHistories");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdHistory_AdId",
                table: "CelebrityAdHistories",
                newName: "IX_CelebrityAdHistories_AdId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingHistory_BookingId",
                table: "BookingHistories",
                newName: "IX_BookingHistories_BookingId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CelebrityAdHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CelebrityAdHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "CelebrityAdHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CelebrityAdHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BookingHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BookingHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "BookingHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "BookingHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CelebrityAdHistories",
                table: "CelebrityAdHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingHistories",
                table: "BookingHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHistories_Bookings_BookingId",
                table: "BookingHistories",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdHistories_CelebrityAdvertisements_AdId",
                table: "CelebrityAdHistories",
                column: "AdId",
                principalTable: "CelebrityAdvertisements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingHistories_Bookings_BookingId",
                table: "BookingHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdHistories_CelebrityAdvertisements_AdId",
                table: "CelebrityAdHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CelebrityAdHistories",
                table: "CelebrityAdHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingHistories",
                table: "BookingHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CelebrityAdHistories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CelebrityAdHistories");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "CelebrityAdHistories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CelebrityAdHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BookingHistories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BookingHistories");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "BookingHistories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "BookingHistories");

            migrationBuilder.RenameTable(
                name: "CelebrityAdHistories",
                newName: "CelebrityAdHistory");

            migrationBuilder.RenameTable(
                name: "BookingHistories",
                newName: "BookingHistory");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityAdHistories_AdId",
                table: "CelebrityAdHistory",
                newName: "IX_CelebrityAdHistory_AdId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingHistories_BookingId",
                table: "BookingHistory",
                newName: "IX_BookingHistory_BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CelebrityAdHistory",
                table: "CelebrityAdHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingHistory",
                table: "BookingHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingHistory_Bookings_BookingId",
                table: "BookingHistory",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdHistory_CelebrityAdvertisements_AdId",
                table: "CelebrityAdHistory",
                column: "AdId",
                principalTable: "CelebrityAdvertisements",
                principalColumn: "Id");
        }
    }
}
