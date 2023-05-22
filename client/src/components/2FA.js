import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './SMSVerification.css';
import {activateSms, ActivateTwoFactorCode, SendTwoFactorCode, SendTwoFactorCodeSMS} from '../services/authService'

export default function TwoFactorAuthentication() {
    const [code, setCode] = useState('');
    const [codeValid,setCodeValid] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [verificationMethod, setVerificationMethod] = useState(0);
    const navigate = useNavigate();

    const handleSubmit = async (event) => {
        event.preventDefault();
        if(codeValid){
            setErrorMessage('');
            try {
                const response = await ActivateTwoFactorCode(code);
                console.log(code);
                navigate('/home');
                window.location.reload();
            } catch (error) {
                if(error.response.status == 404){
                    setErrorMessage('The 2fa code is invalid!');
                }
                else if(error.response.status == 400){
                    setErrorMessage('Invalid or expired 2fa activation token')
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
    const sendCode = async (event) => {
        event.preventDefault();
        if(verificationMethod == 0) {
            SendTwoFactorCode();
        }
        if(verificationMethod == 1) {
            SendTwoFactorCodeSMS();
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
                                Verification Code:
                            </label>
                            <input type="text" id="code" name="code" className='input' value={code} onChange={handleCodeChange}/>
                        </div>
                        <div className="verification-method">
                            <input type="radio" className='radio-button' name="verificationMethod" value="0" checked={verificationMethod === 0} onChange={() => setVerificationMethod(0)} />
                            <label htmlFor="email" className='radio-label'>Email</label>

                            <input type="radio" className='radio-button' name="verificationMethod" value="1" checked={verificationMethod === 1} onChange={() => setVerificationMethod(1)} />
                            <label htmlFor="sms" className='radio-label'>SMS</label>
                        </div>
                        {errorMessage && <p className='error'>{errorMessage}</p>}
                        <div className='buttons'>
                            <button type='button' className='btn' onClick={sendCode}>
                                Send Code
                            </button>
                            <button type='submit' className='btn'>
                                Submit
                            </button>
                            <button type='button' className='btn' onClick={handleBackClick}>
                                Logout
                            </button>
                        </div>
                    </form>
                </div>
            </main>
        </div>
    );
}
