import axios from 'axios';

export default class Api {
    constructor() {
        this.api_token = null;
        this.client = null;
        this.api_url = process.env.REACT_APP_API_ENDPOINT;
    }
    init = () => {
        let headers = {
            Accept: "application/json"
        };
        if (this.api_token) {
            headers.Authorization = `Bearer ${this.api_token
                }`;
        }
        this.client = axios.create({ baseURL: this.api_url, timeout: 31000, headers: headers });
        return this.client;
    };
    getRacq = () => {
        return this.init().get("/api/test/testovi");
    };
    addNewUser = (data) => {
        return this.init().post("/users", data);
    };
}