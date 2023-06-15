import React, { useState } from 'react';
import axios from 'axios';

import {VerifyCert,VerifyCertString} from "../services/certService";

function Verify() {
    const [file, setFile] = useState(null);
    const [verificationStatus, setVerificationStatus] = useState(null);
    const [inputValue, setInputValue] = useState('');

    const handleFileChange = (e) => {
        setFile(e.target.files[0]);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const reader = new FileReader();

        reader.onload = async (e) => {
            const data = new Uint8Array(e.target.result);
            try {
                const response = await VerifyCert(data)
                setVerificationStatus(response ? 'valid' : 'invalid');
            } catch (error) {
                console.error(error.response.status == 400)
                {
                    setVerificationStatus("Unsupported file type")
                };
            }
        };

        reader.readAsArrayBuffer(file);
    };

    const handleInputSubmit = async (e) => {
        e.preventDefault();
        // Call the desired function with the input value
        const response = await VerifyCertString(inputValue);
        setVerificationStatus(response ? 'valid' : 'invalid');
        console.log(response);
    };

    const handleInputChange = (e) => {
        setInputValue(e.target.value);
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="file">Upload file:</label>
                    <input type="file" id="file" name="file" onChange={handleFileChange} />
                </div>
                <button type="submit" className="btn">Verify</button>
            </form>
            <form onSubmit={handleInputSubmit}>
                <div>
                    <label htmlFor="inputField">Input field:</label>
                    <input type="text" id="inputField" name="inputField" value={inputValue} onChange={handleInputChange} />
                </div>
                <button type="submit" className="btn">Verify</button>
            </form>
            {verificationStatus && <p style={{ color: verificationStatus === 'valid' ? 'green' : 'red' }}>{verificationStatus}</p>}

        </div>
    );
}

export default Verify;
