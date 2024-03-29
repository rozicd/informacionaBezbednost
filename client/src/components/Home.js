import React from 'react';
import {Outlet, Link, useNavigate} from 'react-router-dom';
import { useState, useEffect } from 'react';
import { logOut } from '../services/authService';
import Verify from './VerifyCert';
import './Home.css';
import Navbar from './Navbar';
import { checkCookieValidity, GetUserByEmail } from '../services/authService';

function Home() {
  
  const [user, setUser] = useState('');
  const [email, setEmail] = useState('');

  async function checkAuthentication() {
    try {
      const data = await checkCookieValidity();
      
      setEmail(data)
      
    } catch (error) {
      console.error(error);

    }
  }

  async function getUser() {
    try {
      console.log(email)
      const data = await GetUserByEmail(email);
      setUser(data);
      
      
    } catch (error) {
      
      console.error(error);
    }
  };

  useEffect(() => {
    checkAuthentication()
    email && getUser();

  }, [email]);

  return (

      <div className="container">
        <main className="card size80">
          <h1 className="card-header card">Home</h1>
          <div className="home-div">
            <Navbar role = {user.role}/>

                <div className='home-content'>
              <div className="card out-card-div">
              <Outlet />
              </div>

            </div>
          </div>
          
        </main>

      </div>
  );
}

export default Home;
