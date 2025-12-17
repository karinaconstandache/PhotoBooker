import type { PhotographerDto, UpdatePhotographerDto } from '../types/photographer';

const API_BASE_URL = '/api';

class PhotographerService {
  private getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem('token');
    return {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
    };
  }

  async getAllPhotographers(): Promise<PhotographerDto[]> {
    const response = await fetch(`${API_BASE_URL}/photographers`, {
      method: 'GET',
      headers: this.getAuthHeaders(),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to fetch photographers');
    }

    return response.json();
  }

  async getPhotographerById(id: number): Promise<PhotographerDto> {
    const response = await fetch(`${API_BASE_URL}/photographers/${id}`, {
      method: 'GET',
      headers: this.getAuthHeaders(),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to fetch photographer');
    }

    return response.json();
  }

  async updatePhotographerProfile(
    updateDto: UpdatePhotographerDto
  ): Promise<PhotographerDto> {
    const response = await fetch(`${API_BASE_URL}/photographers/profile`, {
      method: 'PUT',
      headers: this.getAuthHeaders(),
      body: JSON.stringify(updateDto),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to update photographer profile');
    }

    return response.json();
  }

  async getPhotographerPortfolios(photographerId: number): Promise<any[]> {
    const response = await fetch(`${API_BASE_URL}/portfolios/photographer/${photographerId}`, {
      method: 'GET',
      headers: this.getAuthHeaders(),
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to fetch photographer portfolios');
    }

    return response.json();
  }
}

export default new PhotographerService();
