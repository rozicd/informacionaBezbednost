import { useRouter } from 'next/router';
import { useEffect, useState } from 'react';
import styles from '../styles/activate.module.css';


export default function ActivateAccount() {
  const router = useRouter();
  const [id, setId] = useState(null);
  const [token, setToken] = useState(null);

  useEffect(() => {
    setId(router.query.id);
    setToken(router.query.token);
  }, [router.query]);

  useEffect(() => {
    if (id && token) {
      activateAccount();
    }
  }, [id, token]);

  async function activateAccount() {
    const response = await fetch(`http://localhost:8000/api/user/activate/${id}/${token}`, {
      method: 'PUT',
    });
    if (response.ok) {
      // Account activated successfully
      window.alert("bonus nispeasd")
      router.push('');
    } else {
      // Error occurred during activation
      const message = await response.text();
      alert(`Failed to activate account: ${message}`);
    }
  }

  return (
    <div className={styles.main}>
      <h1>Activate Your Account</h1>
      <p>Activating your account, please wait...</p>
    </div>
  );
}
