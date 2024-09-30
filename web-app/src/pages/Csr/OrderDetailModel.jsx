import React from "react";

const OrderDetailsModal = ({ order, onClose }) => {
  console.log(order);

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
              <strong>Order ID:</strong> {order.orderID}
            </p>
            <p>
              <strong>Customer ID:</strong> {order.customerID}
            </p>
            <p>
              <strong>Order Date:</strong>{" "}
              {new Date(order.orderDate).toLocaleString()}
            </p>
            <p>
              <strong>Status:</strong> {order.status}
            </p>
            <p>
              <strong>Total Items:</strong> {order.totalItems}
            </p>
            <p>
              <strong>Total Amount:</strong> {order.totalAmount}
            </p>
            <h6>Products:</h6>
            {/* Product Table */}
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>Product ID</th>
                  <th>Total Items</th>
                  <th>Unit Price</th>
                  <th>Total Price</th>
                </tr>
              </thead>
              <tbody>
                {order.products.map((product, index) => (
                  <tr key={index}>
                    <td>{product.productID}</td>
                    <td>{product.totalItems} pcs</td>
                    <td>{product.unitPrice.toFixed(2)} each</td>
                    <td>
                      {(product.unitPrice * product.totalItems).toFixed(2)}
                    </td>
                  </tr>
                ))}
                {order.products.length === 0 && (
                  <tr>
                    <td colSpan="4" className="text-center">
                      No products available
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderDetailsModal;
