import React from "react";
import { Navbar, Button, Dropdown, Nav, Image } from "react-bootstrap";
import { FaBell } from "react-icons/fa"; // Importing notification bell icon
import useUserStore from "../../stores/auth";
import { api } from "../../services/api.service";
import { useNavigate } from "react-router-dom";

const TopNavbar = ({ handleShow }) => {
  const { user, setUser, setToken } = useUserStore();
  console.log(user);
  // // Replace with your user's data
  // const user = {
  //   name: "John Doe",
  //   avatarUrl: "https://via.placeholder.com/40", // Sample avatar URL
  // };

  // Fallback image URL (placeholder image)
  const defaultAvatarUrl = "https://github.com/shadcn.png";

  const navigate = useNavigate();

  const handleLogout = () => {
    setToken(null);
    setUser(null);
    navigate("/signin");
  };

  return (
    <Navbar bg="light" expand="lg" className="mb-4 px-3">
      {/* Sidebar toggle button for mobile screens */}
      <Button variant="primary" className="d-lg-none me-2" onClick={handleShow}>
        ☰
      </Button>
      <Navbar.Brand href="#">Dashboard</Navbar.Brand>

      {/* Use a flexbox container for right-side content */}
      <div className="ms-auto d-flex align-items-center">
        {/* Notification icon */}
        <Nav.Item className="me-3">
          <FaBell size={20} className="text-dark" role="button" />
        </Nav.Item>

        {/* User avatar and dropdown */}
        <Dropdown align="end">
          <Dropdown.Toggle
            variant="none"
            className="d-flex border-0 align-items-center p-0"
          >
            <Image
              src={user.avatarUrl || defaultAvatarUrl}
              roundedCircle
              width="40"
              height="40"
              alt="User Avatar"
              className="me-2"
            />
            <span className="text-dark d-none d-md-block">
              {user?.fullName}
            </span>
          </Dropdown.Toggle>

          <Dropdown.Menu>
            <Dropdown.Item href="/profile">Profile</Dropdown.Item>
            <Dropdown.Item href="/settings">Settings</Dropdown.Item>
            <Dropdown.Divider />
            <Dropdown.Item onClick={handleLogout}>Sign out</Dropdown.Item>
          </Dropdown.Menu>
        </Dropdown>
      </div>
    </Navbar>
  );
};

export default TopNavbar;
