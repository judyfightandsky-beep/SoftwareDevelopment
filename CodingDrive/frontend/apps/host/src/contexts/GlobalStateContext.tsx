import React, { createContext, useState, useContext, ReactNode } from 'react';

interface GlobalStateContextType {
  isLoading: boolean;
  errorMessage: string | null;
  setLoading: (isLoading: boolean) => void;
  setErrorMessage: (message: string | null) => void;
}

const GlobalStateContext = createContext<GlobalStateContextType>({
  isLoading: false,
  errorMessage: null,
  setLoading: () => {},
  setErrorMessage: () => {}
});

export const GlobalStateProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const setLoading = (loading: boolean) => {
    setIsLoading(loading);
  };

  const handleErrorMessage = (message: string | null) => {
    setErrorMessage(message);
    if (message) {
      setTimeout(() => setErrorMessage(null), 5000); // Auto-clear error after 5 seconds
    }
  };

  return (
    <GlobalStateContext.Provider
      value={{
        isLoading,
        errorMessage,
        setLoading,
        setErrorMessage: handleErrorMessage
      }}
    >
      {children}
      {isLoading && (
        <div style={{
          position: 'fixed',
          top: 0,
          left: 0,
          width: '100%',
          height: '100%',
          backgroundColor: 'rgba(0,0,0,0.5)',
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          zIndex: 9999
        }}>
          <div className="spinner">Loading...</div>
        </div>
      )}
      {errorMessage && (
        <div style={{
          position: 'fixed',
          top: '10px',
          right: '10px',
          backgroundColor: 'red',
          color: 'white',
          padding: '10px',
          borderRadius: '5px',
          zIndex: 10000
        }}>
          {errorMessage}
        </div>
      )}
    </GlobalStateContext.Provider>
  );
};

export const useGlobalState = () => useContext(GlobalStateContext);