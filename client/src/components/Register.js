import { Link, useNavigate } from 'react-router-dom';
import './Register.css';
import { useState } from 'react';
import {Register} from '../services/authService'
export default function RegisterPage() {
  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [password, setPassword] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
      const response = await Register(email, name, surname, password, phoneNumber);
      window.alert("SUCCESSFULY REGISTERED")
        navigate('/login')
        setErrorMessage('');
    } catch (error) {
      console.error(error);
      if(error.response.status == 409){
        setErrorMessage('Email already exists');
      }
      else if(error.response.status == 400){
        setErrorMessage('Not all fields are filled in')
      }
      else{
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

  return (
    <div className={'container'}>

      <main className='main'>
        <h1 className='title'>Register</h1>
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
          {errorMessage && <p className='error'>{errorMessage}</p>}
          <div className='register-page-center-button'>
              <button type="submit" className='register-page-button'>
              Register
              </button>
              <Link to='/login'>
                <button type='button' className='register-page-back-button'>
                    Back
                </button>
              </Link>
            </div>
            </form>
            </main>
        </div>)
        }