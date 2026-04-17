import React from "react";
import { formatMontoARS } from "../../utils/precio";

const estados = ["Pendiente", "Enviado", "Entregado", "Cancelado"];

const CardPedido = ({ pedido, onCambiarEstado }) => {
  return (
    <div style={{
      border: "1px solid #bfa100",
      borderRadius: 8,
      padding: 16,
      margin: 8,
      background: "#fffde7",
      width: 600,
      textAlign: "left"
    }}>
      <div><b>ID Pedido:</b> {pedido.id}</div>
      <div><b>Usuario:</b> {pedido.usuario?.nombre} {pedido.usuario?.apellido} ({pedido.usuario?.email})</div>
      <div><b>Dirección:</b> {pedido.direccionEnvio}</div>
      <div><b>Fecha:</b> {new Date(pedido.fechaPedido).toLocaleString()}</div>
      <div><b>Total:</b> {formatMontoARS(pedido.total)}</div>
      <div><b>Estado:</b> <span style={{fontWeight: "bold", color: "#bfa100"}}>{pedido.estado}</span></div>
      <div style={{margin: "8px 0"}}>
        <b>Productos:</b>
        <ul>
          {pedido.pedidosDetalles?.map((det, idx) => (
            <li key={idx}>
              {det.producto?.nombre || "Producto"} x{det.cantidad} — {formatMontoARS(det.precio)} (Subtotal: {formatMontoARS(det.subtotal)})
            </li>
          ))}
        </ul>
      </div>
      <div>
        {estados.map(e => (
          <button
            key={e}
            disabled={pedido.estado === e}
            style={{
              marginRight: 8,
              background: pedido.estado === e ? "#ffe066" : "#bfa100",
              color: "#222",
              border: "none",
              borderRadius: 4,
              padding: "4px 12px",
              cursor: pedido.estado === e ? "not-allowed" : "pointer",
              fontWeight: "bold"
            }}
            onClick={() => onCambiarEstado(pedido, e)}
          >
            {e}
          </button>
        ))}
      </div>
    </div>
  );
};

export default CardPedido;