import { useContext, useEffect, useMemo, useState } from "react";
import Button from "react-bootstrap/Button";
import Card from "react-bootstrap/Card";
import { AuthContext } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { CarritoContext } from "../../context/CarritoContext";
import "./CardProducto.css";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5000";

function CardProducto({
  producto,
  onVerDetalles,
  onModificar,
  recargarProductos,
}) {
  const PALETA_COLORES = {
    negro: "#1f2937",
    blanco: "#f9fafb",
    rojo: "#dc2626",
    azul: "#2563eb",
    celeste: "#0ea5e9",
    verde: "#16a34a",
    rosa: "#ec4899",
    violeta: "#7c3aed",
    morado: "#7c3aed",
    amarillo: "#facc15",
    naranja: "#f97316",
    gris: "#6b7280",
    plateado: "#94a3b8",
    dorado: "#eab308",
    transparente: "linear-gradient(135deg, #ffffff 0%, #d4d4d8 100%)",
  };

  function normalizarPrecio(precio) {
    const num = Number(precio);
    if (num < 1000) {
      return num * 1000;
    }
    return num;
  }

  function inferirTipoProducto(nombre = "") {
    const texto = nombre.toLowerCase();
    // Prioridad: si en el nombre dice funda/case/cover/protector, debe ser Funda
    // aunque también mencione marcas/modelos de celulares.
    if (/(funda|case|cover|protector)/i.test(texto)) {
      return "Funda";
    }
    if (
      /(iphone|samsung|redmi|poco|motorola|galaxy|xiaomi|pixel|phone|celular)/i.test(
        texto
      )
    ) {
      return "Celular";
    }
    return "Accesorio";
  }

  const esFunda = inferirTipoProducto(producto?.nombre) === "Funda";
  const esSilicona =
    (producto?.materialFunda || "").trim().toLowerCase() === "silicona";

  const coloresFunda = useMemo(() => {
    const list = (producto?.color || "")
      .split(/[,/|]/)
      .map((color) => color.trim())
      .filter(Boolean);
    return [...new Set(list.map((c) => c.toLowerCase()))].map(
      (lower) => list.find((c) => c.toLowerCase() === lower) || lower
    );
  }, [producto?.color]);

  const [colorSeleccionado, setColorSeleccionado] = useState("");

  useEffect(() => {
    setColorSeleccionado(coloresFunda[0] || "");
  }, [producto?.id, coloresFunda]);

  const navigate = useNavigate();

  const { usuario, rol } = useContext(AuthContext);
  const { agregarAlCarrito } = useContext(CarritoContext);

  const handleBtnComprarClick = async (e) => {
    if (!usuario) {
      e.preventDefault();
      toast.error("Debes iniciar sesión para comprar.");
      navigate("/login");
    } else {
      try {
        if (esSilicona && coloresFunda.length > 0 && !colorSeleccionado) {
          toast.warning("Seleccioná un color antes de agregar al carrito.");
          return;
        }

        await agregarAlCarrito(producto.id, 1, colorSeleccionado || null);
        toast.success(
          colorSeleccionado
            ? `Producto agregado al carrito (Color: ${colorSeleccionado})`
            : "Producto agregado al carrito!"
        );
      } catch (error) {
        // Mostrar mensaje específico si es error de stock
        if (
          error?.message?.toLowerCase().includes("stock") ||
          error?.message?.toLowerCase().includes("suficiente")
        ) {
          toast.error("No hay suficiente stock disponible.");
        } else {
          toast.error("No se pudo agregar al carrito.");
        }
      }
    }
  };

  const handleEliminar = () => {
    toast.info(
      <div>
        ¿Estás seguro que deseas eliminar este producto?
        <div style={{ marginTop: 10, display: "flex", gap: 10 }}>
          <button
            style={{
              background: "#28a745",
              color: "#fff",
              border: "none",
              padding: "5px 12px",
              borderRadius: 5,
              cursor: "pointer",
            }}
            onClick={async () => {
              toast.dismiss();
              try {
                const response = await fetch(
                  `${API_URL}/productos/${producto.id}`,
                  {
                    method: "DELETE",
                    headers: {
                      "Content-Type": "application/json",
                      Authorization: `Bearer ${localStorage.getItem("token")}`,
                    },
                  }
                );
                if (response.ok) {
                  toast.success("Producto eliminado correctamente");
                  if (recargarProductos) recargarProductos();
                } else {
                  toast.error("Error al eliminar el producto");
                }
              } catch (error) {
                toast.error("Error de conexión al eliminar el producto");
              }
            }}
          >
            Sí
          </button>
          <button
            style={{
              background: "#dc3545",
              color: "#fff",
              border: "none",
              padding: "5px 12px",
              borderRadius: 5,
              cursor: "pointer",
            }}
            onClick={() => toast.dismiss()}
          >
            No
          </button>
        </div>
      </div>,
      { autoClose: false }
    );
  };

  return (
    <Card
      className="card-producto"
      style={{
        minWidth: "9rem",
        maxWidth: "19rem",
        minHeight: "22rem",
        maxHeight: "30rem",
        border: "3px solid #FFD700",
        backgroundColor: "#fdf6e3",
      }}
    >
      <Card.Img
        variant="top"
        src={producto.imagenUrl}
        className="card-producto-img"
        style={{
          minHeight: "11rem",
          maxHeight: "18rem",
          objectFit: "contain",
          backgroundColor: "#fff",
          marginBottom: "0.5rem",
        }}
      />
      <Card.Body
        className="card-producto-body"
        style={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "flex-start",
          height: "calc(420px - 180px)",
          paddingTop: 0,
          paddingBottom: 0,
        }}
      >
        <div>
          <div className="text-center mb-2">
            <span className="badge text-bg-light border">
              {inferirTipoProducto(producto.nombre)}
            </span>
          </div>
          <Card.Title
            className="text-center"
            style={{
              fontWeight: "bold",
              fontSize: "1.3rem",
              marginBottom: "0.25rem",
              color: "#000",
              textShadow: "1px 1px 0 #FFD700",
            }}
          >
            {producto.nombre}
          </Card.Title>
          <div className="text-center" style={{ marginBottom: "0.5rem" }}>
            <span
              style={{
                color: "#000",
                fontWeight: "bold",
                fontSize: "1.2rem",
                background: "linear-gradient(45deg, #FFD700, #FFEA00)",
                borderRadius: "8px",
                padding: "0.25rem 0.75rem",
                boxShadow: "0 0 6px rgba(0,0,0,0.2)",
              }}
            >
              $
              {normalizarPrecio(producto.precio).toLocaleString("es-AR", {
                minimumFractionDigits: 0,
                maximumFractionDigits: 0,
              })}
            </span>
          </div>
          {esFunda && esSilicona && (
            <div className="card-producto-color-select-wrap">
              <label className="card-producto-color-label">Color</label>
              {coloresFunda.length > 0 ? (
                <div className="card-producto-color-input-group">
                  <span
                    className="card-producto-color-dot"
                    style={{
                      background:
                        PALETA_COLORES[(colorSeleccionado || "").toLowerCase()] || "#9ca3af",
                    }}
                  />
                  <select
                    className="card-producto-color-select"
                    value={colorSeleccionado}
                    onChange={(e) => setColorSeleccionado(e.target.value)}
                  >
                    {coloresFunda.map((color) => (
                      <option key={`${producto.id}-${color}`} value={color}>
                        {color}
                      </option>
                    ))}
                  </select>
                </div>
              ) : (
                <div className="card-producto-colores-vacio">Sin colores cargados</div>
              )}
            </div>
          )}
        </div>
        <div className="d-flex justify-content-center gap-2 mt-3 mb-2">
          {producto.stock <= 0 && (
            <div
              style={{
                background: "linear-gradient(90deg, #ff1744 0%, #ff616f 100%)",
                color: "#fff",
                padding: "0.25rem 0.7rem",
                borderRadius: "6px",
                fontWeight: "bold",
                fontSize: "0.95rem",
                marginBottom: "0.5rem",
                alignSelf: "center",
                boxShadow: "0 1px 4px rgba(255,23,68,0.13)",
                border: "1.5px solid #fff",
                letterSpacing: "0.5px",
                display: "inline-flex",
                alignItems: "center",
                gap: "0.4rem",
                textShadow: "1px 1px 2px #b71c1c",
              }}
            >
              <span style={{ fontSize: "1.1rem" }}>⚠️</span>
              Sin stock
            </div>
          )}
          {rol?.toLowerCase() === "admin" && (
            <Button
              type="button"
              style={{
                backgroundColor: "#40A9FF",
                color: "#000",
                border: "1px solid #000",
                fontWeight: "bold",
              }}
              onClick={onModificar}
            >
              Modificar
            </Button>
          )}
          {rol?.toLowerCase() === "admin" && (
            <Button
              type="button"
              style={{
                backgroundColor: "#FF4D4F ",
                color: "#000",
                border: "1px solid #000",
                fontWeight: "bold",
              }}
              onClick={handleEliminar}
            >
              Eliminar
            </Button>
          )}
          {usuario && rol?.toLowerCase() !== "admin" && producto.stock > 0 && (
            <Button
              type="button"
              onClick={handleBtnComprarClick}
              style={{
                backgroundColor: "#FFD700",
                color: "#000",
                border: "1px solid #000",
                fontWeight: "bold",
              }}
            >
              Comprar +
            </Button>
          )}
          {rol?.toLowerCase() !== "admin" && (
            <Button type="button" variant="dark" onClick={onVerDetalles}>
              Ver detalle
            </Button>
          )}
        </div>
      </Card.Body>
    </Card>
  );
}

export default CardProducto;
