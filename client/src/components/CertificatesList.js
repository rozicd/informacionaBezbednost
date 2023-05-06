import React, { useState, useEffect } from 'react';
import getCertificates from '../services/certificateService';
import axios from 'axios';

function Certificates() {
  const [certificates, setCertificates] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [totalItems, setTotalItems] = useState(0);

  useEffect(() => {
    const fetchCertificates = async () => {
    try{
        const response = await getCertificates(currentPage);
        setCertificates(response.items);
        setTotalItems(response.totalItems);
    }
      catch(error){

      }
    };
    fetchCertificates();
  }, [currentPage, pageSize]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const totalPages = Math.ceil(totalItems / pageSize);

  return (
    <div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Issuer</th>
            <th>Expiration Date</th>
          </tr>
        </thead>
        <tbody>
          {certificates.map((certificate) => (
            <tr key={certificate.id}>
              <td>{certificate.id}</td>
              <td>{certificate.serialNumber}</td>
              <td>{certificate.issuer}</td>
              <td>{certificate.validTo}</td>
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
    </div>
  );
}

export default Certificates;
