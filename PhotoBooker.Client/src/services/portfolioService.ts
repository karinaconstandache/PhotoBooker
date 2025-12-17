import type {
  PortfolioDto,
  CreatePortfolioDto,
  UpdatePortfolioDto,
} from '../types/photographer';

const API_BASE_URL = '/api';

class PortfolioService {
  private getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem('token');
    return {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
    };
  }

  async getAllPortfolios(): Promise<PortfolioDto[]> {
    const response = await fetch(`${API_BASE_URL}/portfolios`, {
      method: 'GET',
      headers: this.getAuthHeaders(),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to fetch portfolios');
    }

    return response.json();
  }

  async getMyPortfolios(): Promise<PortfolioDto[]> {
    const response = await fetch(`${API_BASE_URL}/portfolios/my-portfolios`, {
      method: 'GET',
      headers: this.getAuthHeaders(),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to fetch portfolios');
    }

    return response.json();
  }

  async getPortfolioById(id: number): Promise<PortfolioDto> {
    const response = await fetch(`${API_BASE_URL}/portfolios/${id}`, {
      method: 'GET',
      headers: this.getAuthHeaders(),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to fetch portfolio');
    }

    return response.json();
  }

  async createPortfolio(createDto: CreatePortfolioDto): Promise<PortfolioDto> {
    const response = await fetch(`${API_BASE_URL}/portfolios`, {
      method: 'POST',
      headers: this.getAuthHeaders(),
      body: JSON.stringify(createDto),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to create portfolio');
    }

    return response.json();
  }

  async updatePortfolio(
    id: number,
    updateDto: UpdatePortfolioDto
  ): Promise<PortfolioDto> {
    const response = await fetch(`${API_BASE_URL}/portfolios/${id}`, {
      method: 'PUT',
      headers: this.getAuthHeaders(),
      body: JSON.stringify(updateDto),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to update portfolio');
    }

    return response.json();
  }

  async deletePortfolio(id: number): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/portfolios/${id}`, {
      method: 'DELETE',
      headers: this.getAuthHeaders(),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to delete portfolio');
    }
  }
}

export default new PortfolioService();
