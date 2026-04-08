using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCarritosIdSequences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Repara id sin DEFAULT (error PostgreSQL 23502 al insertar en carritos).
            migrationBuilder.Sql(@"
CREATE SEQUENCE IF NOT EXISTS carritos_id_seq;
SELECT setval(
  'carritos_id_seq',
  GREATEST(COALESCE((SELECT MAX(id) FROM carritos), 0), 1),
  true
);
ALTER TABLE carritos ALTER COLUMN id SET DEFAULT nextval('carritos_id_seq');
ALTER SEQUENCE carritos_id_seq OWNED BY carritos.id;

CREATE SEQUENCE IF NOT EXISTS carritos_detalles_id_seq;
SELECT setval(
  'carritos_detalles_id_seq',
  GREATEST(COALESCE((SELECT MAX(id) FROM carritos_detalles), 0), 1),
  true
);
ALTER TABLE carritos_detalles ALTER COLUMN id SET DEFAULT nextval('carritos_detalles_id_seq');
ALTER SEQUENCE carritos_detalles_id_seq OWNED BY carritos_detalles.id;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
