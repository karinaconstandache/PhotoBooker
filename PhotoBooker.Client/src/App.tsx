import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import AuthPage from './components/auth/AuthPage';
import { ClientHomePage, PhotographerProfilePage } from './components/client';
import { PhotographerHomePage, PortfolioViewPage } from './components/photographer';
import { Layout } from './components/layout';
import authService from './services/authService';

const App: React.FC = () => {
  const [isAuthenticated, setIsAuthenticated] = React.useState(false);
  const [userRole, setUserRole] = React.useState<number | null>(null);

  React.useEffect(() => {
    const token = localStorage.getItem('token');
    const user = authService.getCurrentUser();
    setIsAuthenticated(!!token);
    setUserRole(user?.role ?? null);
  }, []);

  return (
    <Router>
      {!isAuthenticated ? (
        <Routes>
          <Route path="*" element={<AuthPage />} />
        </Routes>
      ) : (
        <Routes>
          <Route element={<Layout />}>
            <Route path="/" element={
              userRole === 1 
                ? <Navigate to="/photographer" replace /> 
                : <Navigate to="/client" replace />
            } />
            
            <Route path="/client" element={<ClientHomePage />} />
            <Route path="/photographer" element={<PhotographerHomePage />} />
            <Route path="/photographer/:id" element={<PhotographerProfilePage />} />
            <Route path="/portfolio/:id" element={<PortfolioViewPage />} />
            
            <Route path="*" element={<Navigate to="/" replace />} />
          </Route>
        </Routes>
      )}
    </Router>
  );
};

export default App;
