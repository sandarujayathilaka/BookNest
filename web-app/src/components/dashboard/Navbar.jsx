import React, { useState, useEffect } from "react";
import { Navbar, Button, Dropdown, Nav, Image,Modal,Badge   } from "react-bootstrap";
import { FaBell } from "react-icons/fa"; // Importing notification bell icon
import axios from "axios"; // For API calls
import * as signalR from "@microsoft/signalr";
const TopNavbar = ({ handleShow }) => {
  // Replace with your user's data
  const user = {
    name: "John Doe",
    avatarUrl: "https://via.placeholder.com/40", // Sample avatar URL
  };
 
  const [showNotificationModal, setShowNotificationModal] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const userId = 'CUSGQ35371';  // Example user ID

  useEffect(() => {
    // Fetch notifications to update unread count
    const fetchSavedNotifications = async () => {
      try {
        let fetchedNotifications,response;
        if (userId) {
          // Fetch notifications for specific user
          response = await axios.get(`http://localhost:5271/api/notification/getNotificationsForUser?userId=${userId}`);
          fetchedNotifications = response.data;
          console.log(fetchedNotifications)
        } else {
          // Fetch notifications for all users
          response = await axios.get("http://localhost:5271/api/notification/getAll");
          fetchedNotifications = response.data;

          console.log(fetchedNotifications)
        }
        setNotifications(fetchedNotifications);
        const unreadCount = fetchedNotifications.filter(n => !n.isRead).length;
        setUnreadCount(unreadCount);  // Set unread notification count
      } catch (error) {
        console.error("Error fetching notifications: ", error);
      }
    };

    fetchSavedNotifications();

   const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJDVVNHUTM1MzcxIiwidW5pcXVlX25hbWUiOiJzYW1wbGVAZ21haWwuY29tIiwicm9sZSI6IkN1c3RvbWVyIiwibmJmIjoxNzI3NjczMjM1LCJleHAiOjE3MjgyNzgwMzUsImlhdCI6MTcyNzY3MzIzNSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3QifQ.2fJtKPx2Qd5iZWi71GJHIKZhx6Qk4DyHTZm7o0HI2dw";
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5271/notificationHub", {
      accessTokenFactory: () => token
    })
    .build();

  connection.start()
    .then(() => {
      console.log("SignalR Connected.");

      // Listen for real-time notifications
      connection.on("ReceiveNotification", (message) => {
        console.log("Real-time message received: ", message);

        // Only update unread count if the notification is for this user
        // if (message.userId === userId) {
        //   setUnreadCount((prevCount) => prevCount + 1);  // Increment unread count
        // }
        const formattedMessage = {
          id: message.id,
          message: message.message || "No message provided", // Use 'content' instead of 'message'
          userId: message.userId || "Unknown user", // Use 'user' instead of 'userId'
          timestamp: message.timestamp? new Date(message.timestamp) : new Date(), // Use 'time' instead of 'timestamp'
          isRead: false
        };
      
        // if (message.userId === userId || message.userId === null) {
        //     setNotifications((prevNotifications) => [...prevNotifications, formattedMessage]);
        // }
        setNotifications((prevNotifications) => [formattedMessage, ...prevNotifications]);
        setUnreadCount(prevCount => prevCount + 1);
      });
    })
    .catch((error) => console.error("SignalR Connection Error: ", error));

  // Cleanup connection on component unmount
  return () => {
    connection.stop();
  };
}, [userId]);  // Dependency on userId

const handleClose = () => setShowNotificationModal(false);
const handleShowModal = () => setShowNotificationModal(true);

