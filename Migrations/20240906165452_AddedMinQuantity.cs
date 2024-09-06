using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spatial_inventory_server.Migrations
{
    /// <inheritdoc />
    public partial class AddedMinQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "min_quantity",
                table: "product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "min_quantity",
                table: "product");
        }
    }
}
