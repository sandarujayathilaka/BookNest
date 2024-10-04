import React, { useEffect, useState } from "react";
import { Table, Button, Spinner, Form, InputGroup } from "react-bootstrap";
import { api } from "../../services/api.service";
import { toast } from "react-toastify";
import Swal from "sweetalert2";
import { FaSearch } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [filteredProducts, setFilteredProducts] = useState([]); // Store filtered products
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchQuery, setSearchQuery] = useState(""); // Store search query

  const navigate = useNavigate();

  const fetchProducts = async () => {
    try {
      const response = await api.get("/product/vendor/products");
      setProducts(response.data);
      setFilteredProducts(response.data); // Initialize filteredProducts with all products
      console.log("Products:", response.data);
    } catch (err) {
      setError("Error fetching products");
      toast.error("Failed to load products");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  const handleEdit = (productId) => {
    navigate(`/products/edit/${productId}`);
  };

  const handleDelete = async (productId) => {
    // Show the SweetAlert confirmation dialog
    const result = await Swal.fire({
      title: "Confirmation Needed",
      text: "Please confirm your action",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "",
      confirmButtonText: "Delete",
    });

    // Check if the user confirmed the deletion
    if (result.isConfirmed) {
      try {
        await api.delete(`/product/${productId}`); // Await the delete request

        Swal.fire({
          title: "Deleted!",
          text: "Product has been deleted.",
          icon: "success",
        });
        fetchProducts();
      } catch (err) {
        console.error(err);
        toast.error(err.response.data || "Failed to delete product"); // Handle error
      }
    }
  };

  // Update filtered products when search query changes
  useEffect(() => {
    const filtered = products.filter(
      (product) =>
        product.productID.toString().includes(searchQuery) || // Search by product ID
        product.title.toLowerCase().includes(searchQuery.toLowerCase()) || // Search by title
        product.author.toLowerCase().includes(searchQuery.toLowerCase()) || // Search by author
        product.isbn.toLowerCase().includes(searchQuery.toLowerCase()) // Search by ISBN
    );
    setFilteredProducts(filtered);
  }, [searchQuery, products]); // Re-run the effect when searchQuery or products change

  if (loading) {
    return (
      <div className="text-center mt-5">
        <Spinner animation="border" />
        <p>Loading products...</p>
      </div>
    );
  }

  if (error) {
    return <div className="alert alert-danger">{error}</div>;
  }

  return (
    <div>
      {/* Search Input with Icon */}
      <Form className="mb-4">
        <Form.Group controlId="search">
          <InputGroup>
            <InputGroup.Text>
              <FaSearch />
            </InputGroup.Text>
            <Form.Control
              type="text"
              placeholder="Search by ID, Title, Author, or ISBN"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)} // Update search query
            />
          </InputGroup>
        </Form.Group>
      </Form>

      {/* Product Table */}
      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th>#</th>
            <th>Product Name</th>
            <th>Author</th>
            <th>ISBN</th>
            <th>Category</th>
            <th>Price</th>
            <th>Stock</th>
            <th>Image</th>
            <th className="text-center">Actions</th>
          </tr>
        </thead>
        <tbody>
          {filteredProducts.length > 0 ? (
            filteredProducts.map((product, index) => (
              <tr key={product.id}>
                <td className="align-middle">{product.productID}</td>
                <td className="align-middle">{product.title}</td>
                <td className="align-middle">{product.author}</td>
                <td className="align-middle">{product.isbn}</td>
                <td className="text-capitalize align-middle">
                  {product.category}
                </td>
                <td className="align-middle">Rs. {product.price}</td>
                <td className="align-middle text-center">
                  {product.stockQuantity}
                </td>
                <td className="align-middle">
                  {product.image && product.image.url ? (
                    <img
                      src={product.image.url}
                      alt={product.title}
                      style={{ width: "50px", height: "auto" }}
                    />
                  ) : (
                    "No image"
                  )}
                </td>
                <td className="align-middle">
                  <div className="d-flex justify-content-center">
                    <Button
                      variant="warning"
                      size="sm"
                      className="me-2"
                      onClick={() => handleEdit(product.productID)}
                    >
                      Edit
                    </Button>
                    <Button
                      variant="danger"
                      size="sm"
                      onClick={() => handleDelete(product.productID)}
                    >
                      Delete
                    </Button>
                  </div>
                </td>
              </tr>
            ))
          ) : (
            <tr style={{ height: "100px" }}>
              <td colSpan="9" className="text-center align-middle">
                No products found
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </div>
  );
};

export default ProductList;
