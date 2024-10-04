import React from "react";
import { Row, Col } from "react-bootstrap";
import DashboardOverview from "../../components/products/DashboardOverview";
import ProductList from "../../components/products/ProductList";
import Orders from "../../components/products/Orders";
import Analytics from "../../components/products/Analytics";
import ImageUploadForm from "../../components/products/ImageUploadForm";

const ProductsOverview = () => {
  return (
    <div>
      <DashboardOverview />
      <Row>
        <Col md={4}>
          <Orders />
        </Col>
      </Row>
      <Analytics />

      <ImageUploadForm />
    </div>
  );
};

export default ProductsOverview;
