import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import photographerService from '../../services/photographerService';
import type { PhotographerDto } from '../../types/photographer';
import './ClientHomePage.css';

const ClientHomePage: React.FC = () => {
  const navigate = useNavigate();
  const [photographers, setPhotographers] = useState<PhotographerDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadPhotographers();
  }, []);

  const loadPhotographers = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await photographerService.getAllPhotographers();
      setPhotographers(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load photographers');
      console.error('Error loading photographers:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="client-home-container">
        <div className="loading">Loading photographers...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="client-home-container">
        <div className="error-message">{error}</div>
        <button onClick={loadPhotographers} className="btn-retry">
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="client-home-container">
      <div className="client-home-header">
        <h1>Find Your Photographer</h1>
        <p>Browse through our talented photographers and find the perfect match for your needs</p>
      </div>

      {photographers.length === 0 ? (
        <div className="no-photographers">
          <p>No photographers available at the moment.</p>
        </div>
      ) : (
        <div className="photographers-grid">
          {photographers.map((photographer) => (
            <div key={photographer.id} className="photographer-card">
              <div className="photographer-avatar">
                {photographer.firstName.charAt(0)}
                {photographer.lastName.charAt(0)}
              </div>
              <div className="photographer-info">
                <h3 className="photographer-name">
                  {photographer.firstName} {photographer.lastName}
                </h3>
                <p className="photographer-username">@{photographer.username}</p>
                {photographer.bio && (
                  <p className="photographer-bio">{photographer.bio}</p>
                )}
                <p className="photographer-member-since">
                  Member since {new Date(photographer.createdAt).toLocaleDateString()}
                </p>
              </div>
              <div className="photographer-actions">
                <button 
                  className="btn-view-profile"
                  onClick={() => navigate(`/photographer/${photographer.id}`)}
                >
                  View Profile
                </button>
                <button className="btn-book">Book Now</button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ClientHomePage;
