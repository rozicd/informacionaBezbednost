import axios from 'axios';

const API_BASE_URL = 'http://localhost:8000/api/certificate';

async function VerifyCert(data) {
    const response = await axios.post('http://localhost:8000/api/certificate/validate', data, {
        headers: {
            'Content-Type': 'application/x-x509-ca-cert'
        }
    });
    return response.data
}

export default VerifyCert;
