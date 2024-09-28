// Sidebar.js
import React, { useState } from "react";
import { Offcanvas, Nav } from "react-bootstrap";
import {
  FaTachometerAlt,
  FaBoxOpen,
  FaClipboardList,
  FaChartLine,
  FaCog,
  FaAngleDown,
} from "react-icons/fa"; // Importing icons

// Sample sidebar items array
const sidebarItems = [
  {
    title: "Dashboard",
    icon: <FaTachometerAlt />, // Dashboard icon
    submenu: false,
    submenuItems: [],
    link: "/dashboard", // Add link for Dashboard
  },
  {
    title: "Products",
    icon: <FaBoxOpen />, // Products icon
    submenu: true,
    submenuItems: [
      { title: "Add Product", link: "/products/new" },
      { title: "View Products", link: "/products/view" },
    ],
  },
  {
    title: "Orders",
    icon: <FaClipboardList />, // Orders icon
    submenu: false,
    submenuItems: [],
    link: "/orders", // Add link for Orders
  },
  {
    title: "Analytics",
    icon: <FaChartLine />, // Analytics icon
    submenu: true,
    submenuItems: [
      { title: "Sales Analytics", link: "/analytics/sales" },
      { title: "User Analytics", link: "/analytics/user" },
    ],
  },
  {
    title: "Settings",
    icon: <FaCog />, // Settings icon
    submenu: false,
    submenuItems: [],
    link: "/settings", // Add link for Settings
  },
  {
    title: "Account Approval",
    icon: <FaCog />, // Settings icon
    submenu: false,
    submenuItems: [],
    link: "/accapprove", // Add link for Settings
  },
];

// Sidebar content with expandable menu
const SidebarContent = () => {
  const [openItems, setOpenItems] = useState({}); // State to track which items are open

  const toggleItem = (title) => {
    setOpenItems((prevState) => ({
      ...prevState,
      [title]: !prevState[title], // Toggle the specific item's open state
    }));
  };

  return (
    <Nav className="flex-column">
      {sidebarItems.map((item, index) => (
        <div key={index}>
          {item.submenu ? ( // Render Nav.Link only for items without submenu
            <Nav.Link
              onClick={() => toggleItem(item.title)}
              className="text-white d-flex py-3 justify-content-between align-items-center rounded hover-bg"
            >
              <span className="d-flex align-items-center">
                <span className="me-3">{item.icon}</span>
                {item.title}
              </span>
              <FaAngleDown
                style={{
                  transition: "transform 0.3s ease",
                  transform: openItems[item.title]
                    ? "rotate(180deg)"
                    : "rotate(0deg)",
                }}
              />
            </Nav.Link>
          ) : (
            <Nav.Link
              href={item.link}
              className="text-white d-flex py-3 justify-content-between align-items-center rounded hover-bg"
            >
              <span className="d-flex align-items-center">
                <span className="me-3">{item.icon}</span>
                {item.title}
              </span>
            </Nav.Link>
          )}

          {item.submenu &&
            openItems[item.title] && ( // Check if item has submenu and is open
              <Nav className="flex-column">
                {/* Indent subitems */}
                {item.submenuItems.map((subItem, subIndex) => (
                  <Nav.Link
                    key={subIndex}
                    href={subItem.link}
                    className="text-white rounded hover-bg ps-5"
                  >
                    {subItem.title}
                  </Nav.Link>
                ))}
              </Nav>
            )}
        </div>
      ))}
    </Nav>
  );
};

const Sidebar = ({ show, handleClose }) => {
  const appName = "Matrix";
  return (
    <>
      {/* Sidebar for large screens (lg and above) */}
      <div className="bg-dark text-white p-3 w-25 sidebar vh-100 d-none d-lg-block">
        <h3 className="mb-4 text-center">{appName}</h3>
        <SidebarContent /> {/* Reusing the common content */}
      </div>

      {/* Sidebar for small screens (below lg) */}
      <Offcanvas
        show={show}
        onHide={handleClose}
        placement="start"
        className="bg-dark text-white d-lg-none"
      >
        <Offcanvas.Header closeButton>
          <Offcanvas.Title>{appName}</Offcanvas.Title>
        </Offcanvas.Header>
        <Offcanvas.Body>
          <SidebarContent /> {/* Reusing the common content */}
        </Offcanvas.Body>
      </Offcanvas>
    </>
  );
};

export default Sidebar;
