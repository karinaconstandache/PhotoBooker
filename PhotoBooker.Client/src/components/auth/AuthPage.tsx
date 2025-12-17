import React, { useState } from 'react';
import Login from './Login';
import Register from './Register';
import authService from '../../services/authService';
import './AuthPage.css';

const AuthPage: React.FC = () => {
  const [isLogin, setIsLogin] = useState<boolean>(true);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

  const handleAuthSuccess = () => {
    setIsAuthenticated(true);
    const user = authService.getCurrentUser();
    console.log('User authenticated:', user);
    // You can redirect to another page or show a dashboard here
  };

  const handleLogout = () => {
    authService.logout();
    setIsAuthenticated(false);
  };

  if (isAuthenticated) {
    const user = authService.getCurrentUser();
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
            <p>
              <strong>Role:</strong> {user?.role === 1 ? 'Photographer' : 'Client'}
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
