import React, { useState } from 'react';
import axios from 'axios';
import {VerifyCert} from "../services/certService";

function Verify() {
    const [file, setFile] = useState(null);

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
                console.log(response);
            } catch (error) {
                console.error(error);
            }
        };

        reader.readAsArrayBuffer(file);
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="file">Upload file:</label>
                    <input type="file" id="file" name="file" onChange={handleFileChange} />
                </div>
                <button type="submit" className="btn">Submit</button>
            </form>
        </div>
    );
}

export default Verify;
