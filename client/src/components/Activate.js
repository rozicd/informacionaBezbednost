import { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import parse from 'query-string';
import { useParams, useNavigate } from 'react-router-dom';
import { activateAccount } from '../services/authService';

export default function ActivateAccount() {
  const location = useLocation();
  const { id, token } = parse.parse(location.search);
  const [activated, setActivated] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (id && token) {
      try {
        activateAccount(id, token);
        setActivated(true);
        setTimeout(() => {
          navigate('/login');
        }, 2000);
        console.log("TRY");
      } catch (error) {
        window.alert(error);
        console.log("CATCH");
      }
    } else {
      navigate('/login');
    }
  }, []);

  if (activated) {
    return (
      <div className="activate-main">
        <h1>Account Activated!</h1>
        <p>Your account has been successfully activated.</p>
      </div>
    );
  }

  return (
    <div className="activate-main">
      <h1>Activate Your Account</h1>
      <p>Activating your account, please wait...</p>
    </div>
  );
}