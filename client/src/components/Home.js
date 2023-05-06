import React from 'react';
import {Outlet, Link, useNavigate} from 'react-router-dom';
import { useState, useEffect } from 'react';
import { logOut } from '../services/authService';
import './Home.css';
import Navbar from './Navbar';
import { checkCookieValidity, GetUserByEmail } from '../services/authService';

function Home() {
  const navigate = useNavigate();
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
      console.log("kurcina")
      console.log(email)
      const data = await GetUserByEmail(email);
      setUser(data);
      
      
    } catch (error) {
      
      console.error(error);
    }
  }

  useEffect(() => {
    checkAuthentication()
    email && getUser();

  }, [email]);

  return (
      <div className='container'>

        <main className='card size80'>
          <h1 className='card-header'>Home</h1>
          <div className="home-div">
            <Navbar role = {user.role}/>
            <Outlet/>
          </div>
          
        </main>
      </div>

  );
}

export default Home;
