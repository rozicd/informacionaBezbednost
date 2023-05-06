import React from 'react';
import { Link, Navigate, Route, Routes, useNavigate, Outlet } from 'react-router-dom';
import { logOut } from '../services/authService';
import Verify from './VerifyCert';
import './Home.css';

function Home() {
  const navigate = useNavigate();

  const handleSignOut = async () => {
    try {
      const data = await logOut();
      console.log(data);
      window.location.reload();
    } catch (error) {
      console.error(error);
    }
  };

  return (

      <div className="container">
        <main className="card size80">
          <h1 className="card-header">Home</h1>
          <div className="home-div">
            <button onClick={handleSignOut} className="btn logout-btn">
              Sign Out
            </button>
            <button onClick={() => navigate('verify')} className="btn">
              Verify
            </button>
            <div>
              <Outlet />
            </div>
          </div>

        </main>

      </div>
  );
}

export default Home;
