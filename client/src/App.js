import { Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import Home from './components/Home';
import Activate from './components/Activate'
import Certificates from './components/CertificatesList';
import Requests from './components/RequestList';
import { useState, useEffect } from 'react';
import {checkCookieValidity} from './services/authService'

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [data, setData] = useState('')
    async function checkAuthentication() {
      try {
        const data = await checkCookieValidity();
        setIsAuthenticated(true)
        return data;
        
      } catch (error) {
        console.error(error);
        setIsAuthenticated(false);
      }
    }
    useEffect(() => {
      

      const rolePromise = checkAuthentication();
      rolePromise.then((role) => {
        setData(role);
      });
      
      
  }, []);

    return (
        <Routes>
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
                    isAuthenticated ? <Home data = {data}/> : <Navigate to="/login" />
                }
                
            >
              <Route path="certificates" element={<Certificates/>}/>
              <Route path="requests" element = {<Requests/>} />            
            </Route>
        </Routes>
    );
}

export default App;
