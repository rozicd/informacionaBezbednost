import axios from 'axios';
import { useHistory } from 'react-router-dom';

const API_BASE_URL = 'http://localhost:8000/api/user';

async function SignIn(username, password,recaptcha) {
  const response = await axios.post(`${API_BASE_URL}/login`, {
    username,
    password,
    "RecaptchaToken":recaptcha
  }, {
    withCredentials: true
  });

  return response.data;
}

async function GetUserByEmail(email) {
  const response = await axios.post(`${API_BASE_URL}/email`, {
    email 
  });
  return response.data;
}


async function checkCookieValidity() {
  const response = await axios.get(`${API_BASE_URL}/authorized`, {
    withCredentials: true
  });
  return response.data;
}

async function Register(email, name, surname, password, phoneNumber,verificationMethod,recaptcha) {
  console.log(verificationMethod);
  const response = await axios.post(`${API_BASE_URL}/register`, {
    email,
    name,
    surname,
    password,
    phoneNumber,
    verificationMethod,
    "RecaptchaToken":recaptcha
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

async function googleCallback(code){
  const response = await axios.post('http://localhost:8000/api/user/google-callback?code='+code)
  return response.data
}

 async function googleLogin()
  {
    const response = await axios.get('http://localhost:8000/api/user/google-login')
    return response.data
  }




async function SendTwoFactorCode()
{
  const response = await axios.post('http://localhost:8000/api/user/2fa/email',{},{
    withCredentials: true
  });
  return response.data
}

async function SendTwoFactorCodeSMS()
{
  const response = await axios.post('http://localhost:8000/api/user/2fa/sms',{},{
    withCredentials: true
  });
  return response.data
}


async function ActivateTwoFactorCode(code)
{
  const response = await axios.post('http://localhost:8000/api/user/2fa/activate/'+code,{},{
    withCredentials: true
  });
  return response.data
}

async function SendResetMail(email,recaptcha)
  {
    const response = await axios.post('http://localhost:8000/api/user/forgotpassword',{email:email,"RecaptchaToken":recaptcha})
    return response.data
}

async function resetPassword(id, token, newPassword,recaptcha)
 {
  const response = await axios.post(`http://localhost:8000/api/user/reset-password`, { Id:id,Token:token,NewPassword:newPassword,"RecaptchaToken":recaptcha});
  return response.data;
}

async function checkResetPasswordToken(token)
 {
  const response = await axios.post(`http://localhost:8000/api/user/verify-password-reset-token/${token}`);
  return response.data;
}
export {googleCallback, googleLogin,SendTwoFactorCodeSMS,ActivateTwoFactorCode,SignIn, checkCookieValidity, Register, activateAccount,logOut,activateSms,SendResetMail,resetPassword,checkResetPasswordToken, GetUserByEmail,SendTwoFactorCode};
