import React, { useEffect, useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css"; // Make sure Bootstrap is included

export default function CancelOrder() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [cancelationNote, setCancelationNote] = useState("");
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    // Fetch all orders
    const fetchOrders = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5271/api/Order/cancele_order_req"
        );
        setOrders(response.data);
        setLoading(false);
      } catch (error) {
        setError("Error fetching orders");
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  const handleCancelClick = (order) => {
    setSelectedOrder(order);
    setShowModal(true); // Show the modal
  };

  const handleCancelOrder = async () => {
    try {
      await axios.put(
        `http://localhost:5271/api/Order/${selectedOrder.orderID}/cancelbyofficer`,
        {
          CancelationOfficerNote: cancelationNote,
          Status: "Canceled",
        }
      );
      setOrders((prevOrders) =>
        prevOrders.map((order) =>
          order.orderID === selectedOrder.orderID
            ? { ...order, orderCancelation: true, cancelationNote }
            : order
        )
      );
      setShowModal(false);
      setCancelationNote(""); // Reset the note
    } catch (error) {
      alert("Error canceling the order: " + error.message);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div className="alert alert-danger">{error}</div>;
  }

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Orders</h2>
      {orders.length === 0 ? (
        <div className="alert alert-warning">No orders found</div>
      ) : (
        <table className="table table-striped table-bordered">
          <thead className="thead-dark">
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
                    className="btn btn-danger"
                    onClick={() => handleCancelClick(order)}
                  >
                    {order.orderCancelation === true ? "Canceled" : "Cancel"}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {/* Modal */}
      {showModal && (
        <div className="modal fade show" style={{ display: "block" }}>
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title" id="exampleModalLabel">
                  Cancel Order {selectedOrder?.orderID}
                </h5>
                <button
                  type="button"
                  className="close"
                  onClick={() => setShowModal(false)}
                >
                  <span>&times;</span>
                </button>
              </div>
              <div className="modal-body">
                <div className="form-group">
                  <label htmlFor="cancelation-note" className="col-form-label">
                    Cancellation Note:
                  </label>
                  <textarea
                    className="form-control"
                    id="cancelation-note"
                    value={cancelationNote}
                    onChange={(e) => setCancelationNote(e.target.value)}
                  ></textarea>
                </div>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={() => setShowModal(false)}
                >
                  Close
                </button>
                <button
                  type="button"
                  className="btn btn-primary"
                  onClick={handleCancelOrder}
                >
                  Submit Cancellation
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
