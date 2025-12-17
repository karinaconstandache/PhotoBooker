import React from 'react';
import { Outlet, useNavigate } from 'react-router-dom';
import authService from '../../services/authService';
import './Layout.css';

const Layout: React.FC = () => {
  const navigate = useNavigate();
  const user = authService.getCurrentUser();

  const handleLogout = () => {
    authService.logout();
    window.location.href = '/';
  };

  return (
    <div className="app-layout">
      <nav className="app-navbar">
        <div className="navbar-content">
          <h2 className="navbar-brand">PhotoBooker</h2>
          <div className="navbar-user">
            <span className="user-name">
              {user?.firstName} {user?.lastName}
            </span>
            <button onClick={handleLogout} className="btn-logout">
              Logout
            </button>
          </div>
        </div>
      </nav>
      <main className="app-main">
        <Outlet />
      </main>
    </div>
  );
};

export default Layout;
