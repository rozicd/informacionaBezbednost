import React from 'react';
import { Link, Outlet } from 'react-router-dom';

function AdminMenu() {


  return (
    <>
    <div className="menu">
        <Link to="requests">
            <button>Requests</button>
        </Link>
        <Link to="certificates">
            <button>Certificates</button>
        </Link>
    </div>
    </>
  );
}

export default AdminMenu;
