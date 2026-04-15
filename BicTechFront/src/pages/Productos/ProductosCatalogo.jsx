import React, { useContext, useEffect, useState } from "react";
import CardProducto from "../../components/CardProducto/CardProducto";
import DetalleProducto from "../../components/DetalleProducto/DetalleProducto";
import CardModificarProducto from "../../components/CardModificarProducto/CardModificarProducto";
import ProductosHero from "./ProductosHero";
import "./Productos.css";
import { useFiltro } from "../../context/FiltroContext";
import { AuthContext } from "../../context/AuthContext";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5087";

/**
 * @param {{ pagina: "catalogo" | "tecnologia" | "fundas" }} props
 */
const ProductosCatalogo = ({ pagina }) => {
  const [productos, setProductos] = useState([]);
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
    if (pagina === "tecnologia") setFiltroRapido("celulares");
    else if (pagina === "fundas") setFiltroRapido("fundas");
    else setFiltroRapido("todos");
  }, [pagina, setFiltroRapido]);

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

  const productosFiltrados = productos.filter((producto) => {
    const nombre = (producto.nombre || "").toLowerCase();
    const coincideCategoria = categoriaSeleccionada
      ? Number(producto.categoriaId) === Number(categoriaSeleccionada)
      : true;
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
    const coincideFiltroRapido =
      filtroRapido === "todos"
        ? true
        : filtroRapido === "celulares"
        ? /(iphone|samsung|redmi|poco|motorola|galaxy|xiaomi|pixel|phone|celular)/i.test(
            nombre
          )
        : filtroRapido === "fundas"
        ? /(funda|case|cover|protector)/i.test(nombre)
        : /(cargador|smartwatch|parlante|auricular|accesorio|cable|powerbank)/i.test(
            nombre
          );

    return (
      coincideCategoria &&
      coincideMarca &&
      coincideBusqueda &&
      coincidePrecioMin &&
      coincidePrecioMax &&
      coincideFiltroRapido
    );
  });

  const productosFiltradosUnicos = (() => {
    const vistos = new Set();
    return productosFiltrados.filter((producto) => {
      // Si la base viene con filas duplicadas por inserciones repetidas,
      // evitamos mostrar cards duplicadas al usuario.
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
  })();

  return (
    <>
      <div className="container-fluid py-3 px-3 px-lg-4">
        <ProductosHero productos={productos} pagina={pagina} />
      </div>

      <div className="container py-2" id="catalogo-productos">
        <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
          <h2 className="m-0">Catálogo</h2>
          <span className="badge bg-dark fs-6">
            {productosFiltradosUnicos.length} resultado
            {productosFiltradosUnicos.length === 1 ? "" : "s"}
          </span>
        </div>
        <div className="row">
          {productosFiltradosUnicos.length === 0 ? (
            <div className="col-12">
              <div className="alert alert-secondary text-center">
                No se encontraron productos con esos filtros.
              </div>
            </div>
          ) : (
            productosFiltradosUnicos.map((producto, index) => (
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
            ))
          )}
        </div>
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
