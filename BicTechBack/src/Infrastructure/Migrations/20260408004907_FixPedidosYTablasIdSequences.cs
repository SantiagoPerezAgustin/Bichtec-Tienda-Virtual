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
            // Mismo problema que carritos: columnas id sin DEFAULT → 23502 u otros errores al leer/escribir.
            migrationBuilder.Sql(@"
CREATE SEQUENCE IF NOT EXISTS pedidos_id_seq;
SELECT setval('pedidos_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM pedidos), 0), 1), true);
ALTER TABLE pedidos ALTER COLUMN id SET DEFAULT nextval('pedidos_id_seq');
ALTER SEQUENCE pedidos_id_seq OWNED BY pedidos.id;

CREATE SEQUENCE IF NOT EXISTS pedidos_detalles_id_seq;
SELECT setval('pedidos_detalles_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM pedidos_detalles), 0), 1), true);
ALTER TABLE pedidos_detalles ALTER COLUMN id SET DEFAULT nextval('pedidos_detalles_id_seq');
ALTER SEQUENCE pedidos_detalles_id_seq OWNED BY pedidos_detalles.id;

CREATE SEQUENCE IF NOT EXISTS productos_id_seq;
SELECT setval('productos_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM productos), 0), 1), true);
ALTER TABLE productos ALTER COLUMN id SET DEFAULT nextval('productos_id_seq');
ALTER SEQUENCE productos_id_seq OWNED BY productos.id;

CREATE SEQUENCE IF NOT EXISTS categorias_id_seq;
SELECT setval('categorias_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM categorias), 0), 1), true);
ALTER TABLE categorias ALTER COLUMN id SET DEFAULT nextval('categorias_id_seq');
ALTER SEQUENCE categorias_id_seq OWNED BY categorias.id;

CREATE SEQUENCE IF NOT EXISTS marcas_id_seq;
SELECT setval('marcas_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM marcas), 0), 1), true);
ALTER TABLE marcas ALTER COLUMN id SET DEFAULT nextval('marcas_id_seq');
ALTER SEQUENCE marcas_id_seq OWNED BY marcas.id;

CREATE SEQUENCE IF NOT EXISTS paises_id_seq;
SELECT setval('paises_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM paises), 0), 1), true);
ALTER TABLE paises ALTER COLUMN id SET DEFAULT nextval('paises_id_seq');
ALTER SEQUENCE paises_id_seq OWNED BY paises.id;

CREATE SEQUENCE IF NOT EXISTS categorias_marcas_id_seq;
SELECT setval('categorias_marcas_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM categorias_marcas), 0), 1), true);
ALTER TABLE categorias_marcas ALTER COLUMN id SET DEFAULT nextval('categorias_marcas_id_seq');
ALTER SEQUENCE categorias_marcas_id_seq OWNED BY categorias_marcas.id;

CREATE SEQUENCE IF NOT EXISTS usuarios_id_seq;
SELECT setval('usuarios_id_seq', GREATEST(COALESCE((SELECT MAX(id) FROM usuarios), 0), 1), true);
ALTER TABLE usuarios ALTER COLUMN id SET DEFAULT nextval('usuarios_id_seq');
ALTER SEQUENCE usuarios_id_seq OWNED BY usuarios.id;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
