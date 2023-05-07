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

async function AddCertRequest(certificateType,signatureSerialNumber,userId,flags){
    console.log(certificateType)
    console.log(signatureSerialNumber)
    console.log(userId)
    console.log(flags)
    certificateType = Number(certificateType);
    const response = await axios.post('http://localhost:8000/api/request', {
        certificateType,
        SignitureSerialNumber:signatureSerialNumber,
        userId,
        flags
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
    return response.data
}
export {VerifyCert,AddCertRequest};
