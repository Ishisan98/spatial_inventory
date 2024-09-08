using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spatial_inventory_server.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_limits",
                columns: table => new
                {
                    limit_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_limit = table.Column<int>(type: "int", nullable: false),
                    product_limit = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_limits", x => x.limit_id);
                    table.ForeignKey(
                        name: "FK_user_limits_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_limits_userId",
                table: "user_limits",
                column: "userId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_limits");
        }
    }
}
