import Axios from "axios";

const api = Axios.create({
  baseURL: "http://localhost:7093/",
});

export default api;
