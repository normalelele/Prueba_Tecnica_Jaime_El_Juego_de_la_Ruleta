import axios, { AxiosInstance } from 'axios';
import { ApiBetRequest } from '../types/Game';

const BASE_URL = 'https://localhost:7183/api';

class ApiService {

  private static instance: AxiosInstance;

  private static get client(): AxiosInstance {
    if (!this.instance) {
      this.instance = axios.create({
        baseURL: BASE_URL,
        timeout: 10000,
        headers: { 'Content-Type': 'application/json' },
      });
    }
    
    return this.instance;
  }

  static async getRouletteResult() {
    const res = await this.client.get('/roulette/spin');
    return res.data;
  }

  static async calculatePrize(betRequest: ApiBetRequest) {
    const res = await this.client.post('/roulette/prize', betRequest);
    return res.data;
  }

  static async saveUserBalance(username: string, amount: number) {
    const body = { username, amount };
    const res = await this.client.post('/users/save', body);
    return res.data;
  }

  static async getUserBalance(username: string) {
    try {
      const res = await this.client.get(`/users/${encodeURIComponent(username)}`);
      return res.data;
    }
    catch (err: any) {
      if (err.response && err.response.status === 404) return null;
      throw err;
    }
  }

}

export default ApiService;
