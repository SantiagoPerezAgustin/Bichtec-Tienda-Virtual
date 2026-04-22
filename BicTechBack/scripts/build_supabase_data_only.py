"""Genera supabase_data_only.sql desde el dump de pg_dump (solo datos, orden FK)."""
import re
from pathlib import Path

DUMP = Path(r"c:\Users\santi\Desktop\dump-bichtech_postgres-202604172038.sql")
OUT = Path(__file__).resolve().parent.parent / "supabase_data_only.sql"

ORDER = [
    "paises",
    "usuarios",
    "categorias",
    "marcas",
    "categorias_marcas",
    "productos",
    "carritos",
    "pedidos",
    "carritos_detalles",
    "pedidos_detalles",
]


def table_from_insert(line: str):
    m = re.match(r'^INSERT INTO public\.(?:"([^"]+)"|([a-z_]+))', line)
    if not m:
        return None
    return (m.group(1) or m.group(2)).lower()


def main() -> None:
    text = DUMP.read_text(encoding="utf-8")
    lines = [ln.rstrip() for ln in text.splitlines()]
    grouped: dict[str, list[str]] = {t: [] for t in ORDER}
    for line in lines:
        if not line.startswith("INSERT INTO public."):
            continue
        t = table_from_insert(line)
        if t is None:
            continue
        if t == "__efmigrationshistory":
            continue
        if t in grouped:
            grouped[t].append(line if line.endswith(";") else line + ";")
        else:
            raise SystemExit(f"Tabla no esperada en el dump: {t}")

    parts: list[str] = [
        "-- Importar solo DATOS (Render/pg_dump) en Supabase donde ya existe el esquema (supabase_migrate.sql).",
        "-- 1) Ejecutá esto en Supabase SQL Editor.",
        "-- 2) No incluye __EFMigrationsHistory (ya lo tenés aplicado).",
        "",
        "BEGIN;",
        "",
        "TRUNCATE TABLE",
        "  carritos_detalles,",
        "  pedidos_detalles,",
        "  pedidos,",
        "  carritos,",
        "  categorias_marcas,",
        "  productos,",
        "  marcas,",
        "  categorias,",
        "  usuarios,",
        "  paises",
        "RESTART IDENTITY CASCADE;",
        "",
    ]
    for t in ORDER:
        parts.append(f"-- {t}")
        parts.extend(grouped[t])
        parts.append("")
    parts.extend(
        [
            "-- Ajustar secuencias (mismo criterio que supabase_migrate.sql)",
            "SELECT setval(pg_get_serial_sequence('paises', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM paises), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('usuarios', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM usuarios), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('categorias', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM categorias), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('marcas', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM marcas), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('categorias_marcas', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM categorias_marcas), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('productos', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM productos), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('carritos', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM carritos), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('carritos_detalles', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM carritos_detalles), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('pedidos', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM pedidos), 0), 1), true);",
            "SELECT setval(pg_get_serial_sequence('pedidos_detalles', 'id'), GREATEST(COALESCE((SELECT MAX(id) FROM pedidos_detalles), 0), 1), true);",
            "",
            "COMMIT;",
        ]
    )
    OUT.write_text("\n".join(parts), encoding="utf-8")
    print(f"Escrito: {OUT} ({OUT.stat().st_size} bytes)")


if __name__ == "__main__":
    main()