const markAsRead = async (index) => {
  const updatedNotifications = [...notifications];
  const notification = updatedNotifications[index];
  
  if (!notification.isRead) {
    notification.isRead = true;
    setNotifications(updatedNotifications);
    const unreadCount = updatedNotifications.filter(n => !n.isRead).length;
    setUnreadCount(unreadCount);

    // Send request to backend to mark as read
    try {
     
      await axios.patch(`http://localhost:5271/api/notification/markAsRead/${userId}`, {
        id: notification.id, // cuurent user id
        isRead: true // Mark it as read
      });
      setNotifications(prev => 
        prev.map(notif => 
            notif.id === notification.id ? { ...notif, IsRead: true } : notif
        )
    );
    } catch (error) {
      console.error("Error marking notification as read: ", error);
    }
  }
};

  return (
    <>
    <Navbar bg="light" expand="lg" className="mb-4 px-3">
      {/* Sidebar toggle button for mobile screens */}
      <Button variant="primary" className="d-lg-none me-2" onClick={handleShow}>
        ☰
      </Button>
      <Navbar.Brand href="#">Dashboard</Navbar.Brand>

      {/* Use a flexbox container for right-side content */}
      <div className="ms-auto d-flex align-items-center">
        {/* Notification icon */}
        <Nav.Item className="me-3 position-relative">
          <Dropdown>
            <Dropdown.Toggle variant="none" className="d-flex align-items-center" id="dropdown-basic">
              <FaBell size={20} className="text-dark" />
              {unreadCount > 0 && (
                <Badge pill bg="danger" className="position-absolute top-0 start-100 translate-middle">
                  {unreadCount}
                </Badge>
              )}
            </Dropdown.Toggle>

            <Dropdown.Menu align="end" style={{ width: '500px', minWidth: '200px' }}>
              {notifications.length === 0 ? (
                <Dropdown.Item disabled>No notifications received yet.</Dropdown.Item>
              ) : (
                notifications.map((notification, index) => (
                  <React.Fragment key={index}>
                  <Dropdown.Item
                    key={index}
                    onClick={() => markAsRead(index)}
                    style={{
                      backgroundColor: notification.isRead ? "#f0f0f0" : "#d1e7dd",
                      cursor: "pointer",
                    }}
                  >
                    <p style={{ margin: 0 }}>Message: {notification.message}</p>
                    <p style={{ margin: 0 }}>{new Date(notification.timestamp).toLocaleString()}</p>
                  </Dropdown.Item>
                  {index < notifications.length - 1 && <div className="dropdown-divider"></div>}
                  </React.Fragment>
                ))
              )}
            </Dropdown.Menu>
          </Dropdown>
        </Nav.Item>

        {/* User avatar and dropdown */}
        <Dropdown align="end">
          <Dropdown.Toggle
            variant="none"
            className="d-flex border-0 align-items-center p-0"
          >
            <Image
              src={user.avatarUrl}
              roundedCircle
              width="40"
              height="40"
              alt="User Avatar"
              className="me-2"
            />
            <span className="text-dark d-none d-md-block">{user.name}</span>
          </Dropdown.Toggle>

          <Dropdown.Menu>
            <Dropdown.Item href="/account">Profile</Dropdown.Item>
            <Dropdown.Item href="/settings">Settings</Dropdown.Item>
            <Dropdown.Divider />
            <Dropdown.Item href="/logout">Sign out</Dropdown.Item>
          </Dropdown.Menu>
        </Dropdown>
      </div>
    </Navbar>
    
    <Modal show={showNotificationModal} onHide={handleClose} backdrop="static">
        <Modal.Header closeButton>
          <Modal.Title>Notifications</Modal.Title>
        </Modal.Header>
        <Modal.Body>
  {notifications.length === 0 ? (
    <p className="text-center">No notifications received yet.</p> // Message to show when there are no notifications
  ) : (
    <ul>
      {notifications.map((notification, index) => (
        <li
          key={index}
          onClick={() => markAsRead(index)}
          style={{
            backgroundColor: notification.isRead ? "#f0f0f0" : "#d1e7dd", // Highlight unread notifications
            padding: "10px",
            marginBottom: "5px",
            cursor: "pointer"
          }}
        >
          <p>Message: {notification.message}</p>
          <p>Timestamp: {new Date(notification.timestamp).toLocaleString()}</p>
        </li>
      ))}
    </ul>
  )}
</Modal.Body>

        <Modal.Footer>
         
        </Modal.Footer>
      </Modal>
  </>
  );
};

export default TopNavbar;
