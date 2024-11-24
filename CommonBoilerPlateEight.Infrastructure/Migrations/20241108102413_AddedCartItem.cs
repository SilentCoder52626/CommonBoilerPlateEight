using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CelebrityId = table.Column<int>(type: "int", nullable: false),
                    CompanyTypeId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ManagerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DeliveryType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItemQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartItemId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    TextAnswer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SelectedOptionId = table.Column<int>(type: "int", nullable: true),
                    DateAnswer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberAnswer = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItemQuestions_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItemQuestions_QuestionSettings_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemQuestions_CartItemId",
                table: "CartItemQuestions",
                column: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemQuestions_QuestionId",
                table: "CartItemQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CustomerId",
                table: "CartItems",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemQuestions");

            migrationBuilder.DropTable(
                name: "CartItems");
        }
    }
}
