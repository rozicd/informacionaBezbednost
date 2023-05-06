import React from 'react';
import {Link, useNavigate} from 'react-router-dom';
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
      <div className='container'>

        <main className='card size80'>
          <h1 className='card-header'>Home</h1>
          <div className="home-div">

            <button onClick={handleSignOut} className="btn logout-btn">Sign Out</button>

          </div>

        </main>
      </div>

  );
}

export default Home;
