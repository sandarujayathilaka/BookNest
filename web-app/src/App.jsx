// App.js
import React from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import ProductsOverview from "./pages/Products/ProductsOverview";
import ScrollToTop from "./components/general/ScrollToTop";
import Signin from "./pages/Login/Signin";
import Signup from "./pages/Login/Signup";
import NotFound from "./pages/General/NotFound";
import AddProduct from "./pages/Products/AddProduct";
// import AccountApproval from "./pages/Csr/AccountApproval";
import Orders from "./pages/Csr/Orders";
import ProtectedRoute from "./middleware/ProtectedRoute";
import { Roles } from "./constants/roles";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import MyProducts from "./pages/Products/MyProducts";
import EditProduct from "./pages/Products/EditProduct";
import CancelOrder from "./pages/Csr/CancelOrder";
import UnApprovedUsers from "./pages/Csr/UnApprovedUsers";
import DeactivatedProfile from "./pages/Csr/DeactivatedProfile";
import UserAccounts from "./pages/Csr/UserAccounts";

const App = () => {
  return (
    <BrowserRouter>
      <ScrollToTop>
        <ToastContainer />
        <Routes>
          {/* Public Routes */}
          <Route path="/signin" element={<Signin />} />
          <Route path="/signup" element={<Signup />} />
          <Route path="/" element={<Signin />} />

          {/* common routes - for all roles */}
          <Route
            element={
              <ProtectedRoute
                roles={[Roles.ADMIN, Roles.CSR, Roles.VENDOR]}
                redirectPath="/404"
              />
            }
          >
            <Route path="/dashboard" element={<ProductsOverview />} />
          </Route>

          {/* common routes - for admin and csr roles */}
          <Route
            element={
              <ProtectedRoute
                roles={[Roles.ADMIN, Roles.CSR]}
                redirectPath="/404"
              />
            }
          >
            <Route path="/accapprove" element={<AccountApproval />} />
            <Route path="/orders" element={<Orders />} />
            <Route path="/cancelreq" element={<CancelOrder />} />
            <Route path="/unapprovedUser" element={<UnApprovedUsers />} />
            <Route path="/deactivatedacc" element={<DeactivatedProfile />} />
            <Route path="/useracc" element={<UserAccounts />} />
          </Route>

          {/* vendor routes */}
          <Route
            element={
              <ProtectedRoute roles={[Roles.VENDOR]} redirectPath="/404" />
            }
          >
            <Route path="/products/new" element={<AddProduct />} />
            <Route path="/products/edit/:id" element={<EditProduct />} />
            <Route path="/products/my" element={<MyProducts />} />
          </Route>
          {/* 404 Not Found Page */}
          <Route path="*" element={<NotFound />} />
          <Route path="/404" element={<NotFound />} />
        </Routes>
      </ScrollToTop>
    </BrowserRouter>
  );
};

export default App;
