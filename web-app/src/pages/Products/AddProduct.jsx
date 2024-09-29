import React from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { Form, Button, Container, Alert } from "react-bootstrap";

// Validation schema using Yup
const validationSchema = Yup.object().shape({
  productName: Yup.string().required("Product name is required"),
  productDescription: Yup.string().required("Product description is required"),
  productPrice: Yup.number()
    .required("Product price is required")
    .positive("Price must be a positive number"),
  productCategory: Yup.string().required("Product category is required"),
});

const AddProduct = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(validationSchema),
  });

  const onSubmit = (data) => {
    console.log("Product added", data);
    // Here you would handle form submission, such as sending data to an API
  };

  return (
    <Container className="mt-4">
      <h2>Add Product</h2>
      <Form onSubmit={handleSubmit(onSubmit)}>
        {/* Product Name Field */}
        <Form.Group controlId="formProductName" className="mb-3">
          <Form.Label>Product Name</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter product name"
            {...register("productName")}
            isInvalid={!!errors.productName}
          />
          <Form.Control.Feedback type="invalid">
            {errors.productName?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Product Description Field */}
        <Form.Group controlId="formProductDescription" className="mb-3">
          <Form.Label>Product Description</Form.Label>
          <Form.Control
            as="textarea"
            rows={3}
            placeholder="Enter product description"
            {...register("productDescription")}
            isInvalid={!!errors.productDescription}
          />
          <Form.Control.Feedback type="invalid">
            {errors.productDescription?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Product Price Field */}
        <Form.Group controlId="formProductPrice" className="mb-3">
          <Form.Label>Product Price</Form.Label>
          <Form.Control
            type="number"
            placeholder="Enter product price"
            {...register("productPrice")}
            isInvalid={!!errors.productPrice}
          />
          <Form.Control.Feedback type="invalid">
            {errors.productPrice?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Product Category Field */}
        <Form.Group controlId="formProductCategory" className="mb-3">
          <Form.Label>Product Category</Form.Label>
          <Form.Control
            as="select"
            {...register("productCategory")}
            isInvalid={!!errors.productCategory}
          >
            <option value="">Select category</option>
            <option value="electronics">Electronics</option>
            <option value="clothing">Clothing</option>
            <option value="home">Home</option>
            {/* Add more categories as needed */}
          </Form.Control>
          <Form.Control.Feedback type="invalid">
            {errors.productCategory?.message}
          </Form.Control.Feedback>
        </Form.Group>

        <Button variant="primary" type="submit">
          Add Product
        </Button>
      </Form>
    </Container>
  );
};

export default AddProduct;
