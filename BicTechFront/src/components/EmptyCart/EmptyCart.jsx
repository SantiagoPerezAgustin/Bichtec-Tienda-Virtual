import { Link } from "react-router-dom";
import "../../pages/Cart/Cart.css";

const EmptyCart = () => {
  return (
    <div className="cart-empty" role="status">
      <i className="bi bi-cart-x-fill cart-empty__icon" aria-hidden />
      <p className="cart-empty__text">
        Tu carrito está vacío. Agregá productos para continuar con la compra.
      </p>
      <Link to="/productos" className="cart-empty__btn">
        Ir a productos
      </Link>
    </div>
  );
};

export default EmptyCart;
