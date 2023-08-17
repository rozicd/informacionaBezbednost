import axios from 'axios';
import { useHistory, useNavigate } from 'react-router-dom';

const api = axios.create({
});

axios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && (error.response.status === 403 || error.response.status === 401)) {
        console.log("CAOO")
      const navigate = useNavigate();
      navigate('/login');
    }
    return Promise.reject(error);
  }
);

export default api;