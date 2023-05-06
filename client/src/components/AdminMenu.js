import React from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';

function AdminMenu() {
    const navigate = useNavigate();

  return (
    <>
    <div className="menu">
        <Link to="requests">
            <button className='btn'>Requests</button>
        </Link>
        <Link to="certificates">
            <button className='btn'>Certificates</button>
        </Link>
        
            <button onClick={() => navigate('verify')} className="btn">
              Verify
            </button>
    </div>
    </>
  );
}

export default AdminMenu;
