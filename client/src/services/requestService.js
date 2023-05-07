import axios from 'axios';

const API_BASE_URL = 'http://localhost:8000/api/request';

async function getAllRequest(page) {
    const response = await axios.get(`${API_BASE_URL}/all?page=${page}`, {
      withCredentials: true
    });
    return response.data;
  }

  async function getUsersRequest(id, page) {
    const response = await axios.get(`${API_BASE_URL}/${id}?page=${page}`, {
      withCredentials: true
    });
    return response.data;
  }

  export {getAllRequest, getUsersRequest}