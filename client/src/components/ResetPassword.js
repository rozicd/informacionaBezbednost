import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { checkCookieValidity, checkResetPasswordToken, resetPassword } from '../services/authService';
import './ResetPassword.css';

const ResetPassword = React.memo(({ id, token }) => {
  const [isValidToken, setIsValidToken] = useState(false);
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const isMounted = useRef(false);

  const navigate = useNavigate();

  useEffect(() => {
  async function checkResetToken() {
    try {
      const searchParams = new URLSearchParams(window.location.search);
      const id = searchParams.get('id');
      const token = searchParams.get('token');
      if (!id || !token) {
        alert('Error: Invalid reset link');
        navigate('/login');
        return;
      }
      const response = await checkResetPasswordToken(token);
      setIsValidToken(true);
    } catch (error) {
        alert('Error: Invalid reset link');
        navigate('/login');
    }
  }

  if (window.location.search) {
    checkResetToken();
  }
}, []);

  const handleNewPasswordChange = (event) => {
    setNewPassword(event.target.value);
  };

  const handleConfirmPasswordChange = (event) => {
    setConfirmPassword(event.target.value);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (newPassword !== confirmPassword) {
      setErrorMessage('New password and confirm password do not match');
      return;
    }

    try {
        const searchParams = new URLSearchParams(window.location.search);
        const id = searchParams.get('id');
        const token = searchParams.get('token');
      await resetPassword(id, token, newPassword);
      alert('Password has been successfully reset');
      navigate('/login');
    } catch (error) {
      console.log(error);
      setErrorMessage('Failed to reset password');
    }
  };

  return (
    <div className="reset-password-container">
        <div className="reset-password-main">
            <h2 className="reset-password-title">Reset Password</h2>
            <div className="padding-20">

            <form className="reset-password-form" onSubmit={handleSubmit}>
                <div className="reset-password-fieldset">
                <label className="reset-password-label" htmlFor="new-password">New Password:</label>
                <input
                    className="reset-password-input"
                    type="password"
                    id="new-password"
                    value={newPassword}
                    onChange={handleNewPasswordChange}
                />
                </div>
                <div className="reset-password-fieldset">
                <label className="reset-password-label" htmlFor="confirm-password">Confirm Password:</label>
                <input
                    className="reset-password-input"
                    type="password"
                    id="confirm-password"
                    value={confirmPassword}
                    onChange={handleConfirmPasswordChange}
                />
                </div>
                {errorMessage && <p className="reset-password-error">{errorMessage}</p>}
                <div className="reset-password-buttons">
                <button className="reset-password-button" type="submit">Reset</button>
                <button className="reset-password-cancel-button" onClick={() => navigate('/login')}>Cancel</button>
                </div>
            </form>
            </div>
      </div>
    </div>
  );
});

export default ResetPassword;
