-- Conéctate a la misma base que en appsettings (PostgreSQLConnection).
--
-- Parte 1 (obligatoria): borra solo los 3 productos de ejemplo del seeder.
-- Parte 2 (opcional): borra categorías/marca/país con esos nombres SOLO si no los usás para datos reales.

BEGIN;

-- Parte 1
DELETE FROM "CarritosDetalles"
WHERE "ProductoId" IN (
  SELECT "Id" FROM "Productos"
  WHERE "Nombre" IN ('CPU Ryzen 5', 'RAM DDR4 16GB', 'SSD NVMe 1TB')
);

DELETE FROM "PedidosDetalles"
WHERE "ProductoId" IN (
  SELECT "Id" FROM "Productos"
  WHERE "Nombre" IN ('CPU Ryzen 5', 'RAM DDR4 16GB', 'SSD NVMe 1TB')
);

DELETE FROM "Productos"
WHERE "Nombre" IN ('CPU Ryzen 5', 'RAM DDR4 16GB', 'SSD NVMe 1TB');

-- Parte 2 (opcional: descomentá solo si esos nombres eran solo del seed)
-- DELETE FROM "CategoriasMarcas"
-- WHERE "CategoriaId" IN (
--   SELECT "Id" FROM "Categorias"
--   WHERE "Nombre" IN ('Procesadores', 'Memorias RAM', 'Almacenamiento')
-- )
--    OR "MarcaId" IN (SELECT "Id" FROM "Marcas" WHERE "Nombre" = 'BicTech');
--
-- DELETE FROM "Categorias"
-- WHERE "Nombre" IN ('Procesadores', 'Memorias RAM', 'Almacenamiento');
--
-- DELETE FROM "Marcas"
-- WHERE "Nombre" = 'BicTech';
--
-- DELETE FROM "Paises"
-- WHERE "Nombre" = 'Argentina';

COMMIT;
