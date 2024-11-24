using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertiesInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CelebrityScheduleId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CelebrityId",
                table: "CartItems",
                column: "CelebrityId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CelebrityScheduleId",
                table: "CartItems",
                column: "CelebrityScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CompanyTypeId",
                table: "CartItems",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CountryId",
                table: "CartItems",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Celebrities_CelebrityId",
                table: "CartItems",
                column: "CelebrityId",
                principalTable: "Celebrities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_CelebritySchedules_CelebrityScheduleId",
                table: "CartItems",
                column: "CelebrityScheduleId",
                principalTable: "CelebritySchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_CompanyTypes_CompanyTypeId",
                table: "CartItems",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Countries_CountryId",
                table: "CartItems",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Celebrities_CelebrityId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_CelebritySchedules_CelebrityScheduleId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_CompanyTypes_CompanyTypeId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Countries_CountryId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CelebrityId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CelebrityScheduleId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CompanyTypeId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CountryId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CelebrityScheduleId",
                table: "CartItems");
        }
    }
}
