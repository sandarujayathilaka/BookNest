import React from "react";

const Footer = () => {
  return (
    <footer className="bg-dark text-light py-4">
      <div className="container">
        <div className="row">
          <div className="col text-center">
            <h5>Company Name</h5>
            <p>© {new Date().getFullYear()} All Rights Reserved</p>
          </div>
        </div>
        <div className="row">
          <div className="col text-center">
            <ul className="list-inline">
              <li className="list-inline-item">
                <a href="#" className="text-light">
                  Privacy Policy
                </a>
              </li>
              <li className="list-inline-item">
                <a href="#" className="text-light">
                  Terms of Service
                </a>
              </li>
              <li className="list-inline-item">
                <a href="#" className="text-light">
                  Contact Us
                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
