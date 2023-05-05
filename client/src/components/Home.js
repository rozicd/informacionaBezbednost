import React from 'react';
import Certificates from './CertificatesList';
import Requests from './RequestList';
import { Outlet, useNavigate, Route, Routes } from 'react-router-dom';
import { logOut } from '../services/authService';
import Navbar from './Navbar';
import './Home.css';

function Home({ data }) {
  
  return (
    <div>
      <Navbar role = {data}/>
      <div className="home-container">
        <div className="home-main">
        <Outlet/>
          <h1 className="home-title">Welcome to My App!</h1>
          <div className="home-center"></div>
        </div>
      </div>
    </div>
  );
}

export default Home;