import { useContext } from "react";
import { Link } from "react-router-dom";
import CartSummary from "../../components/CartSummary/CartSummary";
import EmptyCart from "../../components/EmptyCart/EmptyCart";
import { CarritoContext } from "../../context/CarritoContext";
import { WHATSAPP_PHONE } from "../../config/whatsapp";
import { formatPrecioARS, formatMontoARS, valorPrecioDisplay } from "../../utils/precio";
import "./Cart.css";

const IMG_PLACEHOLDER =
  "https://images.unsplash.com/photo-1468495241042-464ab9f5df4e?auto=format&fit=crop&w=200&q=80";

const Cart = () => {
  const { carrito, actualizarCantidad, quitarDelCarrito, obtenerColorSeleccionado } =
    useContext(CarritoContext);

  const cartItems = carrito || [];

  const modificarCantidad = (id, nuevaCantidad) => {
    actualizarCantidad(id, nuevaCantidad);
  };

  const total = cartItems.reduce(
    (acc, item) =>
      acc + valorPrecioDisplay(item.producto.precio) * item.cantidad,
    0,
  );

  const finalizarPorWhatsapp = () => {
    if (cartItems.length === 0) {
      alert("El carrito está vacío");
      return;
    }

    let mensaje = `Hola! 👋\n`;
    mensaje += `Soy cliente de *BichTec* y quería realizar el siguiente pedido:\n\n`;

    cartItems.forEach((item) => {
      const colorElegido = obtenerColorSeleccionado(item.productoId);
      const sub = valorPrecioDisplay(item.producto.precio) * item.cantidad;
      mensaje += `• ${item.producto.nombre}\n`;
      if (colorElegido) {
        mensaje += `  Color: ${colorElegido}\n`;
      }
      mensaje += `  Cantidad: ${item.cantidad}\n`;
      mensaje += `  Subtotal: ${formatMontoARS(sub)}\n\n`;
    });

    mensaje += `🧾 *Total del pedido:* ${formatMontoARS(total)}\n\n`;
    mensaje += `Quedo atento/a para coordinar el pago y la entrega 😊\n\n`;

    const url = `https://wa.me/${WHATSAPP_PHONE}?text=${encodeURIComponent(
      mensaje,
    )}`;

    window.open(url, "_blank");
  };

  const eliminarItem = (id) => {
    quitarDelCarrito(id);
  };

  return (
    <div className="cart-page">
      <div className="cart-page__inner">
        <header className="cart-page__top">
          <Link to="/productos" className="cart-page__back">
            ← Volver a productos
          </Link>
          <div className="cart-page__title-wrap">
            <h1 className="cart-page__title">Carrito de compras</h1>
            <p className="cart-page__subtitle">
              Revisá los productos y confirmá el pedido por WhatsApp
            </p>
          </div>
        </header>

        {carrito.length === 0 ? (
          <EmptyCart />
        ) : (
          <div className="row g-4 g-lg-5">
            <div className="col-lg-8">
              <div className="cart-page__list">
                {cartItems.map((item) => {
                  const img = item.producto?.imagenUrl || IMG_PLACEHOLDER;
                  const lineTotal =
                    valorPrecioDisplay(item.producto.precio) * item.cantidad;
                  return (
                    <article key={item.productoId} className="cart-line">
                      <div className="cart-line__thumb-wrap">
                        <img
                          className="cart-line__thumb"
                          src={img}
                          alt=""
                          loading="lazy"
                        />
                      </div>
                      <div className="cart-line__body">
                        <h2 className="cart-line__name">{item.producto.nombre}</h2>
                        <p className="cart-line__meta">
                          Precio unitario:{" "}
                          <strong>{formatPrecioARS(item.producto.precio)}</strong>
                        </p>
                        {obtenerColorSeleccionado(item.productoId) && (
                          <p className="cart-line__meta">
                            Color:{" "}
                            <strong>{obtenerColorSeleccionado(item.productoId)}</strong>
                          </p>
                        )}
                        <p className="cart-line__subtotal-label">
                          Subtotal: {formatMontoARS(lineTotal)}
                        </p>
                        <div className="cart-line__controls">
                          <div className="cart-qty" aria-label="Cantidad">
                            <button
                              type="button"
                              className="cart-qty__btn"
                              onClick={() =>
                                modificarCantidad(item.productoId, item.cantidad - 1)
                              }
                              disabled={item.cantidad <= 1}
                              aria-label="Disminuir cantidad"
                            >
                              −
                            </button>
                            <span className="cart-qty__value">{item.cantidad}</span>
                            <button
                              type="button"
                              className="cart-qty__btn"
                              onClick={() =>
                                modificarCantidad(item.productoId, item.cantidad + 1)
                              }
                              aria-label="Aumentar cantidad"
                            >
                              +
                            </button>
                          </div>
                          <button
                            type="button"
                            className="cart-line__remove"
                            onClick={() => eliminarItem(item.productoId)}
                          >
                            Quitar
                          </button>
                        </div>
                      </div>
                      <div className="cart-line__price-col">
                        <p className="cart-line__price">
                          {formatMontoARS(lineTotal)}
                        </p>
                        <p className="cart-line__price-note">Total línea</p>
                      </div>
                    </article>
                  );
                })}
              </div>
            </div>
            <div className="col-lg-4">
              <CartSummary total={total} finalizarPorWhatsapp={finalizarPorWhatsapp} />
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Cart;
