import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import portfolioService from '../../services/portfolioService';
import type { PortfolioWithImagesDto } from '../../types/photographer';
import './PortfolioViewPage.css';

const PortfolioViewPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [portfolio, setPortfolio] = useState<PortfolioWithImagesDto | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');
  const [selectedImageIndex, setSelectedImageIndex] = useState<number>(0);
  const [lightboxOpen, setLightboxOpen] = useState<boolean>(false);

  useEffect(() => {
    loadPortfolio();
  }, [id]);

  const loadPortfolio = async () => {
    try {
      setLoading(true);
      setError('');
      if (id) {
        const data = await portfolioService.getPortfolioById(parseInt(id));
        setPortfolio(data);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load portfolio');
      console.error('Error loading portfolio:', err);
    } finally {
      setLoading(false);
    }
  };

  const openLightbox = (index: number) => {
    setSelectedImageIndex(index);
    setLightboxOpen(true);
  };

  const closeLightbox = () => {
    setLightboxOpen(false);
  };

  const nextImage = () => {
    if (portfolio && selectedImageIndex < portfolio.images.length - 1) {
      setSelectedImageIndex(selectedImageIndex + 1);
    }
  };

  const prevImage = () => {
    if (selectedImageIndex > 0) {
      setSelectedImageIndex(selectedImageIndex - 1);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Escape') {
      closeLightbox();
    } else if (e.key === 'ArrowRight') {
      nextImage();
    } else if (e.key === 'ArrowLeft') {
      prevImage();
    }
  };

  if (loading) {
    return (
      <div className="portfolio-view-container">
        <div className="loading">Loading portfolio...</div>
      </div>
    );
  }

  if (error || !portfolio) {
    return (
      <div className="portfolio-view-container">
        <div className="error-message">
          <p>{error || 'Portfolio not found'}</p>
          <button onClick={() => navigate(-1)} className="btn-back">
            Go Back
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="portfolio-view-container">
      <button onClick={() => navigate(-1)} className="btn-back-header">
        ← Back
      </button>

      <div className="portfolio-header">
        <h1 className="portfolio-title">{portfolio.title}</h1>
        <p className="portfolio-photographer">by {portfolio.photographerName}</p>
        <p className="portfolio-date">
          Created on {new Date(portfolio.createdDate).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
          })}
        </p>
      </div>

      <div className="portfolio-description">
        <p>{portfolio.description}</p>
      </div>

      {portfolio.images.length === 0 ? (
        <div className="no-images">
          <p>No images in this portfolio yet.</p>
        </div>
      ) : (
        <div className="gallery-grid">
          {portfolio.images.map((image, index) => (
            <div
              key={image.id}
              className="gallery-item"
              onClick={() => openLightbox(index)}
            >
              <img
                src={image.imageUrl}
                alt={`${portfolio.title} - Image ${index + 1}`}
                loading="lazy"
              />
              <div className="gallery-overlay">
                <span className="view-icon">🔍</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {lightboxOpen && portfolio.images.length > 0 && (
        <div
          className="lightbox"
          onClick={closeLightbox}
          onKeyDown={handleKeyDown}
          tabIndex={0}
        >
          <button className="lightbox-close" onClick={closeLightbox}>
            ×
          </button>
          
          {selectedImageIndex > 0 && (
            <button
              className="lightbox-nav lightbox-prev"
              onClick={(e) => {
                e.stopPropagation();
                prevImage();
              }}
            >
              ‹
            </button>
          )}

          <div className="lightbox-content" onClick={(e) => e.stopPropagation()}>
            <img
              src={portfolio.images[selectedImageIndex].imageUrl}
              alt={`${portfolio.title} - Image ${selectedImageIndex + 1}`}
            />
            <div className="lightbox-counter">
              {selectedImageIndex + 1} / {portfolio.images.length}
            </div>
          </div>

          {selectedImageIndex < portfolio.images.length - 1 && (
            <button
              className="lightbox-nav lightbox-next"
              onClick={(e) => {
                e.stopPropagation();
                nextImage();
              }}
            >
              ›
            </button>
          )}
        </div>
      )}
    </div>
  );
};

export default PortfolioViewPage;
