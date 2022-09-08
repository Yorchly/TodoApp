import axios from "axios";

const debug = process.env.NODE_ENV !== "production";
const baseURL = debug ? "https://localhost:7161/api/" : "";
const api = axios.create({ baseURL });
export default api;
