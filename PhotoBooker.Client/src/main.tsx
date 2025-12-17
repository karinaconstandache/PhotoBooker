import React from 'react';
import ReactDOM from 'react-dom/client';
import AuthPage from './components/auth/AuthPage';
import './style.css';

ReactDOM.createRoot(document.getElementById('app')!).render(
  <React.StrictMode>
    <AuthPage />
  </React.StrictMode>
);
