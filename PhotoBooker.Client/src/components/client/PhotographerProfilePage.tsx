import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import photographerService from '../../services/photographerService';
import type { PhotographerDto, PortfolioDto } from '../../types/photographer';
import { getCategoryName } from '../../utils/categoryHelper';
import './PhotographerProfilePage.css';

const PhotographerProfilePage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [photographer, setPhotographer] = useState<PhotographerDto | null>(null);
  const [portfolios, setPortfolios] = useState<PortfolioDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadPhotographerData();
  }, [id]);

  const loadPhotographerData = async () => {
    try {
      setLoading(true);
      setError('');
      if (id) {
        const photographerId = parseInt(id);
        const [photographerData, portfoliosData] = await Promise.all([
          photographerService.getPhotographerById(photographerId),
          photographerService.getPhotographerPortfolios(photographerId)
        ]);
        setPhotographer(photographerData);
        setPortfolios(portfoliosData);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load photographer profile');
      console.error('Error loading photographer:', err);
    } finally {
      setLoading(false);
    }
  };

  const handlePortfolioClick = (portfolioId: number) => {
    navigate(`/portfolio/${portfolioId}`);
  };

  if (loading) {
    return (
      <div className="photographer-profile-container">
        <div className="loading">Loading photographer profile...</div>
      </div>
    );
  }

  if (error || !photographer) {
    return (
      <div className="photographer-profile-container">
        <div className="error-message">{error || 'Photographer not found'}</div>
        <button onClick={() => navigate('/client')} className="btn-back">
          Back to Photographers
        </button>
      </div>
    );
  }

  return (
    <div className="photographer-profile-container">
      <button onClick={() => navigate('/client')} className="btn-back">
        ← Back to Photographers
      </button>

      <div className="photographer-profile-header">
        <div className="photographer-avatar-large">
          {photographer.firstName.charAt(0)}
          {photographer.lastName.charAt(0)}
        </div>
        <div className="photographer-details">
          <h1>{photographer.firstName} {photographer.lastName}</h1>
          <p className="photographer-username">@{photographer.username}</p>
          {photographer.bio && (
            <p className="photographer-bio">{photographer.bio}</p>
          )}
          <p className="photographer-member-since">
            Member since {new Date(photographer.createdAt).toLocaleDateString('en-US', {
              year: 'numeric',
              month: 'long',
              day: 'numeric'
            })}
          </p>
          <button className="btn-book-photographer">Book This Photographer</button>
        </div>
      </div>

      <div className="portfolios-section">
        <h2>Portfolios</h2>
        {portfolios.length === 0 ? (
          <div className="no-portfolios">
            <p>This photographer hasn't created any portfolios yet.</p>
          </div>
        ) : (
          <div className="portfolios-grid">
            {portfolios.map((portfolio) => (
              <div 
                key={portfolio.id} 
                className="portfolio-card"
                onClick={() => handlePortfolioClick(portfolio.id)}
              >
                <div className="portfolio-card-header">
                  <h3>{portfolio.title}</h3>
                  <span className="portfolio-category">{getCategoryName(portfolio.category)}</span>
                </div>
                <p className="portfolio-description">{portfolio.description}</p>
                <p className="portfolio-date">
                  Created {new Date(portfolio.createdDate).toLocaleDateString()}
                </p>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default PhotographerProfilePage;
