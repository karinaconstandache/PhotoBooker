export interface PhotographerDto {
  id: number;
  username: string;
  firstName: string;
  lastName: string;
  bio?: string;
  createdAt: string;
}

export interface UpdatePhotographerDto {
  bio?: string;
}

export interface PortfolioDto {
  id: number;
  title: string;
  description: string;
  createdDate: string;
  photographerId: number;
  photographerName: string;
}

export interface CreatePortfolioDto {
  title: string;
  description: string;
}

export interface UpdatePortfolioDto {
  title: string;
  description: string;
}
