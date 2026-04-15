import React from "react";
import { Link } from "react-router-dom";

/**
 * Encabezado según la página (/productos, /productos/tecnologia, /fundas).
 * Sin carrusel: solo título y migas de pan; el catálogo es solo la grilla de productos.
 */
function ProductosHero({ pagina }) {
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
    </div>
  );
}

export default ProductosHero;
