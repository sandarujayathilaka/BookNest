import React, { useEffect, useState } from 'react';
import { Table, Button, Modal, Form  } from 'react-bootstrap';
import Toast from 'react-bootstrap/Toast';

const InventoryList = () => {
  const [products, setProducts] = useState([]); 
  const [loading, setLoading] = useState(true); 
  const [error, setError] = useState(null); 
  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState(''); 
  const [toastVariant, setToastVariant] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [selectedProductId, setSelectedProductId] = useState(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedStatus, setSelectedStatus] = useState('');
  

  useEffect(() => {
    const fetchInventory = async () => {
      try {
        const response = await fetch('http://localhost:5271/api/Stock');
       
        if (!response.ok) {
          throw new Error('Failed to fetch inventory');
        }
        const data = await response.json(); 
        console.log(data);
        setProducts(data); 
        setLoading(false); 
      } catch (err) {
        setError(err.message); 
        setLoading(false); 
      }
    };

    fetchInventory();
  }, []); 

 
  const handleRemove = async () => {
    try {
      const response = await fetch(`http://localhost:5271/api/Stock/${selectedProductId}`, {
        method: 'DELETE',
      });

      const result = await response.json();

      if (response.ok) {
        setProducts(products.filter((product) => product.id !== selectedProductId));
        setToastMessage(`Product  removed successfully!`);
        setToastVariant('bg-success'); 
      } else {
        setToastMessage(result.message || 'Failed to remove product.');
        setToastVariant('bg-danger'); 
      }
  
      setShowToast(true); 
    } catch (err) {

      setToastMessage('Error occurred while removing product.');
      setToastVariant('bg-danger'); 
      setShowToast(true);
    }
    setShowModal(false);
  };

  const confirmDelete = (productId) => {
    setSelectedProductId(productId);
    setShowModal(true); 
  };
  const handleNotification = async (productId) => {
    try {
      // Assuming you can extract vendorId from the product based on productId
      const product = products.find((product) => product.id === productId);
      if (!product) {
        throw new Error('Product not found');
      }
      console.log(product)
  const vendorId = product.userId;
      // Construct the URL with the vendorId parameter
      //const url = `http://localhost:5271/api/Notification/sendToVendor?vendorId=${product.vendorId}`;
      const notificationUrl  = `http://localhost:5271/api/Notification/sendToVendor?vendorId=${vendorId}`;
  
      // Define the message to send in the request body
      const message =  `Stock Alert for Product ${product.productId}. Current stock quantity is ${product.quantity}.`;
  
      // Send a POST request with the message in the body
      const notificationResponse  = await fetch(notificationUrl , {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(message),
      });
  
      if (!notificationResponse .ok) {
        throw new Error('Failed to send notification');
      }
  
      const stockUrl = `http://localhost:5271/api/Stock/updateLowStockStatus`;
      const stockUpdatePayload = {
        productId: product.productId, // Product identifier
        isLowStockAlert: true, // Set isLowStockAlert to true
        userId: vendorId, 
        vendorId : product.vendorId

      };

      const stockResponse = await fetch(stockUrl, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(stockUpdatePayload),
      });
  
      if (!stockResponse.ok) {
        throw new Error('Failed to update stock status');
      }
      const updatedProducts = products.map((prod) =>
        prod.id === product.id ? { ...prod, isLowStockAlert: true } : prod
      );
  
      setProducts(updatedProducts);
      // Display success message using toast
        setToastMessage(`Notification sent successfully and stock updated for Vendor ID: ${vendorId}`);
      setToastVariant('bg-success');
    } catch (err) {
      setToastMessage(`Error sending notification: ${err.message}`);
      setToastVariant('bg-danger');
    }
  
    setShowToast(true);
  };
  

  if (loading) {
    return <p>Loading inventory...</p>;
  }

  if (error) {
    return <p>Error loading inventory: {error}</p>;
  }
  const filteredProducts = products.filter((product) => {
    const matchesSearch =
      product.productId.toLowerCase().includes(searchQuery.toLowerCase()) ||
      product.vendorId.toLowerCase().includes(searchQuery.toLowerCase());
  
      const matchesStatus =
      selectedStatus
        ? product.isLowStockAlert === (selectedStatus === 'Yes')
        : true;
    
  
    return matchesSearch && matchesStatus;
  });
  
  return (
    <div className="mb-3" style={{ backgroundColor: 'white', minHeight: '100vh' }}>
      <div className="row align-items-center mb-3">
    <div className="col">
      <h3 className="fw-bold">Inventory Entries</h3>
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
            <option value="Yes">Yes</option>
            <option value="No">No</option>
          </Form.Select>
        </Form.Group>
      </div>
    </div>
  </div>

      
      <Table striped bordered hover className='text-center '>
        <thead>
          <tr>
            <th>Vendor ID</th>
            <th>Product ID</th>
            <th>Stock Quantity</th>
            <th>Low Stock Threshold</th>
            <th>Notification Send</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
  {filteredProducts.length === 0 ? (
    <tr>
      <td colSpan="6" className="text-center fw-bold">
        There are no inventory details to show.
      </td>
    </tr>
  ) : (
    filteredProducts.map((product) => (
      <tr key={product.id}>
        <td>{product.vendorId}</td>
        <td>{product.productId}</td>
        <td>{product.quantity}</td>
        <td>{product.lowStockThreshold}</td>
        <td>{product.isLowStockAlert ? "Yes" : "No"}</td>
        <td>
          <Button
            variant="primary"
            onClick={() => handleNotification(product.id)}
            className="me-2"
          >
            Send Notification
          </Button>
          <Button
            variant="danger"
            onClick={() => confirmDelete(product.id)}
          >
            Remove
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
        style={{ position: 'fixed', bottom: '20px', right: '20px' }}
        className={toastVariant} 
      >
        <Toast.Header>
          <strong className="me-auto">Notification</strong>
        </Toast.Header>
        <Toast.Body>{toastMessage}</Toast.Body>
      </Toast>

      <Modal show={showModal} onHide={() => setShowModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Confirm Product Deletion</Modal.Title>
        </Modal.Header>
        <Modal.Body>Are you sure you want to delete this product?</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowModal(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleRemove}>
            Yes, Delete
          </Button>
        </Modal.Footer>
      </Modal>
      
    </div>
  );
};

export default InventoryList;
