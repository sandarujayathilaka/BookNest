// AdminLayout.js
import { Container } from "react-bootstrap";
import Sidebar from "../components/dashboard/Sidebar";
import TopNavbar from "../components/dashboard/Navbar";
import { useState } from "react";
import { Outlet } from "react-router-dom"; // Import Outlet for nested routes

const AdminLayout = ({ children }) => {
  const [showSidebar, setShowSidebar] = useState(false);

  const handleShowSidebar = () => setShowSidebar(true);
  const handleCloseSidebar = () => setShowSidebar(false);

  return (
    <div className="d-flex">
      <Sidebar show={showSidebar} handleClose={handleCloseSidebar} />
      <Container fluid>
        <TopNavbar handleShow={handleShowSidebar} />
        {children}
      </Container>
    </div>
  );
};

export default AdminLayout;
