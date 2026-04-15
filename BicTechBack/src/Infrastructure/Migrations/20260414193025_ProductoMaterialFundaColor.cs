using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductoMaterialFundaColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "productos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "material_funda",
                table: "productos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "material_funda",
                table: "productos");
        }
    }
}
