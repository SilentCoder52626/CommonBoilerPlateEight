using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedApprovedByPropertyInReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedById",
                table: "CelebrityReviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByAdmin",
                table: "CelebrityReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CelebrityReviews_ApprovedById",
                table: "CelebrityReviews",
                column: "ApprovedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CelebrityReviews_AspNetUsers_ApprovedById",
                table: "CelebrityReviews",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CelebrityReviews_AspNetUsers_ApprovedById",
                table: "CelebrityReviews");

            migrationBuilder.DropIndex(
                name: "IX_CelebrityReviews_ApprovedById",
                table: "CelebrityReviews");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "CelebrityReviews");

            migrationBuilder.DropColumn(
                name: "IsApprovedByAdmin",
                table: "CelebrityReviews");
        }
    }
}
