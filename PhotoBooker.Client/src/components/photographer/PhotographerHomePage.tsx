import React, { useEffect, useState } from 'react';
import portfolioService from '../../services/portfolioService';
import type { PortfolioDto, CreatePortfolioDto, UpdatePortfolioDto } from '../../types/photographer';
import './PhotographerHomePage.css';

const PhotographerHomePage: React.FC = () => {
  const [portfolios, setPortfolios] = useState<PortfolioDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');
  const [showCreateModal, setShowCreateModal] = useState<boolean>(false);
  const [editingPortfolio, setEditingPortfolio] = useState<PortfolioDto | null>(null);
  const [formData, setFormData] = useState<CreatePortfolioDto>({
    title: '',
    description: '',
  });

  useEffect(() => {
    loadPortfolios();
  }, []);

  const loadPortfolios = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await portfolioService.getMyPortfolios();
      setPortfolios(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load portfolios');
      console.error('Error loading portfolios:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateClick = () => {
    setFormData({ title: '', description: '' });
    setEditingPortfolio(null);
    setShowCreateModal(true);
  };

  const handleEditClick = (portfolio: PortfolioDto) => {
    setFormData({ title: portfolio.title, description: portfolio.description });
    setEditingPortfolio(portfolio);
    setShowCreateModal(true);
  };

  const handleCloseModal = () => {
    setShowCreateModal(false);
    setEditingPortfolio(null);
    setFormData({ title: '', description: '' });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingPortfolio) {
        await portfolioService.updatePortfolio(editingPortfolio.id, formData as UpdatePortfolioDto);
      } else {
        await portfolioService.createPortfolio(formData);
      }
      await loadPortfolios();
      handleCloseModal();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to save portfolio');
      console.error('Error saving portfolio:', err);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this portfolio?')) {
      return;
    }

    try {
      await portfolioService.deletePortfolio(id);
      await loadPortfolios();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to delete portfolio');
      console.error('Error deleting portfolio:', err);
    }
  };

  if (loading) {
    return (
      <div className="photographer-home-container">
        <div className="loading">Loading your portfolios...</div>
      </div>
    );
  }

  return (
    <div className="photographer-home-container">
      <div className="photographer-home-header">
        <h1>My Portfolios</h1>
        <button onClick={handleCreateClick} className="btn-create">
          + Create New Portfolio
        </button>
      </div>

      {error && (
        <div className="error-banner">
          {error}
          <button onClick={() => setError('')} className="btn-close-error">
            ×
          </button>
        </div>
      )}

      {portfolios.length === 0 ? (
        <div className="no-portfolios">
          <p>You haven't created any portfolios yet.</p>
          <button onClick={handleCreateClick} className="btn-create-first">
            Create Your First Portfolio
          </button>
        </div>
      ) : (
        <div className="portfolios-grid">
          {portfolios.map((portfolio) => (
            <div key={portfolio.id} className="portfolio-card">
              <div className="portfolio-header">
                <h3 className="portfolio-title">{portfolio.title}</h3>
                <span className="portfolio-date">
                  {new Date(portfolio.createdDate).toLocaleDateString()}
                </span>
              </div>
              <p className="portfolio-description">{portfolio.description}</p>
              <div className="portfolio-actions">
                <button
                  onClick={() => handleEditClick(portfolio)}
                  className="btn-edit"
                >
                  Edit
                </button>
                <button
                  onClick={() => handleDelete(portfolio.id)}
                  className="btn-delete"
                >
                  Delete
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {showCreateModal && (
        <div className="modal-overlay" onClick={handleCloseModal}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>{editingPortfolio ? 'Edit Portfolio' : 'Create New Portfolio'}</h2>
              <button onClick={handleCloseModal} className="btn-close-modal">
                ×
              </button>
            </div>
            <form onSubmit={handleSubmit} className="portfolio-form">
              <div className="form-group">
                <label htmlFor="title">Title</label>
                <input
                  type="text"
                  id="title"
                  value={formData.title}
                  onChange={(e) =>
                    setFormData({ ...formData, title: e.target.value })
                  }
                  placeholder="Enter portfolio title"
                  required
                  maxLength={100}
                />
              </div>
              <div className="form-group">
                <label htmlFor="description">Description</label>
                <textarea
                  id="description"
                  value={formData.description}
                  onChange={(e) =>
                    setFormData({ ...formData, description: e.target.value })
                  }
                  placeholder="Enter portfolio description"
                  required
                  maxLength={500}
                  rows={5}
                />
              </div>
              <div className="form-actions">
                <button type="button" onClick={handleCloseModal} className="btn-cancel">
                  Cancel
                </button>
                <button type="submit" className="btn-submit">
                  {editingPortfolio ? 'Update' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default PhotographerHomePage;
