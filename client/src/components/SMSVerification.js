import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './SMSVerification.css';
import {activateSms} from '../services/authService'

export default function SMSVerificationPage() {
  const [code, setCode] = useState('');
  const [codeValid,setCodeValid] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    if(codeValid){
        setErrorMessage('');
        try {
        const response = await activateSms(code);
        console.log(code);
        window.alert("Account successfuly activated");
        navigate('/login');
        } catch (error) {
        if(error.response.status == 404){
            setErrorMessage('The verification code is invalid!');
            }
            else if(error.response.status == 400){
            setErrorMessage('Invalid or expired sms activation token')
            }
            else{
            setErrorMessage('An error occurred. Please try again later.');
            }
        }
    }
    else{
        setErrorMessage("Code is invalid!")
    }
  };

  const handleCodeChange = (event) => {
    const codeInput = event.target.value;
    setCode(codeInput);
    if (/^\d{6}$/.test(codeInput)) {
        setErrorMessage("")
      setCodeValid(true);
    } else {
        setErrorMessage("Code is invalid!")
      setCodeValid(false);
    }
  };

  const handleBackClick = () => {
    navigate('/login');
  };

  return (
    <div className='container'>
      <main className='card'>
        <h1 className='card-header'>SMS Verification</h1>
          <div className="padding-20">

          <form onSubmit={handleSubmit} className='form'>
          <div className='fieldset'>
            <label htmlFor="code" className='label'>
              SMS code:
            </label>
            <input type="text" id="code" name="code" className='input' value={code} onChange={handleCodeChange} />
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
