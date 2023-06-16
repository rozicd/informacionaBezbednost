import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import {
  checkCookieValidity,
  GetUserByEmail,
} from '../services/authService';
import './RequestList.css';

import {
  getAllRequest,
  getUsersRequest,
  acceptRequest,
  declineRequest,
} from '../services/requestService';

function Requests({ role }) {
  const [user, setUser] = useState({});
  const [email, setEmail] = useState('');
  const [requests, setRequests] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const location = useLocation();
  const [totalItems, setTotalItems] = useState(0);

  async function fetchRequests() {
    let response = null;

    if (user.role === 2) {
      try {
        response = await getAllRequest(currentPage);
        console.log(response)
      } catch (error) {
        console.error(error);
      }
    } else if (user.role === 1) {
      try {
        response = await getUsersRequest(user.id, currentPage);
      } catch (error) {
        console.error(error);
      }
    }

    if (response) {
      setRequests(response.items);
      setTotalItems(response.totalItems);
    }
  }

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  async function checkAuthentication() {
    try {
      const data = await checkCookieValidity();
      setEmail(data);
      return data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  async function getUser(email) {
    try {
      const data = await GetUserByEmail(email);
      setUser(data);
      return data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  const handleRequestButtonClick = async () => {
    try {
      const email = await checkAuthentication();
      const userData = await getUser(email);
      setUser(userData);
      await fetchRequests();
    } catch (error) {
      // Handle errors
    }
  };

  useEffect(() => {
    handleRequestButtonClick();
  }, [currentPage, location]);

  async function handleAccept(requestId) {
    try {
      const acceptResponse = await acceptRequest(requestId);
      window.alert(acceptResponse.data);
    } catch (error) {
      window.alert(error);
    }
  }

  async function handleDecline(requestId) {
    try {
      const acceptResponse = await declineRequest(requestId);
      window.alert(acceptResponse.data);
    } catch (error) {
      window.alert(error);
    }
  }

  const totalPages = Math.ceil(totalItems / pageSize);

  return (
    <div className='center-list'>
      <table className="table table-bordered">
        <thead>
          <tr>
            <th>ID</th>
            <th>Certificate Type</th>
            <th>Signiture Serial Number</th>
            <th>User Id</th>
            <th>Status</th>
            <th>Flags</th>
            <th colSpan={2}>Actions</th>
          </tr>
        </thead>
        <tbody>
          {requests.map((request) => (
            <tr key={request.id}>
              <td>{request.id}</td>
              <td>{request.certificateType}</td>
              <td>{request.signitureSerialNumber}</td>
              <td>
                {request.user.id}
              </td>
              <td>{request.status}</td>
              <td>{request.flags}</td>
              {request.status === 0 && (
                <>
                  <td>
                    <button onClick={() => handleAccept(request.id)}>
                      Accept
                    </button>
                  </td>
                  <td>
                    <button onClick={() => handleDecline(request.id)}>
                      Decline
                    </button>
                  </td>
                </>
              )}
            </tr>
          ))}
        </tbody>
      </table>
      <div>
        <button
          disabled={currentPage === 1}
          onClick={() => handlePageChange(currentPage - 1)}
        >
          Previous
        </button>
        <button
          disabled={currentPage === totalPages}
          onClick={() => handlePageChange(currentPage + 1)}
        >
          Next
        </button>
        <span>
          Page {currentPage} of {totalPages}
        </span>
      </div>
    </div>
  );
}

export default Requests;
