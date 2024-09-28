// Signup.js
import React from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { Form, Button, Container, Card } from "react-bootstrap";
import { Link } from "react-router-dom"; // Import Link for navigation

// Define the validation schema using yup
const schema = yup.object().shape({
  username: yup.string().required("Username is required"),
  email: yup
    .string()
    .email("Invalid email format")
    .required("Email is required"),
  password: yup
    .string()
    .min(6, "Password must be at least 6 characters long")
    .required("Password is required"),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref("password")], "Passwords must match")
    .required("Confirm Password is required"),
});

const Signup = () => {
  // Use react-hook-form with yup validation schema
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema), // Connect yup with react-hook-form
  });

  // Handle form submission
  const onSubmit = (data) => {
    console.log("Form Data:", data);
    // Implement sign-up logic here (e.g., API call)
  };

  return (
    <Container className="d-flex justify-content-center align-items-center vh-100">
      <Card className="p-4" style={{ width: "400px" }}>
        <h3 className="text-center mb-4">Sign Up</h3>
        <Form onSubmit={handleSubmit(onSubmit)}>
          {/* Username Field */}
          <Form.Group controlId="username" className="mb-3">
            <Form.Label>Username</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter username"
              {...register("username")}
              isInvalid={!!errors.username}
            />
            <Form.Control.Feedback type="invalid">
              {errors.username?.message}
            </Form.Control.Feedback>
          </Form.Group>

          {/* Email Field */}
          <Form.Group controlId="email" className="mb-3">
            <Form.Label>Email</Form.Label>
            <Form.Control
              type="email"
              placeholder="Enter email"
              {...register("email")}
              isInvalid={!!errors.email}
            />
            <Form.Control.Feedback type="invalid">
              {errors.email?.message}
            </Form.Control.Feedback>
          </Form.Group>

          {/* Password Field */}
          <Form.Group controlId="password" className="mb-3">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              placeholder="Enter password"
              {...register("password")}
              isInvalid={!!errors.password}
            />
            <Form.Control.Feedback type="invalid">
              {errors.password?.message}
            </Form.Control.Feedback>
          </Form.Group>

          {/* Confirm Password Field */}
          <Form.Group controlId="confirmPassword" className="mb-3">
            <Form.Label>Confirm Password</Form.Label>
            <Form.Control
              type="password"
              placeholder="Confirm password"
              {...register("confirmPassword")}
              isInvalid={!!errors.confirmPassword}
            />
            <Form.Control.Feedback type="invalid">
              {errors.confirmPassword?.message}
            </Form.Control.Feedback>
          </Form.Group>

          <Button variant="primary" type="submit" className="w-100">
            Sign Up
          </Button>

          {/* Link to Login Page */}
          <div className="text-center mt-3">
            <span>Already have an account? </span>
            <Link to="/login" className="text-primary">
              Sign in
            </Link>
          </div>
        </Form>
      </Card>
    </Container>
  );
};

export default Signup;
