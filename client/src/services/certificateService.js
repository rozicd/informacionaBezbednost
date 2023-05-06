import axios from 'axios';

const API_BASE_URL = 'http://localhost:8000/api/certificate';

async function getCertificates() {
    const response = await axios.get(`${API_BASE_URL}`, {
      withCredentials: true
    });
    return response.data;
  }

export default getCertificates;