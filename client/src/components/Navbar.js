import React from 'react';
import { useNavigate } from 'react-router-dom';
import { logOut } from '../services/authService';
import AdminMenu from './AdminMenu';

function Navbar({ role }) {
  const navigate = useNavigate();
  
  const handleSignOut = async () => {
    
    try {
        
      const data = await logOut();
      console.log(data);
      window.location.reload()      
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <nav>
      <div className="home-navbar">
        {role === 2 && <AdminMenu />}
        <button className='btn' onClick={handleSignOut}>Sign Out</button>        
      </div>
    </nav>
  );
}

export default Navbar;
