import axios from 'axios';

const API_BASE_URL = 'http://localhost:8000/api/certificate';

async function VerifyCert(data) {
    const response = await axios.post('http://localhost:8000/api/certificate/validate', data, {
        withCredentials: true,
        headers: {
            'Content-Type': 'application/x-x509-ca-cert'
        }
    });
    return response.data
}

async function VerifyCertString(serialNumber) {
    const response = await axios.get('http://localhost:8000/api/certificate/validate/'+serialNumber, {
        withCredentials: true,

    });
    return response.data
}
async function RevokeCert(serialNumber) {
    const response = await axios.delete('http://localhost:8000/api/certificate/revoke/'+serialNumber, {
        withCredentials: true,

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

export  {VerifyCert,VerifyCertString,RevokeCert,AddCertRequest};

