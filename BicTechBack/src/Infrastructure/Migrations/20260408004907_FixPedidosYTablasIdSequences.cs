using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPedidosYTablasIdSequences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Solo setval sobre la secuencia de cada IDENTITY (compatible con PostgreSQL 10+).
            migrationBuilder.Sql(@"
SELECT setval(pg_get_serial_sequence('pedidos', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM pedidos), 0), 1), true);
SELECT setval(pg_get_serial_sequence('pedidos_detalles', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM pedidos_detalles), 0), 1), true);
SELECT setval(pg_get_serial_sequence('productos', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM productos), 0), 1), true);
SELECT setval(pg_get_serial_sequence('categorias', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM categorias), 0), 1), true);
SELECT setval(pg_get_serial_sequence('marcas', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM marcas), 0), 1), true);
SELECT setval(pg_get_serial_sequence('paises', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM paises), 0), 1), true);
SELECT setval(pg_get_serial_sequence('categorias_marcas', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM categorias_marcas), 0), 1), true);
SELECT setval(pg_get_serial_sequence('usuarios', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM usuarios), 0), 1), true);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
