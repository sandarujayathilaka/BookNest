// Orders.js
import React from "react";
import { Table } from "react-bootstrap";

const Orders = () => {
  return (
    <Table striped bordered hover responsive>
      <thead>
        <tr>
          <th>#</th>
          <th>Order ID</th>
          <th>Customer</th>
          <th>Status</th>
          <th>Total</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>1</td>
          <td>ORD123</td>
          <td>John Doe</td>
          <td>Completed</td>
          <td>$200</td>
        </tr>
        {/* More rows can be added here */}
      </tbody>
    </Table>
  );
};

export default Orders;
