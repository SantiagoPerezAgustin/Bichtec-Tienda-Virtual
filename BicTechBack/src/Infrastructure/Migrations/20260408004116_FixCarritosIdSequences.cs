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
            // Sincroniza secuencias con columnas IDENTITY (no usar CREATE SEQUENCE + SET DEFAULT: PG lo rechaza).
            migrationBuilder.Sql(@"
SELECT setval(
  pg_get_serial_sequence('carritos', 'id'),
  GREATEST(COALESCE((SELECT MAX(id) FROM carritos), 0), 1),
  true
);
SELECT setval(
  pg_get_serial_sequence('carritos_detalles', 'id'),
  GREATEST(COALESCE((SELECT MAX(id) FROM carritos_detalles), 0), 1),
  true
);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
