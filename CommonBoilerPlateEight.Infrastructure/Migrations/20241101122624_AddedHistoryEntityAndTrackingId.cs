using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedHistoryEntityAndTrackingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingId",
                table: "CelebrityAdvertisements",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrackingId",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BookingHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingHistory_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CelebrityAdHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AdId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebrityAdHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CelebrityAdHistory_CelebrityAdvertisements_AdId",
                        column: x => x.AdId,
                        principalTable: "CelebrityAdvertisements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdvertisements_TrackingId",
                table: "CelebrityAdvertisements",
                column: "TrackingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TrackingId",
                table: "Bookings",
                column: "TrackingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_BookingId",
                table: "BookingHistory",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityAdHistory_AdId",
                table: "CelebrityAdHistory",
                column: "AdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingHistory");

            migrationBuilder.DropTable(
                name: "CelebrityAdHistory");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityAdvertisements_TrackingId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TrackingId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "CelebrityAdvertisements");

            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "Bookings");
        }
    }
}
