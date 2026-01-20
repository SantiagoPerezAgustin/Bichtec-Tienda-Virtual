import { useContext, useState } from "react";
import { CarritoContext } from "../../context/CarritoContext";
import { AuthContext } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5000";

const CartSummary = ({ total, finalizarPorWhatsapp }) => {
  const { carrito, vaciarCarrito } = useContext(CarritoContext);
  const { usuario } = useContext(AuthContext);
  const navigate = useNavigate();
  const [direccionEnvio, setDireccionEnvio] = useState("");

  const handleFinalizarCompra = async () => {
    if (!usuario) {
      toast.error("Debes iniciar sesión para finalizar la compra.");
      navigate("/login");
      return;
    }
    if (!items.length) {
      toast.error("El carrito está vacío.");
      return;
    }
    if (!direccionEnvio.trim()) {
      toast.error("Debes ingresar una dirección de envío.");
      return;
    }
    console.log("Items:", items);
    try {
      const detalles = items.map((item) => ({
        productoId: item.productoId,
        cantidad: item.cantidad,
        precio: item.producto?.precio ?? 0,
      }));

      const res = await fetch(`${API_URL}/pedidos`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify({
          usuarioId: usuario.id,
          direccionEnvio,
          detalles,
        }),
      });

      if (!res.ok) {
        const errorData = await res.json();
        throw new Error(errorData.message || "Error al finalizar compra");
      }

      vaciarCarrito();
      toast.success("¡Compra realizada con éxito!");
      navigate("/carrito");
    } catch (error) {
      toast.error("Error al finalizar compra: " + error.message);
    }
  };


  return (
    <div
      className="card text-light shadow-sm"
      style={{ backgroundColor: "#000", border: "1px solid #d4af37" }}
    >
      <div className="card-body">
        <h4 className="card-title" style={{ color: "#d4af37" }}>
          Resumen de compra
        </h4>
        <hr style={{ borderColor: "#d4af37" }} />
        <p className="fs-5">
          Total:{" "}
          <strong style={{ color: "#d4af37" }}>
            ${total.toLocaleString("es-AR", { minimumFractionDigits: 0 })}
          </strong>
        </p>
        <button
          onClick={finalizarPorWhatsapp}
          className="btn w-100 mt-3 d-flex align-items-center justify-content-center gap-2"
          style={{
            backgroundColor: "#25D366",
            color: "#000",
            fontWeight: "bold",
          }}
        >
          <i className="bi bi-whatsapp"></i>
          Finalizar pedido por WhatsApp
        </button>
      </div>
    </div>
  );
};

export default CartSummary;
