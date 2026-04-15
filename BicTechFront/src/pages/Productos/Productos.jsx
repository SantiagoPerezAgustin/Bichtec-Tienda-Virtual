import React from "react";
import { useLocation } from "react-router-dom";
import ProductosCatalogo from "./ProductosCatalogo";

const Productos = () => {
  const location = useLocation();
  const paginaSeccion = location.pathname.endsWith("/tecnologia")
    ? "tecnologia"
    : "catalogo";

  return <ProductosCatalogo pagina={paginaSeccion} />;
};

export default Productos;
