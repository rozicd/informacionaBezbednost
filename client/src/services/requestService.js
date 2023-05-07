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

  async function acceptRequest(cerId) {
    const response = await axios.put(`${API_BASE_URL}/accept/${cerId}`, {
      withCredentials: true
    });
    return response;
  }

  async function declineRequest(cerId) {
    const response = await axios.put(`${API_BASE_URL}/decline/${cerId}`, {
      withCredentials: true
    });
    return response;
  }


  export {getAllRequest, getUsersRequest, acceptRequest, declineRequest}