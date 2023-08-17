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

async function DownloadCert(serialNumber) {
    try {
        const response = await axios.get(`${API_BASE_URL}/download/${serialNumber}`, {
            responseType: 'blob',
            withCredentials: true,
        });

        const contentType = response.headers['content-type'];

        if (contentType === 'application/x-x509-ca-cert') {
            // Single certificate file
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `${serialNumber}.crt`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } else if (contentType === 'application/zip') {
            // Zip file containing certificate and key
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `${serialNumber}.zip`);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } else {
            // Handle unsupported file type or other error
            console.error('Unsupported file type:', contentType);
            // Handle error here
        }
    } catch (error) {
        console.error(error);
        // Handle error here
    }
}
async function AddCertRequest(certificateType,signatureSerialNumber,userId,flags,recaptcha){
    console.log(certificateType)
    console.log(signatureSerialNumber)
    console.log(userId)
    console.log(flags)
    certificateType = Number(certificateType);
    const response = await axios.post('http://localhost:8000/api/request', {
        certificateType,
        SignitureSerialNumber:signatureSerialNumber,
        userId,
        flags,
        "RecaptchaToken":recaptcha
    }, {      withCredentials: true,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    return response.data
}

export  {VerifyCert,VerifyCertString,RevokeCert,AddCertRequest,DownloadCert};

