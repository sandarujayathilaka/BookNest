import axios from "axios";

import useUserStore from "../stores/auth";

const createAxiosInstance = (baseURL) => {
  const instance = axios.create({
    baseURL,
  });

  instance.interceptors.request.use(
    (config) => {
      const token = useUserStore.getState().token;
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  return instance;
};

const apiURL = import.meta.env.VITE_API_URI
  ? `${import.meta.env.VITE_API_URI}`
  : "http://localhost:5271/api";

const api = createAxiosInstance(apiURL);

export { api };
