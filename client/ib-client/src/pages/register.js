import Head from 'next/head';
import styles from '../styles/register.module.css';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { useState } from 'react';

export default function RegisterPage() {
    const router = useRouter();
  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [password, setPassword] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  const handleSubmit = async (event) => {
    
    event.preventDefault();
    
    try {
      const response = await fetch('http://localhost:8000/api/user/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify({ email, name, surname, password, phoneNumber }),
      });
      if (response.ok) {
        // redirect to the dashboard or the protected page
        router.push('/');
        window.alert("Registration successful");
        console.log('Registration successful');
      } else if(response.status == 400){
        setErrorMessage('User with this email already exists!');
      }
    } catch (error) {
      console.error(error);
      setErrorMessage('An error occurred while registering');
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
    <div className={styles.container}>
      <Head>
        <title>Register</title>
        <meta name="description" content="register page" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main className={styles.main}>
        <h1 className={styles.title}>Register</h1>
        <form onSubmit={handleSubmit} className={styles.form}>
          <div className={styles.fieldset}>
            <label htmlFor="email" className={styles.label}>
              Email:
            </label>
            <input type="email" id="email" name="email" className={styles.input} value={email} onChange={handleEmailChange} />
          </div>
          <div className={styles.fieldset}>
            <label htmlFor="name" className={styles.label}>
              Name:
            </label>
            <input type="text" id="name" name="name" className={styles.input} value={name} onChange={handleNameChange} />
          </div>
          <div className={styles.fieldset}>
            <label htmlFor="surname" className={styles.label}>
              Surname:
            </label>
            <input type="text" id="surname" name="surname" className={styles.input} value={surname} onChange={handleSurnameChange} />
          </div>
          <div className={styles.fieldset}>
            <label htmlFor="password" className={styles.label}>
              Password:
            </label>
            <input type="password" id="password" name="password" className={styles.input} value={password} onChange={handlePasswordChange} />
          </div>
          <div className={styles.fieldset}>
            <label htmlFor="phoneNumber" className={styles.label}>
              Phone number:
            </label>
            <input type="tel" id="phoneNumber" name="phoneNumber" className={styles.input} value={phoneNumber} onChange={handlePhoneNumberChange} />
          </div>
          {errorMessage && <p className={styles.error}>{errorMessage}</p>}
            <button type="submit" className={styles.button}>
            Register
            </button>
            </form>
            </main>
        </div>)
        }