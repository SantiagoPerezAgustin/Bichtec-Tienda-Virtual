using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <summary>
    /// Unifica el nombre físico de la columna con snake_case. En PG, "Password" y password son distintos;
    /// Render a menudo tiene solo password (migración SyncPendingModel no aplicada).
    /// </summary>
    public partial class AlignUsuarioPasswordColumnSnakeCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DO $$
BEGIN
  IF EXISTS (
    SELECT 1 FROM pg_attribute a
    INNER JOIN pg_class c ON c.oid = a.attrelid
    INNER JOIN pg_namespace n ON n.oid = c.relnamespace
    WHERE n.nspname = 'public' AND c.relname = 'usuarios'
      AND a.attname = 'Password' AND a.attnum > 0 AND NOT a.attisdropped
  ) THEN
    EXECUTE 'ALTER TABLE usuarios RENAME COLUMN ""Password"" TO password';
  END IF;
END $$;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DO $$
BEGIN
  IF EXISTS (
    SELECT 1 FROM pg_attribute a
    INNER JOIN pg_class c ON c.oid = a.attrelid
    INNER JOIN pg_namespace n ON n.oid = c.relnamespace
    WHERE n.nspname = 'public' AND c.relname = 'usuarios'
      AND a.attname = 'password' AND a.attnum > 0 AND NOT a.attisdropped
  ) THEN
    EXECUTE 'ALTER TABLE usuarios RENAME COLUMN password TO ""Password""';
  END IF;
END $$;
");
        }
    }
}
