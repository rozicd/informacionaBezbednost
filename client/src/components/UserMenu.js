import React from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';

function UserMenu() {
    const navigate = useNavigate();

  return (
    <>
    <div className="menu">
        <Link to="requests">
            <button className='btn menu-btn'>Requests</button>
        </Link>
        <Link to="certificates">
            <button className='btn menu-btn'>Certificates</button>
        </Link>
        <Link to="verify">
            <button className='btn menu-btn'>Verify</button>
        </Link>

    </div>
    </>
  );
}

export default UserMenu;