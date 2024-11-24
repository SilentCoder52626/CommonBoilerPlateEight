using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonBoilerPlateEight.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceRangeInCelebrity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PriceRange",
                table: "Celebrities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceRange",
                table: "Celebrities");
        }
    }
}
