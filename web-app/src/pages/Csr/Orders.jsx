import React, { useEffect, useState } from "react";
import axios from "axios";
import OrderDetailsModal from "./OrderDetailModel";


export default function Orders() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedOrder, setSelectedOrder] = useState(null); 

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const response = await axios.get("http://localhost:5271/api/Order/all");
        setOrders(response.data);
        setLoading(false);
      } catch (error) {
        setError("Error fetching orders");
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  // Function to change the order status to "Delivered"
  const handleChangeStatus = async (orderId) => {
    const confirm = window.confirm(
      "Are you sure you want to mark this order as Delivered?"
    );
    if (confirm) {
      try {
        await axios.put(
          `http://localhost:5271/api/Order/${orderId}/status`,
          { status: "Delivered" },
          {
            headers: {
              "Content-Type": "application/json",
            },
          }
        );

        setOrders((prevOrders) =>
          prevOrders.map((order) =>
            order.orderID === orderId
              ? { ...order, status: "Delivered" }
              : order
          )
        );
      } catch (error) {
        alert(error);
      }
    }
  };

  const handlePopup = (order) => {
    setSelectedOrder(order); // Set the selected order
  };

  const handleCloseModal = () => {
    setSelectedOrder(null); // Close the modal
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="container">
      <h2 className="my-4">Orders</h2>

      {orders.length === 0 ? (
        <div className="alert alert-warning">No orders found</div>
      ) : (
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Order ID</th>
              <th>Customer ID</th>
              <th>Order Date</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {orders.map((order) => (
              <tr key={order.orderID}>
                <td>{order.orderID}</td>
                <td>{order.customerID}</td>
                <td>{new Date(order.orderDate).toLocaleString()}</td>
                <td>{order.status}</td>
                <td>
                  <button
                    className="btn btn-success"
                    onClick={() => handleChangeStatus(order.orderID)}
                    disabled={order.status === "Delivered"}
                  >
                    {order.status === "Delivered"
                      ? "Delivered"
                      : "Mark as Delivered"}
                  </button>
                  <button
                    className="btn btn-primary ms-2"
                    onClick={() => handlePopup(order)}
                  >
                    View
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {/* Show modal if selectedOrder is not null */}
      {selectedOrder && (
        <OrderDetailsModal order={selectedOrder} onClose={handleCloseModal} />
      )}
    </div>
  );
}
