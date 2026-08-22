using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilySpend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCategoryId",
                table: "UserOrderCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderCategoryId",
                table: "UserOrderCategories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
