import React, { useContext, useEffect, useMemo, useState } from "react";
import CardProducto from "../../components/CardProducto/CardProducto";
import DetalleProducto from "../../components/DetalleProducto/DetalleProducto";
import CardModificarProducto from "../../components/CardModificarProducto/CardModificarProducto";
import ProductosHero from "./ProductosHero";
import "./Productos.css";
import { useFiltro } from "../../context/FiltroContext";
import { AuthContext } from "../../context/AuthContext";
import {
  FILTRO_SIN_SELECCION,
  FILTRO_NOMBRE_FUNDA,
  FILTRO_NOMBRE_AURICULAR,
} from "../../constants/filtrosCatalogo";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5087";
const IMG_CATEGORIA_FALLBACK =
  "https://images.unsplash.com/photo-1468495241042-464ab9f5df4e?auto=format&fit=crop&w=900&q=80";

/** Por nombre: cubre fundas y auriculares aunque no tengan categoría en BD. */
const ES_FUNDA = /(funda|case|cover|protector)/i;

/** Incluye variantes sin tilde (normalizamos antes de probar). */
const ES_AURICULAR =
  /(auricular|auriculares|headphone|headphones|headset|headsets|earphone|earphones|earbud|earbuds|airpods?|audifono|audifonos|tws|manos libres|handsfree|hands[\s-]?free|\bbuds\b|\bpods\b)/i;

function normalizarNombreProducto(nombre) {
  return (nombre || "")
    .toLowerCase()
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "");
}

function esAuricularPorNombre(nombre) {
  return ES_AURICULAR.test(normalizarNombreProducto(nombre));
}

/** Categoría de BD tipo "Celulares": no debe mezclarse con fundas por nombre. */
function esCategoriaNombreCelulares(nombreCategoria) {
  if (!nombreCategoria) return false;
  const n = normalizarNombreProducto(nombreCategoria);
  if (/\bfunda/.test(n)) return false;
  return (
    n.includes("celular") ||
    n.includes("smartphone") ||
    (n.includes("tecnologia") && n.includes("movil"))
  );
}

const IMG_FUNDA =
  "https://images.unsplash.com/photo-1601784551446-20c9e07cdbdb?auto=format&fit=crop&w=900&q=80";
const IMG_AURICULAR =
  "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=900&q=80";

function coincideFiltroRapido(producto, filtroRapido, hayFiltroSidebar, categoriaNombrePorId) {
  if (filtroRapido === FILTRO_SIN_SELECCION) {
    // Sin tarjeta: el panel izquierdo ya redujo la lista (categoría, marca, etc.)
    if (hayFiltroSidebar) return true;
    return false;
  }
  if (filtroRapido === "todos") return true;
  const nombre = producto.nombre || "";
  if (filtroRapido === FILTRO_NOMBRE_FUNDA) return ES_FUNDA.test(nombre);
  if (filtroRapido === FILTRO_NOMBRE_AURICULAR) return esAuricularPorNombre(nombre);
  if (filtroRapido.startsWith("cat:")) {
    const catIdFiltro = filtroRapido.replace("cat:", "");
    if (String(producto.categoriaId ?? "") !== catIdFiltro) return false;
    const nombreCat = (categoriaNombrePorId && categoriaNombrePorId[catIdFiltro]) || "";
    if (esCategoriaNombreCelulares(nombreCat) && ES_FUNDA.test(nombre)) return false;
    return true;
  }
  return false;
}

function dedupeProductos(productos) {
  const vistos = new Set();
  return productos.filter((producto) => {
    const clave = [
      (producto.nombre || "").trim().toLowerCase(),
      Number(producto.precio || 0),
      Number(producto.marcaId || 0),
      Number(producto.categoriaId || 0),
      (producto.materialFunda || "").trim().toLowerCase(),
      (producto.color || "").trim().toLowerCase(),
    ].join("|");
    if (vistos.has(clave)) return false;
    vistos.add(clave);
    return true;
  });
}

/**
 * @param {{ pagina: "catalogo" | "tecnologia" | "fundas" }} props
 */
