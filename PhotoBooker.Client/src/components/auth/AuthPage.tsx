import React, { useState } from 'react';
import Login from './Login';
import Register from './Register';
import './AuthPage.css';

const AuthPage: React.FC = () => {
  const [isLogin, setIsLogin] = useState<boolean>(true);

  const handleAuthSuccess = () => {
    // Reload the page to trigger App.tsx re-render with new auth state
    window.location.href = '/';
  };

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
