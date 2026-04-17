/**
 * Valor numérico en pesos (tal cual el backend: decimal en ARS).
 * No se aplica ninguna conversión automática: antes multiplicar por 1000 si era &lt; 1000
 * rompía precios reales bajos (ej. $500 → $500.000) y mezclaba criterios.
 */
export function valorPrecioDisplay(precio) {
  const num = Number(precio);
  if (Number.isNaN(num)) return 0;
  return num;
}

/**
 * Formato visual: separador miles AR y sufijo ARS (ej. $300.000 ARS).
 */
export function formatPrecioARS(precio, { conSimboloPeso = true } = {}) {
  const n = valorPrecioDisplay(precio);
  const formatted = n.toLocaleString("es-AR", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  });
  return conSimboloPeso ? `$${formatted} ARS` : `${formatted} ARS`;
}

/** Solo número formateado + ARS, sin $ (útil en textos que ya dicen "Precio:") */
export function formatMontoARS(numero) {
  const n = Number(numero);
  if (Number.isNaN(n)) return `0 ARS`;
  const formatted = n.toLocaleString("es-AR", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  });
  return `$${formatted} ARS`;
}
