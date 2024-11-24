using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Celebrities_CountryId_MobileNumber",
                table: "Celebrities");

            migrationBuilder.DropIndex(
                name: "IX_Celebrities_Email",
                table: "Celebrities");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOnline",
                table: "Celebrities",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ProfilePictureURL = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    AuthenticationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsCreatedByAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RejectedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OTP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OTPCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_RejectedById",
                        column: x => x.RejectedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerToCelebrityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CelebrityTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerToCelebrityTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerToCelebrityTypes_CelebrityTypes_CelebrityTypeId",
                        column: x => x.CelebrityTypeId,
                        principalTable: "CelebrityTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerToCelebrityTypes_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_CountryId_MobileNumber",
                table: "Celebrities",
                columns: new[] { "CountryId", "MobileNumber" },
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_Email",
                table: "Celebrities",
                column: "Email",
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ApprovedById",
                table: "Customers",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryId_MobileNumber",
                table: "Customers",
                columns: new[] { "CountryId", "MobileNumber" },
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedBy",
                table: "Customers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true,
                filter: "[DeletedDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_RejectedById",
                table: "Customers",
                column: "RejectedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerToCelebrityTypes_CelebrityTypeId",
                table: "CustomerToCelebrityTypes",
                column: "CelebrityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerToCelebrityTypes_CustomerId",
                table: "CustomerToCelebrityTypes",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerToCelebrityTypes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Celebrities_CountryId_MobileNumber",
                table: "Celebrities");

            migrationBuilder.DropIndex(
                name: "IX_Celebrities_Email",
                table: "Celebrities");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOnline",
                table: "Celebrities",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_CountryId_MobileNumber",
                table: "Celebrities",
                columns: new[] { "CountryId", "MobileNumber" },
                unique: true,
                filter: "[CountryId] IS NOT NULL AND [MobileNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Celebrities_Email",
                table: "Celebrities",
                column: "Email",
                unique: true);
        }
    }
}
