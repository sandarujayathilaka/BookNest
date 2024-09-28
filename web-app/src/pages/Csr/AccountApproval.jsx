import React, { useEffect, useState } from 'react';
import axios from 'axios';

export default function AccountApproval() {
  const [accounts, setAccounts] = useState([1,2,3]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

 
  useEffect(() => {
    const fetchAccounts = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5271/api/User/unapproved"
        ); 
        console.log(response);
        setAccounts(response.data);
      } catch (err) {
        setError('Failed to fetch accounts.');
      } finally {
        setLoading(false);
      }
    };

    fetchAccounts();
  }, []);


  const activateAccount = async (accountId) => {
    try {
      await axios.patch(
        `http://localhost:5271/api/User/approve/${accountId}`
      ); 
      setAccounts(accounts.map(account => 
        account.id === accountId ? { ...account, status: 'Active' } : account
      ));
    } catch (err) {
      alert('Error activating account.');
    }
  };

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Account Approval</h2>
      {loading ? (
        <div>Loading accounts...</div>
      ) : error ? (
        <div className="alert alert-danger">{error}</div>
      ) : accounts.length === 0 ? (
        <div className="alert alert-info">No accounts available.</div>
      ) : (
        <table className="table table-striped table-bordered">
          <thead>
            <tr>
              <th>#</th>
              <th>Name</th>
              <th>Email</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {accounts.map((account, index) => (
              <tr key={account.id}>
                <td>{index + 1}</td>
                <td>{account.fullName}</td>
                <td>{account.email}</td>
                <td>{account.isApproved ? "Approved" : "Pending"}</td>
                <td>
                  {account.status !== "Active" ? (
                    <button
                      className="btn btn-success"
                      onClick={() => activateAccount(account.userId)}
                    >
                      Activate
                    </button>
                  ) : (
                    <span>Active</span>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
