using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Celebrities_CelebrityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityReviews_Bookings_BookingId",
                table: "CelebrityReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_CreatedBy",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Customers_CustomerId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CelebrityId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_EventId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CreatedBy",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdType",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CelebrityId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "IsCreatedByAdmin",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "CelebrityReviews",
                newName: "AdId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityReviews_BookingId",
                table: "CelebrityReviews",
                newName: "IX_CelebrityReviews_AdId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_CustomerId",
                table: "Event",
                newName: "IX_Event_CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "CelebrityAdvertisements",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDate",
                table: "CelebrityAdvertisements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "CelebrityAdvertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CelebrityId",
                table: "CelebrityAdvertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedByAdmin",
                table: "CelebrityAdvertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Event",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisements_CelebrityId",
                table: "CelebrityAdvertisements",
                column: "CelebrityId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisements_CreatedBy",
                table: "CelebrityAdvertisements",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisements_AspNetUsers_CreatedBy",
                table: "CelebrityAdvertisements",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisements_Celebrities_CelebrityId",
                table: "CelebrityAdvertisements",
                column: "CelebrityId",
                principalTable: "Celebrities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityReviews_CelebrityAdvertisements_AdId",
                table: "CelebrityReviews",
                column: "AdId",
                principalTable: "CelebrityAdvertisements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Customers_CustomerId",
                table: "Event",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_AspNetUsers_CreatedBy",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_Celebrities_CelebrityId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityReviews_CelebrityAdvertisements_AdId",
                table: "CelebrityReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Customers_CustomerId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityAdvertisements_CelebrityId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityAdvertisements_CreatedBy",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "CelebrityId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "IsCreatedByAdmin",
                table: "CelebrityAdvertisements");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameColumn(
                name: "AdId",
                table: "CelebrityReviews",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_CelebrityReviews_AdId",
                table: "CelebrityReviews",
                newName: "IX_CelebrityReviews_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Event_CustomerId",
                table: "Events",
                newName: "IX_Events_CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "CelebrityAdvertisements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdType",
                table: "Bookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CelebrityId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedByAdmin",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CelebrityId",
                table: "Bookings",
                column: "CelebrityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EventId",
                table: "Bookings",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedBy",
                table: "Events",
                column: "CreatedBy");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityReviews_Bookings_BookingId",
                table: "CelebrityReviews",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_CreatedBy",
                table: "Events",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Customers_CustomerId",
                table: "Events",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
