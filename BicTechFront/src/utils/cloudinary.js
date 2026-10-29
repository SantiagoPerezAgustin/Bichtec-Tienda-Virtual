const CLOUD_NAME = import.meta.env.VITE_CLOUDINARY_CLOUD_NAME;
const PLACEHOLDER = "https://placehold.co/600x450?text=Sin+imagen";

export function cloudinaryUrl(src, { width = 600, ratio = "4:3" } = {}) {
  if (!src) return PLACEHOLDER;

  const transformaciones = `c_fill,ar_${ratio},w_${width},g_auto,f_auto,q_auto`;

  // Caso 1: URL completa de Cloudinary
  if (src.includes("res.cloudinary.com") && src.includes("/upload/")) {
    const [antes, despues] = src.split("/upload/");
    // Si ya trae transformaciones, la dejamos como está
    if (/^(c|w|h|f|q|ar|g)_/.test(despues)) return src;
    return `${antes}/upload/${transformaciones}/${despues}`;
  }

  // Caso 2: URL de otro sitio (o ruta vieja): se devuelve tal cual
  if (/^https?:\/\//.test(src) || src.startsWith("/")) return src;

  // Caso 3: solo el public_id guardado en la base
  if (!CLOUD_NAME) return PLACEHOLDER;
  return `https://res.cloudinary.com/${CLOUD_NAME}/image/upload/${transformaciones}/${src}`;
}
