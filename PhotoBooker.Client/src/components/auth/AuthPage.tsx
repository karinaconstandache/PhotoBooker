import React, { useState } from 'react';
import Login from './Login';
import Register from './Register';
import './AuthPage.css';

const AuthPage: React.FC = () => {
  const [isLogin, setIsLogin] = useState<boolean>(true);

  const handleAuthSuccess = () => {
    window.location.href = '/';
  };

  return (
    <div className="auth-container">
      <div className="auth-welcome">
        <h1>Welcome to PhotoBooker</h1>
        <p>A platform that connects photographers and clients</p>
      </div>
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
