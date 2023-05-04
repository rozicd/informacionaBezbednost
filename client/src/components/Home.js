import React from 'react';
import { useNavigate } from 'react-router-dom';
import { logOut } from '../services/authService';
import './Home.css';

function Home() {
  const navigate = useNavigate();

  const  handleSignOut = async () => {
    try {
      const data = await logOut();
      console.log(data);
      window.location.reload()      
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <div>
      <nav>
        <div className="home-navbar">
          <h1>My App</h1>
          <button onClick={handleSignOut}>Sign Out</button>
        </div>
      </nav>
      <div className="home-container">
        <div className="home-main">
          <h1 className="home-title">Welcome to My App!</h1>
          <div className="home-center"></div>
        </div>
      </div>
    </div>
  );
}

export default Home;