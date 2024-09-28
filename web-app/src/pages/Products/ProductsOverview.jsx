import React from "react";
import { Row, Col } from "react-bootstrap";
import DashboardOverview from "../../components/products/DashboardOverview";
import ProductList from "../../components/products/ProductList";
import Orders from "../../components/products/Orders";
import Analytics from "../../components/products/Analytics";

const ProductsOverview = () => {
  return (
    <div>
      <DashboardOverview />
      <Row>
        <Col md={8}>
          <ProductList />
        </Col>
        <Col md={4}>
          <Orders />
        </Col>
      </Row>
      <Analytics />
    </div>
  );
};

export default ProductsOverview;
