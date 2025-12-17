import React, { useState } from 'react';
import Login from './Login';
import Register from './Register';
import { ClientHomePage } from '../client';
import { PhotographerHomePage } from '../photographer';
import authService from '../../services/authService';
import './AuthPage.css';

const AuthPage: React.FC = () => {
  const [isLogin, setIsLogin] = useState<boolean>(true);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(
    authService.isAuthenticated()
  );

  const handleAuthSuccess = () => {
    setIsAuthenticated(true);
  };

  const handleLogout = () => {
    authService.logout();
    setIsAuthenticated(false);
  };

  if (isAuthenticated) {
    const user = authService.getCurrentUser();
    
    // Role-based routing
    // UserRole.Photographer = 1, UserRole.Client = 2
    if (user?.role === 1) {
      return (
        <div>
          <nav className="app-navbar">
            <div className="navbar-content">
              <h2>PhotoBooker - Photographer</h2>
              <div className="navbar-user">
                <span>
                  {user.firstName} {user.lastName}
                </span>
                <button onClick={handleLogout} className="btn-logout">
                  Logout
                </button>
              </div>
            </div>
          </nav>
          <PhotographerHomePage />
        </div>
      );
    } else if (user?.role === 2) {
      return (
        <div>
          <nav className="app-navbar">
            <div className="navbar-content">
              <h2>PhotoBooker - Client</h2>
              <div className="navbar-user">
                <span>
                  {user.firstName} {user.lastName}
                </span>
                <button onClick={handleLogout} className="btn-logout">
                  Logout
                </button>
              </div>
            </div>
          </nav>
          <ClientHomePage />
        </div>
      );
    }

    // Fallback for unspecified role
    return (
      <div className="auth-container">
        <div className="auth-card">
          <h1>Welcome to PhotoBooker!</h1>
          <div className="user-info">
            <p>
              <strong>Name:</strong> {user?.firstName} {user?.lastName}
            </p>
            <p>
              <strong>Username:</strong> {user?.username}
            </p>
            <p className="error-message">
              Your account role is not properly configured. Please contact support.
            </p>
          </div>
          <button onClick={handleLogout} className="btn-primary">
            Logout
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="auth-container">
      <div className="auth-card">
        {isLogin ? (
          <Login
            onSuccess={handleAuthSuccess}
            onSwitchToRegister={() => setIsLogin(false)}
          />
        ) : (
          <Register
            onSuccess={handleAuthSuccess}
            onSwitchToLogin={() => setIsLogin(true)}
          />
        )}
      </div>
    </div>
  );
};

export default AuthPage;
