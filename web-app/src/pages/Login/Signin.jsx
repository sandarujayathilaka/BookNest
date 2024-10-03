// Signin.js
import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { Form, Button, Container, Row, Col, Alert } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { api } from "../../services/api.service";
import useUserStore from "../../stores/auth";

// Validation schema using Yup
const validationSchema = Yup.object().shape({
  email: Yup.string()
    .email("Please enter a valid email address")
    .required("Email is required"),
  password: Yup.string()
    .min(6, "Password must be at least 6 characters")
    .required("Password is required"),
});

const Signin = () => {
  // Initialize the useForm hook with resolver for yup validation
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(validationSchema),
  });

  const navigate = useNavigate();

  const { setToken, setUser } = useUserStore();

  const [error, setError] = useState(null);

  // Handle form submission
  const onSubmit = (data) => {
    console.log(data); // Placeholder for form submission logic

    // Make API call to login
    api
      .post("/user/login", data)
      .then((response) => {
        console.log(response.data);

        // Assuming the token and user data are part of the response
        const { token, user } = response.data;

        if (user.role === "Customer") {
          setError("Invalid email or password");
          return;
        }

        // Save token and user data to the zustand store
        setToken(token);
        setUser(user);

        // Redirect to home page after login
        navigate("/dashboard");
      })
      .catch((error) => {
        console.error("Login failed", error);
        error.response.status === 401
          ? setError("Invalid email or password")
          : setError(error.response.data);
      });
  };

  return (
    <Container className="d-flex justify-content-center align-items-center vh-100">
      <Row className="w-100">
        <Col md={6} className="mx-auto">
          <h2 className="text-center mb-4">Sign in</h2>

          {/* Display error message */}
          {error && (
            <Alert variant="danger" onClose={() => setError(null)} dismissible>
              {error}
            </Alert>
          )}

          {/* Form with React Hook Form handling */}
          <Form onSubmit={handleSubmit(onSubmit)}>
            {/* Email Input Field */}
            <Form.Group controlId="formEmail" className="mb-3">
              <Form.Label>Email address</Form.Label>
              <Form.Control
                type="email"
                placeholder="Enter your email"
                {...register("email")}
                className={errors.email ? "is-invalid" : ""}
              />
              {/* Error Message for Email */}
              {errors.email && (
                <Form.Text className="text-danger">
                  {errors.email.message}
                </Form.Text>
              )}
            </Form.Group>

            {/* Password Input Field */}
            <Form.Group controlId="formPassword" className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="Enter your password"
                {...register("password")}
                className={errors.password ? "is-invalid" : ""}
              />
              {/* Error Message for Password */}
              {errors.password && (
                <Form.Text className="text-danger">
                  {errors.password.message}
                </Form.Text>
              )}
            </Form.Group>

            {/* Submit Button */}
            <Button variant="primary" type="submit" className="w-100">
              Login
            </Button>
          </Form>

          {/* Optional Links */}
          <div className="text-center mt-3">
            <a href="/forgot-password" className="text-decoration-none">
              Forgot Password?
            </a>
            <br />
            <a href="/signup" className="text-decoration-none">
              Create an Account
            </a>
          </div>
        </Col>
      </Row>
    </Container>
  );
};

export default Signin;
