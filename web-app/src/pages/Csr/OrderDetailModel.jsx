import React from "react";

const OrderDetailsModal = ({ order, onClose }) => {
  return (
    <div className="modal fade show" style={{ display: "block" }} tabIndex="-1">
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Order Details</h5>
            <button
              type="button"
              className="btn-close"
              onClick={onClose}
            ></button>
          </div>
          <div className="modal-body">
            <p>
              <strong>Order ID:</strong> {order.OrderID}
            </p>
            <p>
              <strong>Customer ID:</strong> {order.CustomerID}
            </p>
            <p>
              <strong>Order Date:</strong>{" "}
              {new Date(order.OrderDate).toLocaleString()}
            </p>
            <p>
              <strong>Status:</strong> {order.Status}
            </p>
            <p>
              <strong>Total Items:</strong> {order.TotalItems}
            </p>
            <p>
              <strong>Total Amount:</strong> {order.TotalAmount}
            </p>
            <h6>Products:</h6>
            <ul>
              {order.Products.map((product, index) => (
                <li key={index}>
                  {product.ProductID} - {product.TotalItems} pcs @{" "}
                  {product.UnitPrice} each
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderDetailsModal;
