import React, { useState, useRef, useEffect } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { Form, Button, Container, Spinner } from "react-bootstrap";
import { api } from "../../services/api.service";
import { FaTrashAlt } from "react-icons/fa"; // Import trash icon for delete action
import { toast } from "react-toastify";
import { useNavigate, useParams } from "react-router-dom"; // Import useParams

// Validation schema using Yup
const validationSchema = Yup.object().shape({
  title: Yup.string().required("Book title is required"),
  author: Yup.string().required("Author name is required"),
  ISBN: Yup.string().required("ISBN is required"),
  price: Yup.number()
    .required("Book price is required")
    .positive("Price must be a positive number")
    .min(0.01, "Price must be at least Rs. 0.01")
    .typeError("Book price is required"),
  stockQuantity: Yup.number()
    .required("Stock quantity is required")
    .min(0, "Stock quantity cannot be negative")
    .typeError("Stock quantity must be a number"),
  category: Yup.string().required("Book category is required"),
  description: Yup.string().required("Book description is required"),
  image: Yup.object().shape({
    publicId: Yup.string().required("Book image is required"),
    url: Yup.string().required("Book image is required"),
  }),
});

const EditProduct = () => {
  const { id } = useParams(); // Get the product ID from the URL parameters
  const [imageData, setImageData] = useState({ publicId: "", url: "" });
  const [isUploading, setIsUploading] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const fileInputRef = useRef(null);

  const {
    register,
    handleSubmit,
    setValue,
    setError,
    clearErrors,
    reset,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(validationSchema),
  });

  const navigate = useNavigate();

  useEffect(() => {
    // Fetch product details when the component mounts
    const fetchProduct = async () => {
      try {
        const response = await api.get(`/product/${id}`); // Adjust the endpoint as needed
        const product = response.data;

        // Set the fetched product data into the form
        reset({
          title: product.title,
          author: product.author,
          ISBN: product.isbn,
          price: product.price,
          stockQuantity: product.stockQuantity,
          category: product.category,
          description: product.description,
          image: {
            publicId: product.image.publicId,
            url: product.image.url,
          },
        });
        setImageData(product.image); // Set image data
      } catch (error) {
        console.error("Error fetching product:", error);
        toast.error("Error fetching product data");
      }
    };

    fetchProduct();
  }, [id]);

  const onSubmit = async (data) => {
    console.log("Product updated", data);
    setIsSubmitting(true);

    try {
      await api.put(`/product/${id}`, {
        ...data,
      });

      toast.success("Product updated successfully");

      // Reset the form fields after successful submission
      reset();
      setImageData({ publicId: "", url: "" });

      if (fileInputRef.current) {
        fileInputRef.current.value = "";
      }

      navigate("/products/my");
    } catch (error) {
      console.error("Error updating product:", error);
      toast.error("Error updating product");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleImageUpload = async (event) => {
    const file = event.target.files[0];
    if (file) {
      const formData = new FormData();
      formData.append("file", file);

      setIsUploading(true);
      try {
        const response = await api.post("/image/upload", formData, {
          headers: { "Content-Type": "multipart/form-data" },
        });

        if (response.status === 200) {
          setImageData(response.data);
          setValue("image", {
            publicId: response.data.publicId,
            url: response.data.url,
          });
          clearErrors("image");
          console.log("Image uploaded successfully");
        } else {
          toast.error("Error uploading image");
          console.error("Image upload failed");
        }
      } catch (error) {
        toast.error("Error uploading image");
        console.error("Error uploading image:", error);
      } finally {
        setIsUploading(false);
      }
    }
  };

  const handleDeleteImage = async (publicId) => {
    setIsDeleting(true);
    try {
      const response = await api.delete("/image/delete/" + publicId);

      if (response.status === 200) {
        setImageData({ publicId: "", url: "" });
        setValue("image", { publicId: "", url: "" });
        if (fileInputRef.current) {
          fileInputRef.current.value = "";
        }
        console.log("Image deleted successfully");
      } else {
        console.error("Image delete failed");
      }
    } catch (error) {
      console.error("Error deleting image:", error);
    } finally {
      setIsDeleting(false);
    }
  };

  const handleReset = () => {
    reset(); // Reset form fields
    if (fileInputRef.current) {
      fileInputRef.current.value = ""; // Clear file input
    }
  };

  return (
    <Container className="my-4">
      <h2>Edit Product</h2>
      <Form onSubmit={handleSubmit(onSubmit)}>
        {/* Book Title Field */}
        <Form.Group controlId="formBookTitle" className="mb-3">
          <Form.Label>Book Title</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter book title"
            {...register("title")}
            isInvalid={!!errors.title}
          />
          <Form.Control.Feedback type="invalid">
            {errors.title?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Author Name Field */}
        <Form.Group controlId="formBookAuthor" className="mb-3">
          <Form.Label>Author Name</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter author name"
            {...register("author")}
            isInvalid={!!errors.author}
          />
          <Form.Control.Feedback type="invalid">
            {errors.author?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* ISBN Field */}
        <Form.Group controlId="formBookISBN" className="mb-3">
          <Form.Label>ISBN</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter ISBN"
            {...register("ISBN")}
            isInvalid={!!errors.ISBN}
          />
          <Form.Control.Feedback type="invalid">
            {errors.ISBN?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Book Category Field */}
        <Form.Group controlId="formBookCategory" className="mb-3">
          <Form.Label>Book Category</Form.Label>
          <Form.Control
            as="select"
            {...register("category")}
            isInvalid={!!errors.category}
          >
            <option value="">Select category</option>
            <option value="fiction">Fiction</option>
            <option value="non-fiction">Non-Fiction</option>
            <option value="mystery">Mystery</option>
            <option value="fantasy">Fantasy</option>
            <option value="biography">Biography</option>
          </Form.Control>
          <Form.Control.Feedback type="invalid">
            {errors.category?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Book Price Field */}
        <Form.Group controlId="formBookPrice" className="mb-3">
          <Form.Label>Book Price (Rs.)</Form.Label>
          <Form.Control
            type="number"
            placeholder="Enter book price"
            {...register("price")}
            step="0.01"
            isInvalid={!!errors.price}
          />
          <Form.Control.Feedback type="invalid">
            {errors.price?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Stock Quantity Field */}
        <Form.Group controlId="formStockQuantity" className="mb-3">
          <Form.Label>Stock Quantity</Form.Label>
          <Form.Control
            type="number"
            placeholder="Enter stock quantity"
            {...register("stockQuantity")}
            isInvalid={!!errors.stockQuantity}
          />
          <Form.Control.Feedback type="invalid">
            {errors.stockQuantity?.message}
          </Form.Control.Feedback>
        </Form.Group>

        {/* Book Image Upload Field */}
        <Form.Group controlId="formBookImage" className="mb-3">
          <Form.Label>Book Image</Form.Label>
          <Form.Control
            type="file"
            accept="image/*"
            onChange={handleImageUpload}
            isInvalid={!!errors.image}
            ref={fileInputRef}
            disabled={isUploading}
          />
          <Form.Control.Feedback type="invalid">
            {errors.image &&
              (errors.image.publicId?.message || errors.image.url?.message)}
          </Form.Control.Feedback>

          {/* Display the uploaded image with a delete icon */}
          {isUploading ? (
            <Spinner animation="border" role="status" className="my-2">
              <span className="visually-hidden">Uploading...</span>
            </Spinner>
          ) : (
            imageData.url && (
              <div
                className="position-relative my-4"
                style={{ width: "150px" }}
              >
                <img
                  src={imageData.url}
                  alt="Uploaded"
                  className="img-fluid rounded"
                  style={{
                    width: "150px",
                    height: "150px",
                    objectFit: "cover",
                  }}
                />
                {isDeleting ? (
                  <Spinner
                    animation="border"
                    role="status"
                    size="sm"
                    className="position-absolute"
                    style={{ top: "-5px", right: "-5px", cursor: "pointer" }}
                  >
                    <span className="visually-hidden">Deleting...</span>
                  </Spinner>
                ) : (
                  <FaTrashAlt
                    onClick={() => handleDeleteImage(imageData.publicId)}
                    className="position-absolute text-danger"
                    style={{ top: "-5px", right: "-5px", cursor: "pointer" }}
                  />
                )}
              </div>
            )
          )}
        </Form.Group>

        {/* Book Description Field */}
        <Form.Group controlId="formBookDescription" className="mb-3">
          <Form.Label>Book Description</Form.Label>
          <Form.Control
            as="textarea"
            rows={3}
            placeholder="Enter book description"
            {...register("description")}
            isInvalid={!!errors.description}
          />
          <Form.Control.Feedback type="invalid">
            {errors.description?.message}
          </Form.Control.Feedback>
        </Form.Group>

        <Button
          variant="primary"
          type="submit"
          disabled={isSubmitting}
          style={{ minWidth: "100px" }}
        >
          {isSubmitting ? (
            <>
              <Spinner animation="border" size="sm" />
              <span className="ms-2">Updating...</span>
            </>
          ) : (
            "Update"
          )}
        </Button>

        <Button
          variant="secondary"
          type="button"
          onClick={handleReset}
          className="ms-2"
          style={{ minWidth: "100px" }}
        >
          Reset
        </Button>
      </Form>
    </Container>
  );
};

export default EditProduct;
