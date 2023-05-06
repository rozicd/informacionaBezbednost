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

async function Register(email, name, surname, password, phoneNumber,verificationMethod) {
  console.log(verificationMethod);
  const response = await axios.post(`${API_BASE_URL}/register`, {
    email,
    name,
    surname,
    password,
    phoneNumber,
    verificationMethod
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


async function activateSms(code)
  {
    const response = await axios.post('http://localhost:8000/api/user/activateSms/'+code)
    return response.data
}

async function SendResetMail(email)
  {
    const response = await axios.post('http://localhost:8000/api/user/forgotpassword',{email:email})
    return response.data
}

async function resetPassword(id, token, newPassword)
 {
  const response = await axios.post(`http://localhost:8000/api/user/reset-password`, { Id:id,Token:token,NewPassword:newPassword });
  return response.data;
}

async function checkResetPasswordToken(token)
 {
  const response = await axios.post(`http://localhost:8000/api/user/verify-password-reset-token/${token}`);
  return response.data;
}
export { SignIn, checkCookieValidity, Register, activateAccount,logOut,activateSms,SendResetMail,resetPassword,checkResetPasswordToken};