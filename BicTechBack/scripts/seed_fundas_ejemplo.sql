-- =============================================================================
-- Fundas de ejemplo (silicona + otras) para PostgreSQL / DBeaver
-- Tabla: productos (snake_case, como EF)
--
-- Requisitos: al menos UNA fila en la tabla marcas.
-- Si no existe categoría con "funda" en el nombre, se inserta "Fundas" automáticamente.
-- Los nombres de producto incluyen "Funda"/"Cover" para el filtro del front.
-- =============================================================================

BEGIN;

-- Si no tenés ninguna categoría con "funda" en el nombre, el script crea "Fundas" solo.
DO $$
DECLARE
  v_cat INT;
  v_marca INT := (SELECT id FROM marcas ORDER BY id LIMIT 1);
BEGIN
  -- Evita error "duplicate key (id)=..." si la secuencia quedó atrás del MAX(id)
  PERFORM setval(
    pg_get_serial_sequence('categorias', 'id'),
    COALESCE((SELECT MAX(id) FROM categorias), 0),
    true
  );

  INSERT INTO categorias (nombre)
  SELECT 'Fundas'
  WHERE NOT EXISTS (
    SELECT 1 FROM categorias WHERE nombre ILIKE '%funda%'
  );

  v_cat := (SELECT id FROM categorias WHERE nombre ILIKE '%funda%' ORDER BY id LIMIT 1);

  IF v_cat IS NULL THEN
    RAISE EXCEPTION 'No se pudo obtener categoría para fundas (revisá tabla categorias).';
  END IF;
  IF v_marca IS NULL THEN
    RAISE EXCEPTION 'No hay marcas en la tabla marcas. Creá al menos una marca.';
  END IF;

  PERFORM setval(
    pg_get_serial_sequence('productos', 'id'),
    COALESCE((SELECT MAX(id) FROM productos), 0),
    true
  );

  INSERT INTO productos (
    nombre,
    precio,
    descripcion,
    categoria_id,
    marca_id,
    stock,
    imagen_url,
    material_funda,
    color
  ) VALUES
  (
    'Funda silicona Samsung Galaxy A54',
    18500.00,
    'Funda flexible, buen agarre. Varios colores.',
    v_cat,
    v_marca,
    12,
    'https://images.unsplash.com/photo-1601784551446-20c9e07cdbdb?w=600&q=80',
    'Silicona',
    'Negro, Blanco, Celeste, Rosa'
  ),
  (
    'Funda silicona iPhone 15',
    22000.00,
    'Silicona suave, compatible MagSafe (según versión).',
    v_cat,
    v_marca,
    8,
    'https://images.unsplash.com/photo-1603313011108-b8780c8d4448?w=600&q=80',
    'Silicona',
    'Negro, Blanco, Azul marino'
  ),
  (
    'Funda silicona Xiaomi Redmi Note 13',
    16500.00,
    'Protección diaria, tacto mate.',
    v_cat,
    v_marca,
    15,
    'https://images.unsplash.com/photo-1580910051074-3eb694886efe?w=600&q=80',
    'Silicona',
    'Gris, Verde menta'
  ),
  (
    'Funda rigida TPU Motorola Edge 40',
    19900.00,
    'Bordes reforzados, no es silicona pura.',
    v_cat,
    v_marca,
    6,
    'https://images.unsplash.com/photo-1603313011108-b8780c8d4448?w=600&q=80',
    'Otro',
    NULL
  ),
  (
    'Funda transparente con soporte Galaxy S23',
    17500.00,
    'TPU + esquinas reforzadas.',
    v_cat,
    v_marca,
    9,
    'https://images.unsplash.com/photo-1601784551446-20c9e07cdbdb?w=600&q=80',
    'Otro',
    NULL
  ),
  (
    'Cover protector rigido iPhone 14',
    18900.00,
    'Case rígido, acabado mate.',
    v_cat,
    v_marca,
    7,
    'https://images.unsplash.com/photo-1592899677977-9c10ca588bbd?w=600&q=80',
    'Otro',
    NULL
  );

  PERFORM setval(
    pg_get_serial_sequence('productos', 'id'),
    COALESCE((SELECT MAX(id) FROM productos), 0),
    true
  );

END $$;

COMMIT;

-- Opcional: alinear secuencia de id (por si insertaste manual antes)
-- SELECT setval(
--   pg_get_serial_sequence('productos', 'id'),
--   COALESCE((SELECT MAX(id) FROM productos), 1)
-- );
