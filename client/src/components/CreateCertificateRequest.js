import React, { useState, useEffect } from 'react';
import { AddCertRequest } from '../services/certService';
import { GetUserByEmail, checkCookieValidity } from '../services/authService';
import ReCAPTCHA, { GoogleReCaptcha } from 'react-google-recaptcha-v3';
import './CreateCertificateRequest.css';

function CreateCertificateRequest() {
  const [certificateType, setCertificateType] = useState(1);
  const [signatureSerialNumber, setSignatureSerialNumber] = useState('');
  const [flags, setFlags] = useState('');

  const [user, setUser] = useState({});
  const [email, setEmail] = useState('');
  const [recaptcha, setRecaptcha] = useState('');
  const [refreshRecaptcha, setRefreshRecaptcha] = useState(false);

  const [errorMessage, setErrorMessage] = useState('');

  async function checkAuthentication() {
    try {
      const data = await checkCookieValidity();
      setEmail(data);
    } catch (error) {
      console.error(error);
    }
  }

  async function getUser() {
    try {
      const data = await GetUserByEmail(email);
      setUser(data);
    } catch (error) {
      console.error(error);
    }
  }

  useEffect(() => {
    checkAuthentication();
    email && getUser();
  }, [email]);

  const handleSubmit = async (event) => {
    event.preventDefault();
    setRefreshRecaptcha(r => !r);

    try {
      const response = await AddCertRequest(
        certificateType,
        signatureSerialNumber,
        user.id,
        flags,
        recaptcha
      );


      alert('Request added successfully!');
    } catch (error) {
      console.error(error);
      if(!error.response.data.errors){
        setErrorMessage(error.response.data);
      }
      else{
        const fieldErrors = error.response.data.errors;
        const errorMessages = Object.values(fieldErrors).flat();
        setErrorMessage(errorMessages[0]);      }
    }
  };

  const handleRecaptchaChange = (event) => {
    setRecaptcha(event);
  };
  const recaptchaComp = React.useMemo( () => <GoogleReCaptcha onVerify={handleRecaptchaChange} refreshReCaptcha={refreshRecaptcha} />, [refreshRecaptcha] );


  return (
    <form onSubmit={handleSubmit} className='center-form'>
      {recaptchaComp}
      <div className ='fieldset'>
        <label htmlFor="signatureSerialNumber" className='cert-create-label'>Signature Serial Number</label>
        <input type="text" id="signatureSerialNumber" className='cert-create-input' value={signatureSerialNumber} onChange={(e) => setSignatureSerialNumber(e.target.value)} />
      </div>
      <div className='fieldset'>
        <label htmlFor="flags" className='cert-create-label'>Flags</label>
        <input type="text" id="flags" className='cert-create-input' value={flags} onChange={(e) => setFlags(e.target.value)} />
      </div>
      <div  className ='fieldset'>
        <label htmlFor="certificateType" className='cert-create-label'>Certificate Type</label>
        <div className='center-option-cert-create'>
            <select id="certificateType" value={certificateType} className='bootstrap-select cert-create-select' onChange={(e) => setCertificateType(e.target.value)}>
            {user.role === 2 && <option value="0">Root</option>}
            <option value="1">Intermediate</option>
            <option value="2">End</option>
            </select>
        </div>
      </div>
      {errorMessage && <div className="error-message">{errorMessage}</div>}

      <div className='cert-create-buttons'>
        <button type="submit" className='btn btn-wide'>Add Certificate</button>
      </div>
    </form>
  );
}

export default CreateCertificateRequest;