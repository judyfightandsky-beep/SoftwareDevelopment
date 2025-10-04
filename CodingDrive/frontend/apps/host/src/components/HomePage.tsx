import React, { useEffect, useState } from 'react';
import homePageImage from '../assets/images/首頁.jpg';

const HomePage: React.FC = () => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [mousePosition, setMousePosition] = useState({ x: 0, y: 0 });

  useEffect(() => {
    setIsLoaded(true);

    const handleMouseMove = (e: MouseEvent) => {
      setMousePosition({ x: e.clientX, y: e.clientY });
    };

    window.addEventListener('mousemove', handleMouseMove);
    return () => window.removeEventListener('mousemove', handleMouseMove);
  }, []);

  const parallaxOffset = {
    x: (mousePosition.x - window.innerWidth / 2) * 0.01,
    y: (mousePosition.y - window.innerHeight / 2) * 0.01,
  };

  return (
    <div style={{
      position: 'relative',
      minHeight: '100vh',
      overflow: 'hidden',
      background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
    }}>
      {/* 動態背景粒子效果 */}
      <div style={{
        position: 'absolute',
        top: 0,
        left: 0,
        width: '100%',
        height: '100%',
        background: `
          radial-gradient(circle at 20% 80%, rgba(120, 119, 198, 0.3) 0%, transparent 50%),
          radial-gradient(circle at 80% 20%, rgba(255, 119, 198, 0.3) 0%, transparent 50%),
          radial-gradient(circle at 40% 40%, rgba(120, 219, 255, 0.3) 0%, transparent 50%)
        `,
        animation: 'float 6s ease-in-out infinite'
      }}></div>

      {/* 主要內容區 */}
      <div style={{
        position: 'relative',
        zIndex: 2,
        transform: `translate(${parallaxOffset.x}px, ${parallaxOffset.y}px)`,
        transition: 'transform 0.1s ease-out'
      }}>
        {/* Hero Section */}
        <section style={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          minHeight: '100vh',
          padding: '2rem',
          textAlign: 'center'
        }}>
          {/* 主標題 */}
          <h1 style={{
            fontSize: '4rem',
            fontWeight: '700',
            color: 'white',
            marginBottom: '1rem',
            textShadow: '0 4px 20px rgba(0,0,0,0.3)',
            transform: isLoaded ? 'translateY(0)' : 'translateY(50px)',
            opacity: isLoaded ? 1 : 0,
            transition: 'all 1s ease-out',
            animation: 'glow 2s ease-in-out infinite alternate'
          }}>
            軟體開發專案管理平台
          </h1>

          {/* 副標題 */}
          <p style={{
            fontSize: '1.5rem',
            color: 'rgba(255, 255, 255, 0.9)',
            marginBottom: '3rem',
            maxWidth: '600px',
            transform: isLoaded ? 'translateY(0)' : 'translateY(30px)',
            opacity: isLoaded ? 1 : 0,
            transition: 'all 1s ease-out 0.3s'
          }}>
            運用AI技術打造未來軟體開發新體驗
          </p>

          {/* 主圖片容器 */}
          <div style={{
            position: 'relative',
            marginBottom: '3rem',
            transform: isLoaded ? 'scale(1)' : 'scale(0.8)',
            opacity: isLoaded ? 1 : 0,
            transition: 'all 1.2s ease-out 0.6s'
          }}>
            <div style={{
              position: 'absolute',
              top: '-20px',
              left: '-20px',
              right: '-20px',
              bottom: '-20px',
              background: 'linear-gradient(45deg, #ff6b6b, #4ecdc4, #45b7d1, #96ceb4)',
              borderRadius: '25px',
              filter: 'blur(20px)',
              opacity: 0.7,
              animation: 'pulse 3s ease-in-out infinite'
            }}></div>

            <img
              src={homePageImage}
              alt="軟體開發專案管理平台"
              style={{
                position: 'relative',
                width: '100%',
                maxWidth: '800px',
                height: 'auto',
                borderRadius: '15px',
                boxShadow: '0 25px 50px rgba(0, 0, 0, 0.25)',
                transform: `rotateY(${parallaxOffset.x * 2}deg) rotateX(${-parallaxOffset.y * 2}deg)`,
                transition: 'transform 0.1s ease-out'
              }}
            />
          </div>

          {/* CTA 按鈕群組 */}
          <div style={{
            display: 'flex',
            gap: '1.5rem',
            flexWrap: 'wrap',
            justifyContent: 'center',
            transform: isLoaded ? 'translateY(0)' : 'translateY(30px)',
            opacity: isLoaded ? 1 : 0,
            transition: 'all 1s ease-out 1s'
          }}>
            <a
              href="/register"
              style={{
                display: 'inline-block',
                background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                color: 'white',
                padding: '1rem 2.5rem',
                borderRadius: '50px',
                textDecoration: 'none',
                fontWeight: '600',
                fontSize: '1.1rem',
                boxShadow: '0 10px 30px rgba(102, 126, 234, 0.4)',
                transition: 'all 0.3s ease',
                border: '2px solid transparent'
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-3px)';
                e.currentTarget.style.boxShadow = '0 15px 40px rgba(102, 126, 234, 0.6)';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0)';
                e.currentTarget.style.boxShadow = '0 10px 30px rgba(102, 126, 234, 0.4)';
              }}
            >
              開始使用
            </a>

            <a
              href="/login"
              style={{
                display: 'inline-block',
                background: 'transparent',
                color: 'white',
                padding: '1rem 2.5rem',
                borderRadius: '50px',
                textDecoration: 'none',
                fontWeight: '600',
                fontSize: '1.1rem',
                border: '2px solid rgba(255, 255, 255, 0.3)',
                transition: 'all 0.3s ease',
                backdropFilter: 'blur(10px)'
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-3px)';
                e.currentTarget.style.background = 'rgba(255, 255, 255, 0.1)';
                e.currentTarget.style.borderColor = 'rgba(255, 255, 255, 0.6)';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0)';
                e.currentTarget.style.background = 'transparent';
                e.currentTarget.style.borderColor = 'rgba(255, 255, 255, 0.3)';
              }}
            >
              登入
            </a>
          </div>
        </section>
      </div>

      {/* 全局樣式 */}
      <style>{`
        @keyframes float {
          0%, 100% { transform: translateY(0px) rotate(0deg); }
          33% { transform: translateY(-20px) rotate(1deg); }
          66% { transform: translateY(10px) rotate(-1deg); }
        }

        @keyframes glow {
          0% { text-shadow: 0 4px 20px rgba(255,255,255,0.3); }
          100% { text-shadow: 0 4px 30px rgba(255,255,255,0.6), 0 0 40px rgba(102, 126, 234, 0.4); }
        }

        @keyframes pulse {
          0%, 100% { transform: scale(1); opacity: 0.7; }
          50% { transform: scale(1.05); opacity: 0.9; }
        }

        @media (max-width: 768px) {
          h1 { font-size: 2.5rem !important; }
          p { font-size: 1.2rem !important; }
          .cta-buttons { flex-direction: column !important; }
        }
      `}</style>
    </div>
  );
};

export default HomePage;