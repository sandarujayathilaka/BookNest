import React, { useEffect, useState } from 'react';
import { Table, Button, Modal, Form,Container, Row, Col } from 'react-bootstrap';
import Toast from 'react-bootstrap/Toast';
import { useNavigate } from 'react-router-dom';
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";

const validationSchema = Yup.object().shape({
  vendorID: Yup.string().required("Vendor ID is required"),
  productID: Yup.string().required("Product ID is required"),
  productQuantity: Yup.number()
    .required("Product Quantity is required")
    .positive("Quantity must be a positive number"),
  productThreshold: Yup.number().required("Product Low Stock Threshold is required"),
});


const ProductApproval = () => {
  const [products, setProducts] = useState([]); 
  const [loading, setLoading] = useState(true); 
  const [error, setError] = useState(null); 
  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState(''); 
  const [toastVariant, setToastVariant] = useState('');
  const [showModal, setShowModal] = useState(false); 
  const [deniedMessage, setDeniedMessage] = useState(''); 
  const [currentProductId, setCurrentProductId] = useState(null);
  const [currentProduct, setCurrentProduct] = useState(null); 
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [showAddInventoryModal, setShowAddInventoryModal] = useState(false);
  const [productToDelete, setProductToDelete] = useState(null);
  const [searchQuery, setSearchQuery] = useState('');
const [selectedStatus, setSelectedStatus] = useState('');
const [attemptedSubmit, setAttemptedSubmit] = useState(false);
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset, // Add reset function
  } = useForm({
    resolver: yupResolver(validationSchema),
  });

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const response = await fetch('http://localhost:5271/api/Product');
       
        if (!response.ok) {
          throw new Error('Failed to fetch Product');
        }
        const data = await response.json(); 
        const filteredData = data.filter(product => product.status !== 'Published'); 
        console.log(filteredData);
        setProducts(filteredData); 
        setLoading(false); 
      } catch (err) {
        setError(err.message); 
        setLoading(false); 
      }
    };

    fetchProduct();
  }, []); 


  useEffect(() => {
    if (currentProduct) {
      reset({
        vendorID: currentProduct.vendorID,
        productID: currentProduct.productID,
        productQuantity: currentProduct.stockQuantity,
        productThreshold: 0, // Default value or adjust as needed
      });
    }
  }, [currentProduct, reset]);

  const handleView = (product) => {
    setCurrentProduct(product); 
    setShowModal(true); 
  };

  const handleApprove = () => {
    setShowAddInventoryModal(true);
  }
  const onSubmit = async (data) => {
    try {
      const productID = data.productID;
      const status = 'Published';
  
      // First, update the product status to "published"
      const productResponse = await fetch(`http://localhost:5271/api/Product/updateStatus?productID=${productID}&status=${status}`, {
        method: 'PUT', 
        headers: {
          'Content-Type': 'application/json',
        }
      });
  
      // Check if the PUT request was successful
      if (!productResponse.ok) {
        throw new Error(`Failed to update product status: ${productResponse.statusText}`);
      }
  
      // If product status update is successful, proceed to post the inventory
      const data1 = {
        VendorId: data.vendorID, 
        ProductId: productID,
        UserId: currentProduct.userId,
        Quantity: data.productQuantity,
        LowStockThreshold: data.productThreshold,
        IsLowStockAlert: false
      };
  console.log(data1)
      
      const stockResponse = await fetch(`http://localhost:5271/api/Stock/add`, {
        method: 'POST', 
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data1),
      });
  
      // Check if the POST request was successful
      if (!stockResponse.ok) {
        throw new Error(`Failed to add Inventory entry: ${stockResponse.statusText}`);
      }
  
      // If both requests are successful
      setToastMessage('Product approved and inventory entry added successfully!');
      setToastVariant('bg-success');
      setShowToast(true);
  
      // Remove the approved product from the list
      setProducts((prevProducts) => 
        prevProducts.filter((product) => product.productID !== productID)
      );
  
      setShowModal(false);
      setShowAddInventoryModal(false); // Close the inventory modal
      
      // Navigate to /inventory after 3 seconds
      setTimeout(() => {
        navigate('/inventory');
      }, 3000);
  
    } catch (err) {
      // If any request fails, set the error toast message
      setToastMessage('Error occurred: ' + err.message);
      setToastVariant('bg-danger');
      setShowToast(true);
    }
  };
  

  const handleDeny = async () => {
    // if (window.confirm("Are you sure you want to deny this product?")) {
    //   setCurrentProductId(productId);
    //   setShowModal(true); 
    // }
    //setCurrentProductId(productId);
   
  setShowDeleteModal(true);
  };

  const confirmDeny = async () => {
    setAttemptedSubmit(true); // Mark that user tried to submit
    if (deniedMessage.trim() === '') {
      // Do not proceed if the message is empty
      return;
    }
    setDeniedMessage('');
    setShowDeleteModal(false);
    setAttemptedSubmit(false);
    try {
      const status = "Denied";
      const response = await fetch(`http://localhost:5271/api/Product/updateStatus?productID=${currentProduct.productID}&status=${status}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify( deniedMessage ), 
      });

      if (!response.ok) {
        throw new Error('Failed to deny product');
      }

      const updatedProduct = await response.json();
      
      setProducts((prevProducts) =>
        prevProducts.map((product) =>
          product.productID === currentProduct.productID
            ? { ...product, status: 'Denied', deniedMessage: deniedMessage }
            : product
        )
      );

      setShowModal(false);
      setToastMessage('Product denied successfully');
      setToastVariant('bg-success');
      setShowToast(true);
      setDeniedMessage('');
      setCurrentProduct(null);
    } catch (err) {
      setToastMessage('Error denying product: ' + err.message);
      setToastVariant('bg-danger text-light');
      setShowToast(true);
    }
  };

  // const handleApprove = async () => {
  //   navigate('/inventory/new', { state: { product: currentProduct } });
  //   setShowModal(false);
  // }

  const handleConfirmDeny = async () => {
    try {
      console.log(currentProductId,deniedMessage)
      const status = "Denied";
        const response = await fetch(`http://localhost:5271/api/Product/updateStatus?productID=${currentProductId}&status=${status}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(deniedMessage), // Send the denied message in the body
        });
      
      if (!response.ok) {
        setShowModal(false);
      setToastMessage('Product denied with message: ' + deniedMessage);
      setToastVariant('bg-danger text-light');
      setShowToast(true);
      setDeniedMessage('');
      setCurrentProductId(null);
      }
  
      const updatedProduct = await response.json();
      
      setProducts((prevProducts) =>
        prevProducts.map((product) =>
          product.productID === currentProductId
            ? { ...product, status: 'Denied', deniedMessage: deniedMessage }
            : product
        )
      );
  
      setShowModal(false);
      setToastMessage('Product denied successfully');
      setToastVariant('bg-success');
      setShowToast(true);
      setDeniedMessage('');
      setCurrentProductId(null);
    } catch (err) {
      console.error(err);
      setToastMessage('Error denying product.'+err);
      setToastVariant('bg-danger text-light');
      setShowToast(true);
    }
  };
  

  if (loading) {
    return <p>Loading Approval...</p>;
  }

  if (error) {
    return <p>Error loading Approval: {error}</p>;
  }

  const filteredProducts = products.filter((product) => {
    const matchesSearch =
      product.productID.toLowerCase().includes(searchQuery.toLowerCase()) ||
      product.vendorID.toLowerCase().includes(searchQuery.toLowerCase());
  
    const matchesStatus = selectedStatus
      ? product.status === selectedStatus
      : true;
  
    return matchesSearch && matchesStatus;
  });
  
  return (
    <div className="mb-3" style={{ backgroundColor: 'white', minHeight: '100vh' }}>
     <div className="row align-items-center mb-3">
    <div className="col">
      <h3 className="fw-bold">Approval for Product</h3>
    </div>
    <div className="col text-end">
      <div className="d-flex justify-content-end align-items-center"> {/* Aligns items to the end */}
        {/* Search Bar */}
        <Form.Group controlId="searchBar" className="mb-0 me-2"> {/* Using me-2 for spacing */}
          <Form.Control
            type="text"
            placeholder="Search by Product ID or Vendor ID"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            style={{ width: '300px' }} // Adjust width as needed
          />
        </Form.Group>
        {/* Status Dropdown */}
        <Form.Group controlId="statusDropdown" className="mb-0">
          <Form.Select
            value={selectedStatus}
            onChange={(e) => setSelectedStatus(e.target.value)}
          >
            <option value="">All Statuses</option>
            <option value="Denied">Denied</option>
            <option value="Pending">Pending</option>
          </Form.Select>
        </Form.Group>
      </div>
    </div>
  </div>

      <Table striped bordered hover className='text-center'>
        <thead>
          <tr>
            <th>Vendor ID</th>
            <th>Product ID</th>
           
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
  {filteredProducts.length === 0 ? (
    <tr>
      <td colSpan="6" className="text-center fw-bold">
        There are no product details to show for approvals.
      </td>
    </tr>
  ) : (
    filteredProducts.map((product) => (
      <tr key={product.id}>
        <td>{product.vendorID}</td>
        <td>{product.productID}</td>
   
        <td>{product.status}</td>
        <td>
        <Button
                    variant="primary"
                    onClick={() => handleView(product)}
                  >
                    View
                  </Button>
        </td>
      </tr>
    ))
  )}
</tbody>

      </Table>
   
      <Toast
        onClose={() => setShowToast(false)}
        show={showToast}
        delay={3000}
        autohide
        style={{ position: 'fixed', bottom: '20px', right: '20px' , zIndex: 1051, // Ensure this is higher than any modal's z-index
          backgroundColor: toastVariant === 'bg-danger' ? '#f8d7da' : '#d4edda', // Custom background color based on variant
        }}
        className={toastVariant} 
      >
        <Toast.Header>
          <strong className="me-auto">Notification</strong>
        </Toast.Header>
        <Toast.Body>{toastMessage}</Toast.Body>
      </Toast>

      <Modal show={showModal} onHide={() => setShowModal(false)} backdrop="static" size="lg" style={{ backdropFilter: 'none', zIndex: 1050 }}>
        <Modal.Header closeButton className="justify-content-center">
          <Modal.Title className="w-100 text-center fs-2">Vendor and Product Details</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {currentProduct && (
            <Container>
              <Row>
                {/* Vendor Details on the left side */}
                <Col md={6}>
                  <h4 className="mb-3">Vendor Details</h4>
                  <p><strong>Vendor ID:</strong> {currentProduct.vendorID}</p>
                  <p><strong>Vendor Name:</strong> {currentProduct.vendorName}</p>
                  <p><strong>Vendor Rating:</strong> {currentProduct.vendorRating}</p>
                  <p><strong>Vendor Delivery Success Rate:</strong> {currentProduct.vendorSuccessRate}</p>
                  <p><strong>Vendor Delivery Unsuccess Rate:</strong> {currentProduct.vendorSuccessRate}</p>
                  <p><strong>Vendor List of reasons for unsuccess delivery:</strong> {currentProduct.vendorSuccessRate}</p>
                </Col>
                
                {/* Product Details on the right side */}
                <Col md={6}>
                  <h4 className="mb-3">Product Details</h4>
                  <p><strong>Product ID:</strong> {currentProduct.productID}</p>
                  <p><strong>Product Name:</strong> {currentProduct.name}</p>
                  <p><strong>Product Description:</strong> {currentProduct.description}</p>
                  <p><strong>Product Price:</strong> {currentProduct.price}</p>
                  <p><strong>Product Quantity:</strong> {currentProduct.stockQuantity}</p>
                  <p><strong>Status:</strong> {currentProduct.status}</p>
                </Col>
              </Row>

              {/* Centered Approve and Deny Buttons */}
              <Row className="mt-4">
                <Col className="d-flex justify-content-center">
                  {currentProduct.status !== 'Denied' ? (
                    <>
                      <Button variant="success" onClick={handleApprove} className="me-2">
                        Approve
                      </Button>
                      <Button variant="danger" onClick={handleDeny}>
                        Deny
                      </Button>
                    </>
                  ) : (
                    <span style={{ color: 'red' }}>{currentProduct.deniedMessage}</span>
                  )}
                </Col>
              </Row>
            </Container>
          )}
        </Modal.Body>

        <Modal.Footer>
          {/* <Button variant="secondary" onClick={() => setShowModal(false)}>
            Close
          </Button> */}
        </Modal.Footer>
      </Modal>

      <Modal show={showAddInventoryModal} onHide={() => setShowAddInventoryModal(false)} size="lg">
        <Modal.Header closeButton>
          <Modal.Title>Add Inventory Entry</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleSubmit(onSubmit)}>
            {/* Vendor ID */}
            <Form.Group controlId="formVendorID" className="mb-3">
              <Form.Label>Vendor ID</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter vendor id"
                {...register("vendorID")}
                isInvalid={!!errors.vendorID}
                value={currentProduct?.vendorID || ''}
                disabled
              />
              <Form.Control.Feedback type="invalid">
                {errors.vendorID?.message}
              </Form.Control.Feedback>
            </Form.Group>

            {/* Product ID */}
            <Form.Group controlId="formProductID" className="mb-3">
              <Form.Label>Product ID</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter Product ID"
                {...register("productID")}
                isInvalid={!!errors.productID}
                value={currentProduct?.productID || ''}
                disabled
              />
              <Form.Control.Feedback type="invalid">
                {errors.productID?.message}
              </Form.Control.Feedback>
            </Form.Group>

            {/* Product Quantity */}
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

            {/* Product Threshold */}
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

            <Row className="mt-3">
      <Col className="d-flex justify-content-center">
        <Button variant="primary" type="submit">
          Add Entry
        </Button>
      </Col>
    </Row>
          </Form>
        </Modal.Body>
        <Modal.Footer>

        </Modal.Footer>
      </Modal>

      {/* Confirm Deny Modal */}
      <Modal show={showDeleteModal} onHide={() => setShowDeleteModal(false)} backdrop="static">
        <Modal.Header closeButton className="justify-content-center">
          <Modal.Title  className="w-100 text-center fs-3">Reason for Denying</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group>
              <Form.Label>Deny Message</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter the denial reason to proceed with Confirm Deny"
                value={deniedMessage}
                onChange={(e) => setDeniedMessage(e.target.value)}
                required
              />
              {attemptedSubmit && deniedMessage.trim() === '' && ( // Show error only if attempted to submit
              <Form.Text className="text-danger">Reason for denial is required.</Form.Text>
            )}
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={confirmDeny} disabled={deniedMessage.trim() === ''}>
            Confirm Deny
          </Button>
        </Modal.Footer>
      </Modal>

    </div>
  );
};

export default ProductApproval;
