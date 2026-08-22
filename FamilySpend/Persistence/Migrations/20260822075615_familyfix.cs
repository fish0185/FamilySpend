using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilySpend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class familyfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyLinks_ZipUser_FamilyUserId",
                table: "FamilyLinks");

            migrationBuilder.DropTable(
                name: "ZipUser");

            migrationBuilder.DropIndex(
                name: "IX_FamilyLinks_FamilyUserId",
                table: "FamilyLinks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZipUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyLinks_FamilyUserId",
                table: "FamilyLinks",
                column: "FamilyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyLinks_ZipUser_FamilyUserId",
                table: "FamilyLinks",
                column: "FamilyUserId",
                principalTable: "ZipUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
