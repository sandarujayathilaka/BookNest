// App.js
import React from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import AdminLayout from "./layouts/AdminLayout";
import ProductsOverview from "./pages/Products/ProductsOverview";
import ScrollToTop from "./components/general/ScrollToTop";
import Signin from "./pages/Login/Signin";
import Signup from "./pages/Login/Signup";
import NotFound from "./pages/General/NotFound";
import AddProduct from "./pages/Products/AddProduct";
// import AccountApproval from "./pages/Csr/AccountApproval";
import Orders from "./pages/Csr/Orders";
import CancelOrder from "./pages/Csr/CancelOrder";
import UnApprovedUsers from "./pages/Csr/UnApprovedUsers";
import DeactivatedProfile from "./pages/Csr/DeactivatedProfile";
import UserAccounts from "./pages/Csr/UserAccounts";



const App = () => {
  return (
    <BrowserRouter>
      <ScrollToTop>
        <Routes>
          {/* Public Routes */}
          <Route path="/login" element={<Signin />} />
          <Route path="/signup" element={<Signup />} />
          {/* Admin Layout with Nested Routes */}
          <Route element={<AdminLayout />}>
            <Route path="/" element={<ProductsOverview />} />
            <Route path="/products/new" element={<AddProduct />} />
            {/* <Route path="/accapprove" element={<AccountApproval />} /> */}
            <Route path="/orders" element={<Orders />} />
            <Route path="/cancelreq" element={<CancelOrder />} />
            <Route path="/unapprovedUser" element={<UnApprovedUsers />} />
            <Route path="/deactivatedacc" element={<DeactivatedProfile />} />
            <Route path="/useracc" element={<UserAccounts />} />
          </Route>
          {/* 404 Not Found Page */}
          <Route path="*" element={<NotFound />} />
        </Routes>
      </ScrollToTop>
    </BrowserRouter>
  );
};

export default App;
