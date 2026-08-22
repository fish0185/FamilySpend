using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilySpend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserOrderCategories_UserId_OrderCategoryId",
                table: "UserOrderCategories",
                columns: new[] { "UserId", "OrderCategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserOrderCategories_UserId_OrderCategoryId",
                table: "UserOrderCategories");
        }
    }
}
