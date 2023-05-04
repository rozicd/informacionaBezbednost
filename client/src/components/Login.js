import { Link, useNavigate } from 'react-router-dom';
import './Login.css';
import { useState } from 'react';
import {SignIn} from '../services/authService'

export default function LoginPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();
  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
        const data = await SignIn(username, password);
        console.log(data);
        window.location.reload()      
      } catch (error) {
        if(error.response.status == 401){
          setErrorMessage('Username or password is not correct!');
        }
        else if(error.response.status == 400){
          setErrorMessage('Not all fields are filled in!')
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


  return (
    <div className='container'>

      <main className='main'>
        <h1 className='title'>Login</h1>
        <form onSubmit={handleSubmit} className='form'>
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
          {errorMessage && <p className='error'>{errorMessage}</p>}
          <div className='center'>
            <button type="submit" className='button'>Login</button>
            <Link to="/register">
                <button type="button" className='register-button'>
                Register
                </button>
            </Link>
          </div>

        </form>
      </main>
    </div>
  );
}