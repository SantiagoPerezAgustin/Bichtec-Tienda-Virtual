import "../../pages/Cart/Cart.css";
import { formatMontoARS } from "../../utils/precio";

const CartSummary = ({ total, finalizarPorWhatsapp }) => {
  return (
    <div className="card cart-summary text-light border-0">
      <div className="cart-summary__header">
        <h3 className="cart-summary__title">Resumen</h3>
        <p className="cart-summary__hint">
          Total estimado. Coordiná pago y entrega por WhatsApp.
        </p>
      </div>
      <div className="cart-summary__body">
        <div className="cart-summary__row">
          <span className="cart-summary__label">Total</span>
          <span className="cart-summary__total">{formatMontoARS(total)}</span>
        </div>
        <button
          type="button"
          onClick={finalizarPorWhatsapp}
          className="cart-summary__wa"
        >
          <i className="bi bi-whatsapp" aria-hidden />
          Finalizar pedido por WhatsApp
        </button>
      </div>
    </div>
  );
};

export default CartSummary;
