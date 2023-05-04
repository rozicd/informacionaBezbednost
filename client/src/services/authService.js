import axios from 'axios';
import { useHistory } from 'react-router-dom';

const API_BASE_URL = 'http://localhost:8000/api/user';

async function SignIn(username, password) {
  const response = await axios.post(`${API_BASE_URL}/login`, {
    username,
    password
  }, {
    withCredentials: true
  });
  return response.data;
}

async function checkCookieValidity() {
  const response = await axios.get(`${API_BASE_URL}/authorized`, {
    withCredentials: true
  });
  return response.data;
}

async function Register(email, name, surname, password, phoneNumber) {
  const response = await axios.post(`${API_BASE_URL}/register`, {
    email,
    name,
    surname,
    password,
    phoneNumber
  }, {
    withCredentials: true
  });
  return response.data;
}

async function activateAccount(id, token) {

  const response = await axios.put(`http://localhost:8000/api/user/activate/${id}/${token}`);
    return response.data;
    
}

async function logOut()
  {
    const response = await axios.post('http://localhost:8000/api/user/logout',{},{
      withCredentials: true
    });
    return response.data
}

export { SignIn, checkCookieValidity, Register, activateAccount,logOut };