using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerIdInAdEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_AspNetUsers_CreatedBy",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityAdvertisements_CreatedBy",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "IsCreatedByAdmin",
                table: "CelebrityAdvertisements");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "CelebrityAdvertisements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CelebrityAdvertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedByAdmin",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisements_CustomerId",
                table: "CelebrityAdvertisements",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityAdvertisements_Customers_CustomerId",
                table: "CelebrityAdvertisements",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityAdvertisements_Customers_CustomerId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityAdvertisements_CustomerId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "IsCreatedByAdmin",
                table: "Bookings");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsCreatedByAdmin",
                table: "CelebrityAdvertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }
    }
}
