import React, { useState } from 'react';
import { useNavigate
 } from 'react-router';
import { SendResetMail } from '../services/authService';
import ReCAPTCHA, { GoogleReCaptcha } from 'react-google-recaptcha-v3';


function ForgotPassword(props) {
  const [email, setEmail] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [recaptcha, setRecaptcha] = useState('');
  const [refreshRecaptcha, setRefreshRecaptcha] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setRefreshRecaptcha(r => !r);


    if(errorMessage == ''){
        try {
        const response = await SendResetMail(email,recaptcha);
        window.alert("An email has been sent with the password reset link");
        setErrorMessage('')
        navigate('/login');
        } catch (error) {
        if(error.response.status == 404){
            setErrorMessage('The user does not exist!');
            }
            else if(error.response.status == 400){
            setErrorMessage('Please fill out the form!')
            }
            else{
            setErrorMessage('An error occurred. Please try again later.');
            }
        }
    }
  };

  const handleBackClick = () => {
    navigate('/login');
  };
  const handleEmailChange = (event) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // regular expression for email format
    setEmail(event.target.value);
    if (emailRegex.test(email)) {
        setErrorMessage('')
    } else {
        setErrorMessage('Please enter a valid email address');
    }
  };

  const handleRecaptchaChange = (event) => {
    console.log(event)
    setRecaptcha(event);
  };
  const recaptchaComp = React.useMemo( () => <GoogleReCaptcha onVerify={handleRecaptchaChange} refreshReCaptcha={refreshRecaptcha} />, [refreshRecaptcha] );


  return (
    <div className='container'>
      <main className='card'>
        <h1 className='card-header'>Forgot Password</h1>
          <div className="padding-20">

          <form onSubmit={handleSubmit} className='form'>
          {recaptchaComp}

          <div className='fieldset'>
            <label htmlFor="email" className='label'>
              Email:
            </label>
            <input type="email" id="email" name="email" className='input' value={email} onChange={handleEmailChange} />
          </div>
          {errorMessage && <p className='error'>{errorMessage}</p>}
          <div className='buttons'>
            <button type='submit' className='btn'>
              Submit
            </button>
            <button type='button' className='btn' onClick={handleBackClick}>
              Back
            </button>
          </div>
        </form>
          </div>
      </main>
    </div>
  );
}

export default ForgotPassword;
