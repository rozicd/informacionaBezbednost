import React, { useState, useEffect } from 'react';
import getCertificates from '../services/certificateService';
import {RevokeCert} from "../services/certService";
import axios from 'axios';
import { Link, Outlet, useLocation } from 'react-router-dom';
import './CertificatesList.css';

function Certificates() {
  const [certificates, setCertificates] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [totalItems, setTotalItems] = useState(0);
  const [showOutlet, setShowOutlet] = useState(false);
  const location = useLocation();

  useEffect(() =>{
    if(location.pathname == '/home/certificates'){
      setShowOutlet(false);

    }
  },[location]);

  useEffect(() => {
    setShowOutlet(false);
    const fetchCertificates = async () => {
      try {
        const response = await getCertificates(currentPage);
        setCertificates(response.items);
        setTotalItems(response.totalItems);
      } catch (error) {
        console.error(error);
      }
    };
    fetchCertificates();
  }, [currentPage, pageSize]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const totalPages = Math.ceil(totalItems / pageSize);

  const handleButtonClick = async (serialNumber) => {
    // Do something with the serial number, like send it to a server
    await RevokeCert(serialNumber);
  };

  return (
    <div className='center-list'>
      {!showOutlet && (
      <><table className='table table-bordered'>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Issuer</th>
              <th>Expiration Date</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {certificates.map((certificate) => (
              <tr key={certificate.id}>
                <td>{certificate.id}</td>
                <td>{certificate.serialNumber}</td>
                <td>{certificate.issuer}</td>
                <td>{certificate.validTo}</td>
                <td>{certificate.status}</td>
                <td>
                  {certificate.status !== 2 && (
                      <button className="btn revoke-btn" onClick={() => handleButtonClick(certificate.serialNumber)}>Revoke</button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table><div>
            <button disabled={currentPage === 1} onClick={() => handlePageChange(currentPage - 1)}>
              Previous
            </button>
            <button disabled={currentPage === totalPages} onClick={() => handlePageChange(currentPage + 1)}>
              Next
            </button>
            <span>
              Page {currentPage} of {totalPages}
            </span>
          </div></>)}
      
      {showOutlet && <Outlet />}
      <Link to="create-request">
        <button className='btn btn-wide top-margin-button' onClick={() => setShowOutlet(!showOutlet)}>{showOutlet ? "Back" : "Create Certificate"}</button>
      </Link>
    </div>
  );
}

export default Certificates;
