import React, { useState }  from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { Form, Button, Container, Alert } from "react-bootstrap";
import Toast from 'react-bootstrap/Toast';
import { useLocation } from 'react-router-dom';


const validationSchema = Yup.object().shape({
  vendorID: Yup.string().required("Vendor ID is required"),
  productID: Yup.string().required("Product ID is required"),
  productQuantity: Yup.number()
    .required("Product Quantity is required")
    .positive("Quantity must be a positive number"),
  productThreshold: Yup.number().required("Product Low Stock Threshold is required"),
  // productNotification: Yup.string().required("Product Notification is required"),
});

const InventoryAdd = () => {
  const location = useLocation();
  const product = location.state?.product || {};
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      vendorID: product.vendorID,
      productID: product.productID,
      productQuantity: product.stockQuantity
    },
  });

  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState(''); 
  const [toastVariant, setToastVariant] = useState('');

  const onSubmit = async (data) => {
    try {
        // Prepare data for stock API request
        const data1 = {
            VendorId: data.vendorID, 
            ProductId: data.productID,
            Quantity: data.productQuantity,
            LowStockThreshold: data.productThreshold,
            IsLowStockAlert: false
        };

        const isAdding = false;
        console.log(data1);

        // Step 1: Add the product to stock
        const stockResponse = await fetch(`http://localhost:5271/api/Stock?isAdding=${isAdding}`, {
            method: 'POST', 
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data1),
        });

        if (!stockResponse.ok) {
            setToastMessage(`Failed to add Inventory entry: ${stockResponse.statusText}`);
            setToastVariant('bg-danger');
            setShowToast(true);
            return;
        }

        // Parse the result from stock API (if any)
        const resultText = await stockResponse.text();
        const stockResult = resultText ? JSON.parse(resultText) : {}; 
        console.log(stockResult);

        setToastMessage('Inventory entry added successfully!');
        setToastVariant('bg-success');
        setShowToast(true);

        // Step 2: If stock addition is successful, update product status to "published"
        const productID = data.productID;
        const status = 'Published';

        const productResponse = await fetch(`http://localhost:5271/api/Product/updateStatus?productID=${productID}&status=${status}`, {
            method: 'PUT', // Assuming it's a PUT request to update product status
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (!productResponse.ok) {
            setToastMessage(`Failed to update product status: ${productResponse.statusText}`);
            setToastVariant('bg-danger');
            setShowToast(true);
            return;
        }

        console.log('Product status updated to published');
        setToastMessage('Product status updated to published successfully!');
        setToastVariant('bg-success');
        setShowToast(true);

    } catch (err) {
        console.error(err);
        setToastMessage('Error occurred while processing the product approval.');
        setToastVariant('bg-danger');
        setShowToast(true);
    }
};



  return (
    <Container className="mt-4">
      <h2>Add Inventory Entry</h2>
      <Form onSubmit={handleSubmit(onSubmit)}>
        {/* Product Name Field */}
        <Form.Group controlId="formVendorID" className="mb-3">
          <Form.Label>Vendor ID</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter vendor id"
            {...register("vendorID")}
            isInvalid={!!errors.vendorID}
            disabled
          />
          <Form.Control.Feedback type="invalid">
            {errors.vendorID?.message}
          </Form.Control.Feedback>
        </Form.Group>

      
        <Form.Group controlId="formProductID: " className="mb-3">
          <Form.Label>Product ID</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter Product ID"
            {...register("productID")}
            isInvalid={!!errors.productID}
            disabled
          />
          <Form.Control.Feedback type="invalid">
            {errors.productID?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Product Price Field */}
        <Form.Group controlId="formProductQuantity" className="mb-3">
          <Form.Label>Product Quantity</Form.Label>
          <Form.Control
            type="number"
            placeholder="Enter Product Quantity"
            {...register("productQuantity")}
            isInvalid={!!errors.productQuantity}
            disabled
          />
          <Form.Control.Feedback type="invalid">
            {errors.productQuantity?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Product Category Field */}
        <Form.Group controlId="formProductThreshold" className="mb-3">
          <Form.Label>Product Threshold</Form.Label>
          <Form.Control
            type="number"
            placeholder="Enter Product Threshold"
            {...register("productThreshold")}
            isInvalid={!!errors.productThreshold}
          />
          <Form.Control.Feedback type="invalid">
            {errors.productThreshold?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* <Form.Group controlId="formProductNotification: " className="mb-3">
          <Form.Label>Product Notification</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter Product Notification"
            {...register("productNotification")}
            isInvalid={!!errors.productNotification}
          />
          <Form.Control.Feedback type="invalid">
            {errors.productNotification?.message}
          </Form.Control.Feedback>
        </Form.Group> */}


        <Button variant="primary" type="submit">
          Add Entry
        </Button>
      </Form>
      <Toast
        onClose={() => setShowToast(false)}
        show={showToast}
        delay={3000}
        autohide
        style={{ position: 'fixed', bottom: '20px', right: '20px' }}
        className={toastVariant} 
      >
        <Toast.Header>
          <strong className="me-auto">Notification</strong>
        </Toast.Header>
        <Toast.Body>{toastMessage}</Toast.Body>
      </Toast>
    </Container>
  );
};

export default InventoryAdd;
