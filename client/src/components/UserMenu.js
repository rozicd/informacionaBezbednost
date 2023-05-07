import React from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';

function UserMenu() {
    const navigate = useNavigate();

    return (
        <>
            <div className="menu">
                <Link to="verify">
                    <button className='btn'>Verify</button>
                </Link>


            </div>
        </>
    );
}

export default UserMenu;
