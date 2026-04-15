import React, { useMemo } from "react";
import Carousel from "react-bootstrap/Carousel";
import { Link } from "react-router-dom";

const ES_FUNDA = /(funda|case|cover|protector)/i;
const ES_CELULAR =
  /(iphone|samsung|redmi|poco|motorola|galaxy|xiaomi|pixel|phone|celular)/i;

const IMG_FUNDA =
  "https://images.unsplash.com/photo-1601784551446-20c9e07cdbdb?w=800&q=80";
const IMG_PHONE =
  "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=800&q=80";

function quitarSlidesDuplicados(slides = []) {
  const vistos = new Set();
  return slides.filter((s) => {
    // Priorizamos imagen para evitar carruseles "iguales visualmente".
    const clave = (s.imagen || "").trim().toLowerCase();
    if (!clave) return true;
    if (vistos.has(clave)) return false;
    vistos.add(clave);
    return true;
  });
}

/**
 * Hero según la página (/productos, /productos/tecnologia, /fundas).
 * El menú principal está en el header (dropdown Productos).
 */
function ProductosHero({ productos, pagina }) {
  const fundasParaCarousel = useMemo(() => {
    return (productos || [])
      .filter((p) => ES_FUNDA.test(p.nombre || ""))
      .slice(0, 6);
  }, [productos]);

  const celularesParaCarousel = useMemo(() => {
    return (productos || [])
      .filter((p) => ES_CELULAR.test(p.nombre || ""))
      .slice(0, 6);
  }, [productos]);

  const slidesFundas = useMemo(() => {
    const base = quitarSlidesDuplicados(
      fundasParaCarousel.map((p, i) => ({
        key: `f-${p.id}-${i}`,
        imagen: p.imagenUrl || IMG_FUNDA,
        titulo: p.nombre?.slice(0, 52) || "Funda",
        texto:
          "Protección y estilo. Consultá stock y colores en el catálogo de abajo.",
      }))
    );
    if (base.length >= 2) return base.slice(0, 5);
    return quitarSlidesDuplicados([
      ...base,
      {
        key: "rf1",
        imagen: IMG_FUNDA,
        titulo: "Fundas y protectores",
        texto:
          "Silicona, rígidas, con soporte: elegí la que mejor se adapte a tu celular.",
      },
      {
        key: "rf2",
        imagen:
          "https://images.unsplash.com/photo-1580910051074-3eb694886efe?w=800&q=80",
        titulo: "Combiná con tu equipo",
        texto: "Compatibilidad por modelo. Mirá todas las opciones en el catálogo.",
      },
      {
        key: "rf3",
        imagen:
          "https://images.unsplash.com/photo-1603313011108-b8780c8d4448?w=800&q=80",
        titulo: "Fundas con estilo",
        texto: "Múltiples colores y acabados para cada modelo.",
      },
    ]).slice(0, 5);
  }, [fundasParaCarousel]);

  const slidesTecnologia = useMemo(() => {
    const base = quitarSlidesDuplicados(
      celularesParaCarousel.map((p, i) => ({
        key: `c-${p.id}-${i}`,
        imagen: p.imagenUrl || IMG_PHONE,
        titulo: p.nombre?.slice(0, 52) || "Smartphone",
        texto:
          "Rendimiento, cámara y batería. Precios y stock actualizados abajo.",
      }))
    );
    if (base.length >= 2) return base.slice(0, 5);
    return quitarSlidesDuplicados([
      ...base,
      {
        key: "rc1",
        imagen: IMG_PHONE,
        titulo: "Tecnología móvil",
        texto:
          "Smartphones de las mejores marcas. Compará modelos en nuestro catálogo.",
      },
      {
        key: "rc2",
        imagen:
          "https://images.unsplash.com/photo-1592899677977-9c10ca588bbd?w=800&q=80",
        titulo: "5G y pantallas fluidas",
        texto: "Encontrá el equipo ideal para trabajo, estudio o entretenimiento.",
      },
    ]).slice(0, 5);
  }, [celularesParaCarousel]);

  const slidesCatalogo = useMemo(() => {
    const mix = [
      ...slidesTecnologia.slice(0, 3),
      ...slidesFundas.slice(0, 3),
    ].slice(0, 5);
    return mix.length > 0
      ? mix
      : [
          {
            key: "mix1",
            imagen: IMG_PHONE,
            titulo: "Bienvenido a BICHTEC",
            texto: "Explorá tecnología y fundas en el catálogo de abajo.",
          },
        ];
  }, [slidesTecnologia, slidesFundas]);

  const slides =
    pagina === "fundas"
      ? slidesFundas
      : pagina === "tecnologia"
        ? slidesTecnologia
        : slidesCatalogo;

  const titulo =
    pagina === "tecnologia"
      ? "Tecnología"
      : pagina === "fundas"
        ? "Fundas"
        : "Productos";

  const subtitulo =
    pagina === "tecnologia"
      ? "Celulares y smartphones"
      : pagina === "fundas"
        ? "Protectores, fundas y más para cuidar tu equipo"
        : "Todo el catálogo BICHTEC";

  return (
    <div className="productos-hero mb-4">
      <div className="productos-hero-head mb-3">
        <div className="d-flex flex-wrap align-items-end justify-content-between gap-2">
          <div>
            <h1 className="productos-hero-title m-0">{titulo}</h1>
            <p className="productos-hero-subtitle mb-0 text-muted">{subtitulo}</p>
          </div>
          <div className="productos-hero-breadcrumb small">
            <Link to="/productos" className="text-decoration-none">
              Catálogo
            </Link>
            {pagina === "tecnologia" && (
              <>
                {" "}
                / <span className="fw-semibold">Tecnología</span>
              </>
            )}
            {pagina === "fundas" && (
              <>
                {" "}
                / <span className="fw-semibold">Fundas</span>
              </>
            )}
          </div>
        </div>
      </div>

      <div className="productos-carousel-wrap">
        <Carousel
          variant="dark"
          className="rounded-3 overflow-hidden shadow"
          indicators
          controls
        >
          {slides.map((s) => (
            <Carousel.Item key={s.key}>
              <div className="productos-carousel-item">
                <img
                  className="d-block w-100 productos-carousel-img"
                  src={s.imagen}
                  alt={s.titulo}
                  loading="lazy"
                />
                <Carousel.Caption className="productos-carousel-caption text-start">
                  <h2 className="h4 mb-2">{s.titulo}</h2>
                  <p className="mb-0 d-none d-sm-block small">{s.texto}</p>
                </Carousel.Caption>
              </div>
            </Carousel.Item>
          ))}
        </Carousel>
      </div>
    </div>
  );
}

export default ProductosHero;