const ProductosCatalogo = ({ pagina }) => {
  const [productos, setProductos] = useState([]);
  const [categorias, setCategorias] = useState([]);
  const [productoSeleccionado, setProductoSeleccionado] = useState(null);
  const [productoAModificar, setProductoAModificar] = useState(null);
  const { rol, usuario } = useContext(AuthContext);

  const {
    categoriaSeleccionada,
    marcaSeleccionada,
    busqueda,
    precioMin,
    precioMax,
    filtroRapido,
    setFiltroRapido,
  } = useFiltro();

  useEffect(() => {
    if (pagina === "catalogo") setFiltroRapido(FILTRO_SIN_SELECCION);
    else if (pagina === "fundas") setFiltroRapido(FILTRO_NOMBRE_FUNDA);
    else setFiltroRapido(FILTRO_SIN_SELECCION);
  }, [pagina, setFiltroRapido]);

  useEffect(() => {
    if (categoriaSeleccionada == null) return;
    if (pagina === "fundas") return;
    setFiltroRapido(`cat:${categoriaSeleccionada}`);
  }, [categoriaSeleccionada, setFiltroRapido, pagina]);

  useEffect(() => {
    fetchProductos();
    // eslint-disable-next-line
  }, []);

  const fetchProductos = async () => {
    try {
      const response = await fetch(`${API_URL}/productos`);
      if (!response.ok) {
        console.error("Error al obtener productos:", response.status, response.statusText);
        setProductos([]);
        return;
      }
      const data = await response.json();
      const list = data.productos ?? data.Productos;
      setProductos(Array.isArray(list) ? list : []);
    } catch (error) {
      console.error("Error al obtener productos:", error);
      setProductos([]);
    }
  };

  const fetchCategorias = async () => {
    try {
      const response = await fetch(`${API_URL}/categorias`);
      if (!response.ok) {
        setCategorias([]);
        return;
      }
      const data = await response.json();
      const list = Array.isArray(data) ? data : data.categorias || [];
      setCategorias(Array.isArray(list) ? list : []);
    } catch (error) {
      console.error("Error al obtener categorías:", error);
      setCategorias([]);
    }
  };

  useEffect(() => {
    fetchCategorias();
    // eslint-disable-next-line
  }, []);

  const categoriaNombrePorId = useMemo(() => {
    const map = {};
    for (const c of categorias) {
      map[String(c.id ?? c._id)] = c.nombre || c.name || `Categoría ${c.id ?? c._id}`;
    }
    return map;
  }, [categorias]);

  const productosPreFiltrados = useMemo(() => {
    return productos.filter((producto) => {
      const nombre = producto.nombre || "";
      if (pagina === "tecnologia" && ES_FUNDA.test(nombre)) return false;

      const coincideCategoria = categoriaSeleccionada
        ? Number(producto.categoriaId) === Number(categoriaSeleccionada)
        : true;
      const nombreCatSidebar = categoriaSeleccionada
        ? categoriaNombrePorId[String(categoriaSeleccionada)] || ""
        : "";
      if (
        categoriaSeleccionada &&
        esCategoriaNombreCelulares(nombreCatSidebar) &&
        ES_FUNDA.test(nombre)
      ) {
        return false;
      }

      const coincideMarca = marcaSeleccionada
        ? Number(producto.marcaId) === Number(marcaSeleccionada)
        : true;
      const coincideBusqueda = busqueda
        ? producto.nombre.toLowerCase().includes(busqueda.toLowerCase())
        : true;
      const coincidePrecioMin =
        precioMin !== "" ? producto.precio >= Number(precioMin) : true;
      const coincidePrecioMax =
        precioMax !== "" ? producto.precio <= Number(precioMax) : true;
      return (
        coincideCategoria &&
        coincideMarca &&
        coincideBusqueda &&
        coincidePrecioMin &&
        coincidePrecioMax
      );
    });
  }, [
    productos,
    pagina,
    categoriaSeleccionada,
    marcaSeleccionada,
    busqueda,
    precioMin,
    precioMax,
    categoriaNombrePorId,
  ]);

  const productosBaseUnicos = useMemo(
    () => dedupeProductos(productosPreFiltrados),
    [productosPreFiltrados]
  );

  const tarjetasPorNombre = useMemo(() => {
    const defs = [
      {
        filtro: FILTRO_NOMBRE_FUNDA,
        nombre: "Fundas",
        descripcion: "Fundas y protectores (por nombre del producto).",
        match: (n) => ES_FUNDA.test(n),
        imagenFallback: IMG_FUNDA,
      },
      {
        filtro: FILTRO_NOMBRE_AURICULAR,
        nombre: "Auriculares",
        descripcion: "Auriculares, earbuds, TWS, etc. (por nombre del producto).",
        match: (n) => esAuricularPorNombre(n),
        imagenFallback: IMG_AURICULAR,
      },
    ];
    return defs
      .map((def) => {
        let count = 0;
        let imagen = null;
        for (const p of productosBaseUnicos) {
          const n = p.nombre || "";
          if (!def.match(n)) continue;
          count += 1;
          if (!imagen && p.imagenUrl) imagen = p.imagenUrl;
        }
        if (count === 0) return null;
        return {
          key: def.filtro,
          filtro: def.filtro,
          nombre: def.nombre,
          descripcion: def.descripcion,
          imagen: imagen || def.imagenFallback,
          count,
        };
      })
      .filter(Boolean);
  }, [productosBaseUnicos]);

  const categoriasVisuales = useMemo(() => {
    const base = {};
    for (const p of productosBaseUnicos) {
      const id = String(p.categoriaId ?? "");
      if (!id) continue;
      if (!base[id]) {
        base[id] = {
          id,
          nombre:
            p.categoriaNombre ||
            p.categoria?.nombre ||
            categoriaNombrePorId[id] ||
            `Categoría ${id}`,
          descripcion: "Ver productos de esta categoría.",
          imagen: p.imagenUrl || IMG_CATEGORIA_FALLBACK,
          count: 0,
        };
      }
      base[id].count += 1;
      if (!base[id].imagen && p.imagenUrl) base[id].imagen = p.imagenUrl;
    }
    const list = Object.values(base);
    return list.sort((a, b) => a.nombre.localeCompare(b.nombre, "es"));
  }, [productosBaseUnicos, categoriaNombrePorId]);

  const tarjetasCatalogo = useMemo(() => {
    const desdeBd = categoriasVisuales.map((c) => ({
      key: `cat:${c.id}`,
      filtro: `cat:${c.id}`,
      nombre: c.nombre,
      descripcion: c.descripcion,
      imagen: c.imagen,
      count: c.count,
    }));
    const hayCategoriaFundasEnBd = categoriasVisuales.some((c) =>
      /funda/i.test(c.nombre || "")
    );
    const virtualesSinDuplicarFundas = tarjetasPorNombre.filter((t) => {
      if (t.filtro !== FILTRO_NOMBRE_FUNDA) return true;
      return !hayCategoriaFundasEnBd;
    });
    return [...virtualesSinDuplicarFundas, ...desdeBd];
  }, [tarjetasPorNombre, categoriasVisuales]);

  /** Con categoría elegida en el panel: solo esa tarjeta (ej. Celulares sin mezclar Fundas). */
  const tarjetasCatalogoVisibles = useMemo(() => {
    if (categoriaSeleccionada == null) return tarjetasCatalogo;
    const fid = `cat:${categoriaSeleccionada}`;
    return tarjetasCatalogo.filter((t) => t.filtro === fid);
  }, [tarjetasCatalogo, categoriaSeleccionada]);

  const hayFiltroSidebar = Boolean(
    categoriaSeleccionada != null ||
      marcaSeleccionada != null ||
      (busqueda && String(busqueda).trim() !== "") ||
      precioMin !== "" ||
      precioMax !== ""
  );

  const productosFiltrados = productosPreFiltrados.filter((producto) =>
    coincideFiltroRapido(producto, filtroRapido, hayFiltroSidebar, categoriaNombrePorId)
  );

  const productosFiltradosUnicos = dedupeProductos(productosFiltrados);

  const tituloSeccionCatalogo = useMemo(() => {
    if (filtroRapido === FILTRO_NOMBRE_FUNDA) return "Fundas";
    if (filtroRapido === FILTRO_NOMBRE_AURICULAR) return "Auriculares";
    if (filtroRapido.startsWith("cat:")) {
      const id = filtroRapido.replace("cat:", "");
      const match = categoriasVisuales.find((c) => c.id === id);
      return match?.nombre || "Categoría";
    }
    return "Catálogo";
  }, [filtroRapido, categoriasVisuales]);

  const renderGrid = (lista) =>
    lista.map((producto, index) => (
      <div
        className="col-12 col-sm-6 col-md-4 d-flex justify-content-center mb-4"
        key={`producto-${producto.id ?? "sin-id"}-${index}`}
      >
        <CardProducto
          producto={producto}
          onVerDetalles={() => setProductoSeleccionado(producto)}
          onModificar={() => setProductoAModificar(producto)}
          recargarProductos={fetchProductos}
        />
      </div>
    ));

  const mostrarMensajeElegirTarjeta =
    filtroRapido === FILTRO_SIN_SELECCION && !hayFiltroSidebar;

  return (
    <>
      <div className="container-fluid catalogo-top-energized py-4 px-3 px-lg-4">
        <ProductosHero pagina={pagina} />

        {productosBaseUnicos.length > 0 && (
          <div className="catalogo-tipo-wrap catalogo-tipo-wrap--hero mt-4">
            <div className="catalogo-tipo-title-wrap text-center">
              <h3 className="catalogo-tipo-title mb-1">Elegí una categoría</h3>
              <p className="catalogo-tipo-sub mb-0">
                Tocá una tarjeta para filtrar, o usá categoría, marca y precio en el panel izquierdo.
              </p>
            </div>

            <div className="catalogo-tipo-grid">
              {tarjetasCatalogoVisibles.map((cat) => {
                const active = filtroRapido === cat.filtro;
                return (
                  <button
                    key={cat.key}
                    type="button"
                    className={`catalogo-tipo-card ${active ? "active" : ""}`}
                    onClick={() => setFiltroRapido(cat.filtro)}
                  >
                    <img
                      src={cat.imagen || IMG_CATEGORIA_FALLBACK}
                      alt={cat.nombre}
                      className="catalogo-tipo-img"
                      loading="lazy"
                    />
                    <div className="catalogo-tipo-body">
                      <h4>{cat.nombre}</h4>
                      <p>{cat.descripcion}</p>
                      <span className="catalogo-tipo-badge">{cat.count} disponibles</span>
                    </div>
                  </button>
                );
              })}
            </div>
          </div>
        )}
      </div>

      <div className="container py-2" id="catalogo-productos">
        <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
          <h2 className="m-0">
            {filtroRapido === FILTRO_SIN_SELECCION && !hayFiltroSidebar
              ? "Catálogo"
              : tituloSeccionCatalogo}
          </h2>
          <span className="badge bg-dark fs-6">
            {filtroRapido === FILTRO_SIN_SELECCION && !hayFiltroSidebar
              ? "Elegí una categoría"
              : `${productosFiltradosUnicos.length} resultado${
                  productosFiltradosUnicos.length === 1 ? "" : "s"
                }`}
          </span>
        </div>

        {productosBaseUnicos.length === 0 ? (
          <div className="alert alert-secondary text-center">
            No hay productos que coincidan con los filtros del panel (categoría, marca, precio o
            búsqueda).
          </div>
        ) : (
          <>
            {mostrarMensajeElegirTarjeta ? (
              <div className="alert alert-light border text-center catalogo-sin-seleccion-msg">
                <p className="mb-0 text-muted">
                  Elegí una tarjeta arriba o filtrá por el panel izquierdo para ver el listado.
                </p>
              </div>
            ) : productosFiltradosUnicos.length === 0 ? (
              <div className="row">
                <div className="col-12">
                  <div className="alert alert-secondary text-center">
                    No se encontraron productos con esos filtros.
                  </div>
                </div>
              </div>
            ) : (
              <div className="row">{renderGrid(productosFiltradosUnicos)}</div>
            )}
          </>
        )}
      </div>

      {productoSeleccionado && (
        <DetalleProducto
          producto={productoSeleccionado}
          onClose={() => setProductoSeleccionado(null)}
          categoriaSeleccionada={categoriaSeleccionada}
          marcaSeleccionada={marcaSeleccionada}
        />
      )}
      {productoAModificar && usuario && rol?.toLowerCase() === "admin" && (
        <CardModificarProducto
          producto={productoAModificar}
          onClose={() => setProductoAModificar(null)}
          categoriaSeleccionada={categoriaSeleccionada}
          marcaSeleccionada={marcaSeleccionada}
          recargarProductos={fetchProductos}
        />
      )}
    </>
  );
};

export default ProductosCatalogo;
