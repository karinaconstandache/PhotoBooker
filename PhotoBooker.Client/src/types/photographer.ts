export const PortfolioCategory = {
  Portraits: 0,
  Outdoors: 1,
  Products: 2,
  Wedding: 3,
  Events: 4,
  Fashion: 5,
  Architecture: 6,
  Wildlife: 7,
  Sports: 8,
  Food: 9,
  Other: 10
} as const;

export type PortfolioCategory = typeof PortfolioCategory[keyof typeof PortfolioCategory];

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

export interface PortfolioImageDto {
  id: number;
  imageUrl: string;
  displayOrder: number;
}

export interface PortfolioDto {
  id: number;
  title: string;
  description: string;
  category: PortfolioCategory;
  createdDate: string;
  photographerId: number;
  photographerName: string;
}

export interface PortfolioWithImagesDto extends PortfolioDto {
  images: PortfolioImageDto[];
}

export interface CreatePortfolioDto {
  title: string;
  description: string;
  category: PortfolioCategory;
}

export interface UpdatePortfolioDto {
  title: string;
  description: string;
  category: PortfolioCategory;
}
