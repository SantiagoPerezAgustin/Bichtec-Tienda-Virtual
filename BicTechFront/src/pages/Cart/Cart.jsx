import { useContext } from "react";
import CartItem from "../../components/CartItem/CartItem";
import CartSummary from "../../components/CartSummary/CartSummary";
import EmptyCart from "../../components/EmptyCart/EmptyCart";
import { CarritoContext } from "../../context/CarritoContext";
import { WHATSAPP_PHONE } from "../../config/whatsapp";

const Cart = () => {
  const API_URL = import.meta.env.VITE_API_URL;

  const { carrito, actualizarCantidad, quitarDelCarrito } =
    useContext(CarritoContext);

    const cartItems = carrito || [];

  const modificarCantidad = (id, nuevaCantidad) => {
    actualizarCantidad(id, nuevaCantidad);
  };

  const finalizarPorWhatsapp = () => {
    if (cartItems.length === 0) {
      alert("El carrito está vacío");
      return;
    }

    let mensaje = `Hola!\n`;
    mensaje += `Quería hacer el siguiente pedido:\n\n`;

    cartItems.forEach((item) => {
      mensaje += `• ${item.producto.nombre} x${item.cantidad} - $${(
        item.producto.precio * item.cantidad
      ).toLocaleString("es-AR")}\n`;
    });

    mensaje += `\nTotal: $${total.toLocaleString("es-AR")}`;

    const url = `https://wa.me/${WHATSAPP_PHONE}?text=${encodeURIComponent(
      mensaje,
    )}`;

    window.open(url, "_blank");
  };


  const total = cartItems.reduce(
    (acc, item) => acc + item.producto.precio * item.cantidad,
    0,
  );


  const eliminarItem = (id) => {
    quitarDelCarrito(id);
  };
  console.log(carrito);
  return (
    <div className="bg-dark text-light min-vh-100 py-5">
      <div
        className="container-fluid px-5"
        style={{ maxWidth: "1400px", margin: "0 auto" }}
      >
        <div className="d-flex justify-content-between align-items-center mb-4">
          <a
            href="/productos"
            className="btn btn-outline-light"
            style={{ borderColor: "#d4af37", color: "#d4af37" }}
          >
            ← Volver a productos
          </a>
          <h1
            className="text-center flex-grow-1 m-0"
            style={{ color: "#d4af37" }}
          >
            🛒 Carrito de compras
          </h1>
          <div style={{ width: "142.5px" }}></div>
        </div>

        {carrito.length === 0 ? (
          <EmptyCart />
        ) : (
          <div className="row">
            <div className="col-md-8">
              {cartItems.map((item) => (
                <div
                  key={item.productoId}
                  className="card mb-3 text-light"
                  style={{
                    backgroundColor: "#000",
                    border: "1px solid #d4af37",
                  }}
                >
                  <div className="card-body d-flex justify-content-between align-items-center">
                    <div>
                      <h5
                        className="card-title mb-1"
                        style={{ color: "#d4af37" }}
                      >
                        {item.producto.nombre}
                      </h5>
                      <p className="mb-1">
                        Unitario: $
                        {item.producto.precio.toLocaleString("es-AR")}
                      </p>

                      <p className="mb-1 fw-bold text-light">
                        Subtotal: $
                        {(item.producto.precio * item.cantidad).toLocaleString(
                          "es-AR",
                        )}
                      </p>
                      <div className="d-flex align-items-center gap-2 mt-2">
                        <button
                          className="btn btn-outline-light btn-sm"
                          onClick={() =>
                            modificarCantidad(
                              item.productoId,
                              item.cantidad - 1,
                            )
                          }
                          disabled={item.cantidad <= 1}
                        >
                          −
                        </button>

                        <span>{item.cantidad}</span>

                        <button
                          className="btn btn-outline-light btn-sm"
                          onClick={() =>
                            modificarCantidad(
                              item.productoId,
                              item.cantidad + 1,
                            )
                          }
                        >
                          +
                        </button>

                        <button
                          className="btn btn-outline-danger btn-sm ms-3"
                          onClick={() => eliminarItem(item.productoId)}
                        >
                          🗑
                        </button>
                      </div>
                    </div>

                    <div className="text-end">
                      <p className="mb-0 fw-bold">
                        $
                        {(item.producto.precio * item.cantidad).toLocaleString(
                          "es-AR",
                        )}
                      </p>
                    </div>
                  </div>
                </div>
              ))}

              <div
                className="p-3 mt-3 rounded"
                style={{
                  border: "1px solid #d4af37",
                  backgroundColor: "#000",
                }}
              >
                <h4 className="mb-0 text-end" style={{ color: "#d4af37" }}>
                  Total: ${total.toLocaleString("es-AR")}
                </h4>
              </div>
            </div>
            <div className="col-md-4">
              <CartSummary
                total={total}
                finalizarPorWhatsapp={finalizarPorWhatsapp}
              />
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Cart;
