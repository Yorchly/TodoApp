import axios from "axios";

//const debug = process.env.NODE_ENV !== "production";
const baseURL = "http://localhost:5000/api/";
const api = axios.create({ baseURL });
export default api;
