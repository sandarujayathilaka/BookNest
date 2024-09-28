// ProductList.js
import React from "react";
import { Table, Button } from "react-bootstrap";

const ProductList = () => {
  return (
    <Table striped bordered hover responsive>
      <thead>
        <tr>
          <th>#</th>
          <th>Product Name</th>
          <th>Price</th>
          <th>Stock</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>1</td>
          <td>Product A</td>
          <td>$50</td>
          <td>100</td>
          <td className="d-flex display-1">
            <Button variant="warning" size="sm" className="me-2">
              Edit
            </Button>
            <Button variant="danger" size="sm">
              Delete
            </Button>
          </td>
        </tr>
        {/* More rows can be added here */}
      </tbody>
    </Table>
  );
};

export default ProductList;
