import { Link, useNavigate } from 'react-router-dom';
import './Register.css';
import React, { useState } from 'react';
import {Register} from '../services/authService'
import ReCAPTCHA, { GoogleReCaptcha } from 'react-google-recaptcha-v3';

export default function RegisterPage() {
  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [password, setPassword] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [verificationMethod, setVerificationMethod] = useState(0);
  const [recaptcha, setRecaptcha] = useState('');
  const [refreshRecaptcha, setRefreshRecaptcha] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setRefreshRecaptcha(r => !r);
    try {
      const response = await Register(email, name, surname, password, phoneNumber, verificationMethod, recaptcha);
      window.alert("SUCCESSFULLY REGISTERED");
      navigate('/login');
      setErrorMessage('');
    } catch (error) {
      console.error(error);
      if (error.response.status === 409) {
        setErrorMessage('Email already exists');
      } else if (error.response.status === 400) {
        if (error.response.data.errors) {
          const fieldErrors = error.response.data.errors;
          const errorMessages = Object.values(fieldErrors).flat();
          setErrorMessage(errorMessages[0]);
        } else {
          setErrorMessage('Not all fields are filled in');
        }
      } else {
        setErrorMessage('An error occurred. Please try again later.');
      }
    }
  };

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handleNameChange = (event) => {
    setName(event.target.value);
  };

  const handleSurnameChange = (event) => {
    setSurname(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handlePhoneNumberChange = (event) => {
    setPhoneNumber(event.target.value);
  };

  const handleVerificationMethodChange = (event) => {
    setVerificationMethod(Number(event.target.value));
  };

  const handleRecaptchaChange = (event) => {
    console.log(event)
    setRecaptcha(event);
  };
  const recaptchaComp = React.useMemo( () => <GoogleReCaptcha onVerify={handleRecaptchaChange} refreshReCaptcha={refreshRecaptcha} />, [refreshRecaptcha] );

  return (
    <div className={'container'}>

      <main className='card'>
        <h1 className='card-header'>Register</h1>
        <div className="padding-20">

        <form onSubmit={handleSubmit} className='form'>
          <div className='fieldset'>
            <label htmlFor="email" className='label'>
              Email:
            </label>
            <input type="email" id="email" name="email" className='input' value={email} onChange={handleEmailChange} />
          </div>
          <div className='fieldset'>
            <label htmlFor="name" className='label'>
              Name:
            </label>
            <input type="text" id="name" name="name" className='input' value={name} onChange={handleNameChange} />
          </div>
          <div className='fieldset'>
            <label htmlFor="phoneNumber" className='label'>
              Phone:
            </label>
            <input type="tel" id="phoneNumber" name="phoneNumber" className='input'value={phoneNumber} onChange={handlePhoneNumberChange} />
          </div>
          <div className='fieldset'>
            <label htmlFor="surname" className='label'>
              Surname:
            </label>
            <input type="text" id="surname" name="surname" className='input' value={surname} onChange={handleSurnameChange} />
          </div>
          <div className='fieldset'>
            <label htmlFor="password" className='label'>
              Password:
            </label>
            <input type="password" id="password" name="password" className='input' value={password} onChange={handlePasswordChange} />
          </div>
          <label htmlFor="verification-method" className='verification-label'>
              Verification method:
            </label>
          <div className="verification-method">
          <input type="radio" className='radio-button' name="verificationMethod" value="0" checked={verificationMethod === 0} onChange={() => setVerificationMethod(0)} />
            <label htmlFor="email" className='radio-label'>Email</label>

            <input type="radio" className='radio-button' name="verificationMethod" value="1" checked={verificationMethod === 1} onChange={() => setVerificationMethod(1)} />
            <label htmlFor="sms" className='radio-label'>SMS</label>
          </div>
          {errorMessage && <p className='error'>{errorMessage}</p>}
          {recaptchaComp}
          <div className='register-page-center-button'>
              <button type="submit" className='btn'>
              Register
              </button>
              <Link to='/login'>
                <button type='button' className='btn'>
                    Back
                </button>
              </Link>
            </div>
            </form>
        </div>
            </main>
        </div>)
        }
