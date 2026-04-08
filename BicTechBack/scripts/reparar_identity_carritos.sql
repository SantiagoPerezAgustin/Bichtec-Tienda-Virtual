-- column_default vacío en carritos.id y carritos_detalles.id → error 23502 al insertar.
-- Ejecutar TODO el script en la base de la API (pgAdmin: Query Tool).

-- carritos.id
CREATE SEQUENCE IF NOT EXISTS carritos_id_seq;
SELECT setval(
  'carritos_id_seq',
  GREATEST(COALESCE((SELECT MAX(id) FROM carritos), 0), 1),
  true
);
ALTER TABLE carritos ALTER COLUMN id SET DEFAULT nextval('carritos_id_seq');
ALTER SEQUENCE carritos_id_seq OWNED BY carritos.id;

-- carritos_detalles.id
CREATE SEQUENCE IF NOT EXISTS carritos_detalles_id_seq;
SELECT setval(
  'carritos_detalles_id_seq',
  GREATEST(COALESCE((SELECT MAX(id) FROM carritos_detalles), 0), 1),
  true
);
ALTER TABLE carritos_detalles ALTER COLUMN id SET DEFAULT nextval('carritos_detalles_id_seq');
ALTER SEQUENCE carritos_detalles_id_seq OWNED BY carritos_detalles.id;

-- Comprobar (debe mostrar nextval(...) en column_default):
-- SELECT table_name, column_name, column_default
-- FROM information_schema.columns
-- WHERE table_schema = 'public' AND table_name IN ('carritos', 'carritos_detalles') AND column_name = 'id';
