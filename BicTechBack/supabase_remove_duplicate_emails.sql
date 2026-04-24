-- Quitar usuarios duplicados por mismo email (ignora mayúsculas y espacios al inicio/fin).
-- Ejecutá en Supabase: primero el bloque 1 (solo lectura), revisá, luego el bloque 2.
--
-- Regla: por cada email normalizado se conserva el usuario con MENOR id; el resto se borra.
-- Si un duplicado tenía carritos/pedidos y la FK es ON DELETE CASCADE, esos datos se borran con él.
-- Si la FK es RESTRICT, el DELETE fallará: en ese caso borrá o reasigná manualmente.

-- ========== 1) Vista previa (no modifica nada) ==========
SELECT lower(btrim(email)) AS email_norm,
       count(*)            AS cantidad,
       array_agg(id ORDER BY id) AS ids_en_orden
FROM public.usuarios
WHERE email IS NOT NULL AND btrim(email) <> ''
GROUP BY 1
HAVING count(*) > 1
ORDER BY email_norm;

-- Detalle de filas que serían eliminadas (rn > 1):
WITH ranked AS (
  SELECT id,
         nombre,
         email,
         ROW_NUMBER() OVER (PARTITION BY lower(btrim(email)) ORDER BY id) AS rn
  FROM public.usuarios
  WHERE email IS NOT NULL AND btrim(email) <> ''
)
SELECT id, nombre, email
FROM ranked
WHERE rn > 1
ORDER BY lower(btrim(email)), id;

-- ========== 2) Borrado (ejecutá solo cuando estés conforme con la vista previa) ==========
BEGIN;

WITH ranked AS (
  SELECT id,
         ROW_NUMBER() OVER (PARTITION BY lower(btrim(email)) ORDER BY id) AS rn
  FROM public.usuarios
  WHERE email IS NOT NULL AND btrim(email) <> ''
)
DELETE FROM public.usuarios u
WHERE u.id IN (SELECT id FROM ranked WHERE rn > 1);

COMMIT;

-- ========== 3) Opcional: evitar nuevos duplicados (fallará si aún quedan duplicados) ==========
-- CREATE UNIQUE INDEX IF NOT EXISTS usuarios_email_lower_unique
-- ON public.usuarios (lower(btrim(email)));
