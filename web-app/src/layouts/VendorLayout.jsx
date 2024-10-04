// VendorLayout
import { Container } from "react-bootstrap";
import Sidebar from "../components/dashboard/Sidebar";
import TopNavbar from "../components/dashboard/Navbar";
import { useState } from "react";
import { Outlet } from "react-router-dom"; 

const VendorLayout = () => {
  const [showSidebar, setShowSidebar] = useState(false);

  const handleShowSidebar = () => setShowSidebar(true);
  const handleCloseSidebar = () => setShowSidebar(false);

  return (
    <div className="d-flex">
      <Sidebar show={showSidebar} handleClose={handleCloseSidebar} />
      <Container fluid>
        <TopNavbar handleShow={handleShowSidebar} />
        {/* Render nested routes */}
        <Outlet />
      </Container>
    </div>
  );
};

export default VendorLayout;
