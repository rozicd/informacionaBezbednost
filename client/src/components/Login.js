import { Link, useNavigate } from 'react-router-dom';
import './Login.css';
import React, { useState } from 'react';
import {SignIn, googleLogin} from '../services/authService'
import ReCAPTCHA, { GoogleReCaptcha } from 'react-google-recaptcha-v3';

export default function LoginPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [recaptcha, setRecaptcha] = useState('');
  const [refreshRecaptcha, setRefreshRecaptcha] = useState(false);
  
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();
  const handleSubmit = async (event) => {
    event.preventDefault();
    setRefreshRecaptcha(r => !r);
    try {
        const data = await SignIn(username, password,recaptcha);
        console.log(data);
      if (data.redirectUrl) {
        window.location.href = data.redirectUrl;
      }
      else {
        window.location.reload()
      }
      } catch (error) {
        if(error.response.status == 401){
          setErrorMessage('Username or password is not correct!');
        }
        else if(error.response.status == 400){
          setErrorMessage("Not all fields are filled in!")
        }
        else{
        setErrorMessage('An error occurred. Please try again later.');
        }
      }
};

  const handleUsernameChange = (event) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleRecaptchaChange = (event) => {
    console.log(event)
    setRecaptcha(event);
  };

  const handleGoogleClick =  (event) => {
    window.location.href = "http://localhost:8000/api/user/google-login";


  };
  const recaptchaComp = React.useMemo( () => <GoogleReCaptcha onVerify={handleRecaptchaChange} refreshReCaptcha={refreshRecaptcha} />, [refreshRecaptcha] );

  return (
    <div className='container'>

      <main className='card'>
        <h1 className='card-header'>Login</h1>
        <div className="padding-20">
        <form onSubmit={handleSubmit} className='form'>
          {recaptchaComp}
          <div className='fieldset'>
            <label htmlFor="username" className='label'>
              Username:
            </label>
            <input type="text" id="username" name="username" className={'input'} value={username} onChange={handleUsernameChange} />
          </div>
          <div className='fieldset'>
            <label htmlFor="password" className='label'>
              Password:
            </label>
            <input type="password" id="password" name="password" className='input' value={password} onChange={handlePasswordChange} />
          </div>
          <div className="links">
            <Link to="/forgot-password" className="forgot-password-link">Forgot password?</Link>
            <Link to="/verify-sms" className="verify-sms-link">Verify with SMS</Link>
          </div>
          {errorMessage && <p className='error'>{errorMessage}</p>}
          <div className='center'>
            <button type="submit" className='btn'>Login</button>
            <Link to="/register">
                <button type="button" className='btn'>
                Register
                </button>
            </Link>
            <button type="button" className='btn' onClick={handleGoogleClick}>
                Google
              </button>
          </div>

        </form>
        </div>
      </main>
    </div>
  );
}
