import axios from 'axios';

const API_BASE_URL = 'http://localhost:8000/api/certificate';

async function getCertificates(page) {
    const response = await axios.get(`${API_BASE_URL}?page=${page}`, {
      withCredentials: true
    });
    return response.data;
  }

export default getCertificates;