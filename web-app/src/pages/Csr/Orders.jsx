import React, { useEffect, useState } from "react";
import axios from "axios";

export default function Orders() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

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
         "Delivered", // Send status as plain string, not JSON
         {
           headers: {
             "Content-Type": "text/plain", 
           },
         }
       );
      } catch (error) {
        alert("Error changing order status.");
      }
    }
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
                  {order.status !== "Delivered" && (
                    <button
                      className="btn btn-success"
                      onClick={() => handleChangeStatus(order.orderID)}
                    >
                      Mark as Delivered
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
