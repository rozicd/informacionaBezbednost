import { Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import Home from './components/Home';
import Activate from './components/Activate';
import SMSVerification from './components/SMSVerification';
import ForgotPassword from './components/ForgotPassword';

import { useState, useEffect } from 'react';
import {checkCookieValidity} from './services/authService'
import ResetPassword from './components/ResetPassword';
import VerifyCert from "./components/VerifyCert";

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    async function checkAuthentication() {
      try {
        const data = await checkCookieValidity();
        setIsAuthenticated(true)
      } catch (error) {
        console.error(error);
        setIsAuthenticated(false);
      }
    }
    useEffect(() => {
      

      checkAuthentication();
  }, []);

    return (
        <Routes>
          <Route
                path="/"
                element={
                    isAuthenticated ? <Navigate to="/home" /> : <Navigate to="/login" />
                }
            />
            <Route
                path="/login"
                element={
                    isAuthenticated ? <Navigate to="/home" /> : <Login />
                }
            />
            <Route
                path="/register"
                element={
                  isAuthenticated ? <Navigate to="/home" /> : <Register />
                }
            />
             <Route
                path="/activate"
                element={
                  <Activate  />
                }
            />
            <Route
                path="/home"
                element={
                    isAuthenticated ? <Home /> : <Navigate to="/login" />
                }
            >
                <Route path="verify" element={<VerifyCert/>}/>

            </Route>
            <Route
                path="/verify-sms"
                element={
                    <SMSVerification/>
                }
            />
            <Route
                path="/forgot-password"
                element={
                    <ForgotPassword/>
                }
            />
            <Route
                path="/reset-password"
                element={
                    <ResetPassword/>
                }
            />
        </Routes>
    );
}

export default App;
