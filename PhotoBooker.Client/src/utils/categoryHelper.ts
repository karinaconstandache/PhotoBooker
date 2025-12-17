import { PortfolioCategory } from '../types/photographer';

export const getCategoryName = (category: PortfolioCategory): string => {
  switch (category) {
    case PortfolioCategory.Portraits:
      return 'Portraits';
    case PortfolioCategory.Outdoors:
      return 'Outdoors';
    case PortfolioCategory.Products:
      return 'Products';
    case PortfolioCategory.Wedding:
      return 'Wedding';
    case PortfolioCategory.Events:
      return 'Events';
    case PortfolioCategory.Fashion:
      return 'Fashion';
    case PortfolioCategory.Architecture:
      return 'Architecture';
    case PortfolioCategory.Wildlife:
      return 'Wildlife';
    case PortfolioCategory.Sports:
      return 'Sports';
    case PortfolioCategory.Food:
      return 'Food';
    case PortfolioCategory.Other:
      return 'Other';
    default:
      return 'Other';
  }
};
