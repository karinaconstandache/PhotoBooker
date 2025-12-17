import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import portfolioService from '../../services/portfolioService';
import type { PortfolioDto, CreatePortfolioDto, UpdatePortfolioDto, PortfolioCategory } from '../../types/photographer';
import { getCategoryName } from '../../utils/categoryHelper';
import './PhotographerHomePage.css';

const PhotographerHomePage: React.FC = () => {
  const navigate = useNavigate();
  const [portfolios, setPortfolios] = useState<PortfolioDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');
  const [showCreateModal, setShowCreateModal] = useState<boolean>(false);
  const [editingPortfolio, setEditingPortfolio] = useState<PortfolioDto | null>(null);
  const [selectedImages, setSelectedImages] = useState<File[]>([]);
  const [uploadingImages, setUploadingImages] = useState<boolean>(false);
  const [formData, setFormData] = useState<CreatePortfolioDto>({
    title: '',
    description: '',
    category: 0, 
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
    setFormData({ title: '', description: '', category: 0 });
    setEditingPortfolio(null);
    setSelectedImages([]);
    setShowCreateModal(true);
  };

  const handleEditClick = (portfolio: PortfolioDto) => {
    setFormData({ title: portfolio.title, description: portfolio.description, category: portfolio.category });
    setEditingPortfolio(portfolio);
    setSelectedImages([]);
    setShowCreateModal(true);
  };

  const handleCloseModal = () => {
    setShowCreateModal(false);
    setEditingPortfolio(null);
    setFormData({ title: '', description: '', category: 0 });
    setSelectedImages([]);
  };

  const handleImageSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      const files = Array.from(e.target.files);
      const validFiles = files.filter((file) => {
        const isValid = file.type === 'image/jpeg' || file.type === 'image/png';
        if (!isValid) {
          setError(`File ${file.name} is not a valid JPG or PNG image`);
        }
        return isValid;
      });
      setSelectedImages((prev) => [...prev, ...validFiles]);
    }
  };

  const handleRemoveImage = (index: number) => {
    setSelectedImages((prev) => prev.filter((_, i) => i !== index));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      let portfolioId: number;

      if (editingPortfolio) {
        await portfolioService.updatePortfolio(editingPortfolio.id, formData as UpdatePortfolioDto);
        portfolioId = editingPortfolio.id;
      } else {
        const newPortfolio = await portfolioService.createPortfolio(formData);
        portfolioId = newPortfolio.id;
      }

      // Upload images if any were selected
      if (selectedImages.length > 0) {
        setUploadingImages(true);
        try {
          for (let i = 0; i < selectedImages.length; i++) {
            await portfolioService.uploadImage(portfolioId, selectedImages[i], i);
          }
        } catch (err) {
          setError(err instanceof Error ? err.message : 'Failed to upload some images');
        } finally {
          setUploadingImages(false);
        }
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
              <div 
                className="portfolio-clickable" 
                onClick={() => navigate(`/portfolio/${portfolio.id}`)}
              >
                <div className="portfolio-header">
                  <h3 className="portfolio-title">{portfolio.title}</h3>
                  <span className="portfolio-category-badge">{getCategoryName(portfolio.category)}</span>
                </div>
                <p className="portfolio-description">{portfolio.description}</p>
                <span className="portfolio-date">
                  {new Date(portfolio.createdDate).toLocaleDateString()}
                </span>
              </div>
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
              <div className="form-group">
                <label htmlFor="category">Category</label>
                <select
                  id="category"
                  value={formData.category}
                  onChange={(e) =>
                    setFormData({ ...formData, category: parseInt(e.target.value) as PortfolioCategory })
                  }
                  required
                >
                  <option value={0}>Portraits</option>
                  <option value={1}>Outdoors</option>
                  <option value={2}>Products</option>
                  <option value={3}>Wedding</option>
                  <option value={4}>Events</option>
                  <option value={5}>Fashion</option>
                  <option value={6}>Architecture</option>
                  <option value={7}>Wildlife</option>
                  <option value={8}>Sports</option>
                  <option value={9}>Food</option>
                  <option value={10}>Other</option>
                </select>
              </div>
              <div className="form-group">
                <label htmlFor="images">Images (JPG or PNG)</label>
                <input
                  type="file"
                  id="images"
                  accept="image/jpeg,image/png"
                  multiple
                  onChange={handleImageSelect}
                  className="file-input"
                />
                {selectedImages.length > 0 && (
                  <div className="selected-images">
                    <p>{selectedImages.length} image(s) selected</p>
                    <div className="image-preview-grid">
                      {selectedImages.map((file, index) => (
                        <div key={index} className="image-preview">
                          <img
                            src={URL.createObjectURL(file)}
                            alt={`Preview ${index + 1}`}
                          />
                          <button
                            type="button"
                            onClick={() => handleRemoveImage(index)}
                            className="btn-remove-image"
                          >
                            ×
                          </button>
                        </div>
                      ))}
                    </div>
                  </div>
                )}
              </div>
              <div className="form-actions">
                <button type="button" onClick={handleCloseModal} className="btn-cancel">
                  Cancel
                </button>
                <button type="submit" className="btn-submit" disabled={uploadingImages}>
                  {uploadingImages ? 'Uploading...' : editingPortfolio ? 'Update' : 'Create'}
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
