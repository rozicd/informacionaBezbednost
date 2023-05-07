import React from 'react';

import { useLocation } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { checkCookieValidity, GetUserByEmail } from '../services/authService';
import { getAllRequest, getUsersRequest } from '../services/requestService';

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
    try{
      response = await getAllRequest(currentPage);
    }
    catch(error){

    }
    
  } else if (user.role === 1) {
    try{
      response = await getUsersRequest(user.id, currentPage);
    }
    catch(error){
      
    }
  }

  setRequests(response.items);
  setTotalItems(response.totalItems);

  }

  const handlePageChange = (page) => {
    
    setCurrentPage(page);
  };

  async function checkAuthentication() {
    try {
      const data = await checkCookieValidity();
      
      setEmail(data)
      
    } catch (error) {
      console.error(error);

    }
  }

  async function getUser() {
    try {
      console.log(email)
      const data = await GetUserByEmail(email);
      setUser(data);
      
      
    } catch (error) {
      
      console.error(error);
    }
  };

  
  useEffect(() =>{
    if(location.pathname === '/home/requests'){
      async function nesto (){
        await checkAuthentication()
        await getUser();
        await fetchRequests();
        console.log("currentPage")
        console.log(totalItems)
      }
      nesto()

    }
  },[location]);

  useEffect(() => {
    async function nesto (){
      await checkAuthentication()
      await getUser();
      await fetchRequests();
      console.log("currentPage")
      console.log(totalItems)
    }
    nesto()
    

  }, [currentPage]);

  const totalPages = Math.ceil(totalItems / pageSize);  

  return (
     <>
      <h1 className='cmp'>LISTA ZAHTEVAAAA BATOOO</h1>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Certificate Type</th>
            <th>Signiture Serial Number</th>
            <th>User</th>
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
              <td>{request.user.firstName} {request.user.lastName}</td>
              <td>{request.status}</td>
              <td>{request.flags}</td>
              {request.status === 0 && 
              <>
                <td>
                  <button >Accept</button>
                </td>
                <td>
                  <button >Decline</button>
                </td>
              </>}
            </tr>
          ))}
        </tbody>
      </table>
      <div>
        <button disabled={currentPage === 1} onClick={() => handlePageChange(currentPage - 1)}>
          Previous
        </button>
        <button disabled={currentPage === totalPages} onClick={() => handlePageChange(currentPage + 1)}>
          Next
        </button>
        <span>
          Page {currentPage} of {totalPages}
        </span>
      </div>
    </>
  );
}

export default Requests;
