import Head from 'next/head';
import styles from '../styles/index.module.css';
import { useState } from 'react';

export default function LoginPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
      const response = await fetch('http://localhost:8000/api/user/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify({ username, password }),
      });
      if (response.ok) {
        // redirect to the dashboard or the protected page
        console.log('Login successful');
      } else {
        setErrorMessage('Invalid username or password');
      }
    } catch (error) {
      console.error(error);
      setErrorMessage('An error occurred while logging in');
    }
  };

  const handleUsernameChange = (event) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleRegisterClick = () => {
    window.location.href = '/register';
  };

  return (
    <div className={styles.container}>
      <Head>
        <title>Login</title>
        <meta name="description" content="login page" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main className={styles.main}>
        <h1 className={styles.title}>Login</h1>
        <form onSubmit={handleSubmit} className={styles.form}>
          <div className={styles.fieldset}>
            <label htmlFor="username" className={styles.label}>
              Username:
            </label>
            <input type="text" id="username" name="username" className={styles.input} value={username} onChange={handleUsernameChange} />
          </div>
          <div className={styles.fieldset}>
            <label htmlFor="password" className={styles.label}>
              Password:
            </label>
            <input type="password" id="password" name="password" className={styles.input} value={password} onChange={handlePasswordChange} />
          </div>
          {errorMessage && <p className={styles.error}>{errorMessage}</p>}
          <div className={styles.center}>
            <button type="submit" className={styles.button}>OK</button>
            <button type="button" className={styles.button} onClick={handleRegisterClick}>
              Register
            </button>
          </div>

        </form>
      </main>
    </div>
  );
}