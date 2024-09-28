// DashboardOverview.js
import React from "react";
import { Card, Row, Col } from "react-bootstrap";

const DashboardOverview = () => {
  return (
    <Row>
      <Col md={4}>
        <Card className="mb-4">
          <Card.Body>
            <Card.Title>Sales</Card.Title>
            <Card.Text>$10,000</Card.Text>
          </Card.Body>
        </Card>
      </Col>
      <Col md={4}>
        <Card className="mb-4">
          <Card.Body>
            <Card.Title>Orders</Card.Title>
            <Card.Text>150</Card.Text>
          </Card.Body>
        </Card>
      </Col>
      <Col md={4}>
        <Card className="mb-4">
          <Card.Body>
            <Card.Title>Customers</Card.Title>
            <Card.Text>1,200</Card.Text>
          </Card.Body>
        </Card>
      </Col>
    </Row>
  );
};

export default DashboardOverview;
