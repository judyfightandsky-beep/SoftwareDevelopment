import React from 'react';
import { useAuth } from '../contexts/AuthContext';

// 統計卡片組件
const StatCard: React.FC<{
  title: string;
  value: string | number;
  change: string;
  changeType: 'positive' | 'negative' | 'neutral';
  icon: React.ReactNode;
  bgColor: string;
}> = ({ title, value, change, changeType, icon, bgColor }) => {
  const changeColor = {
    positive: 'text-green-600',
    negative: 'text-red-600',
    neutral: 'text-gray-600'
  }[changeType];

  return (
    <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm font-medium text-gray-600">{title}</p>
          <p className="text-3xl font-semibold text-gray-900">{value}</p>
        </div>
        <div className={`w-12 h-12 ${bgColor} rounded-lg flex items-center justify-center`}>
          {icon}
        </div>
      </div>
      <div className="mt-4 flex items-center text-sm">
        <span className={`font-medium ${changeColor}`}>{change}</span>
        <span className="text-gray-600 ml-1">較上週</span>
      </div>
    </div>
  );
};

// 專案進度項目組件
const ProjectProgressItem: React.FC<{
  name: string;
  progress: number;
  status: string;
  statusColor: string;
}> = ({ name, progress, status, statusColor }) => {
  return (
    <div className="flex items-center justify-between">
      <div className="flex-1">
        <div className="flex items-center justify-between mb-2">
          <span className="text-sm font-medium text-gray-900">{name}</span>
          <span className="text-sm text-gray-600">{progress}%</span>
        </div>
        <div className="w-full bg-gray-200 rounded-full h-2">
          <div
            className="bg-blue-600 h-2 rounded-full"
            style={{ width: `${progress}%` }}
          ></div>
        </div>
      </div>
      <div className="ml-4 text-sm text-gray-600">
        <span className={`px-2 py-1 ${statusColor} rounded-full`}>{status}</span>
      </div>
    </div>
  );
};

// 最近活動項目組件
const ActivityItem: React.FC<{
  user: string;
  action: string;
  time: string;
  type: 'success' | 'warning' | 'info';
}> = ({ user, action, time, type }) => {
  const dotColor = {
    success: 'bg-green-600',
    warning: 'bg-yellow-600',
    info: 'bg-blue-600'
  }[type];

  return (
    <div className="flex space-x-3">
      <div className={`flex-shrink-0 w-2 h-2 ${dotColor} rounded-full mt-2`}></div>
      <div className="flex-1">
        <p className="text-sm text-gray-900">{user} {action}</p>
        <p className="text-xs text-gray-500">{time}</p>
      </div>
    </div>
  );
};

// 主儀表板組件
const Dashboard: React.FC = () => {
  const { user, logout } = useAuth();

  return (
    <div className="min-h-screen bg-gray-50" style={{ fontSize: '14px' }}>
      {/* Header Navigation */}
      <header className="bg-white shadow-sm border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <a
                  href="/"
                  className="text-xl font-bold text-gray-900 hover:text-blue-600 transition-colors"
                >
                  軟體開發專案管理平台
                </a>
              </div>
            </div>

            <div className="flex items-center space-x-4">
              {/* Search */}
              <div className="relative">
                <input
                  type="text"
                  placeholder="搜尋專案、任務..."
                  className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                />
              </div>

              {/* Notifications */}
              <button className="p-2 text-gray-400 hover:text-gray-600 relative">
                <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 17h5l-5 5v-5zM9 7h6l-6-6v6z" />
                </svg>
                <div className="absolute -top-1 -right-1 w-5 h-5 bg-red-500 rounded-full flex items-center justify-center">
                  <span className="text-xs font-medium text-white">3</span>
                </div>
              </button>

              {/* User Menu */}
              <div className="relative flex items-center space-x-3">
                <span className="text-sm text-gray-700">歡迎, {user?.username}</span>
                <button
                  onClick={logout}
                  className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 transition-colors"
                >
                  登出
                </button>
              </div>
            </div>
          </div>
        </div>
      </header>

      <div className="flex">
        {/* Sidebar */}
        <nav className="w-64 bg-white shadow-sm min-h-screen border-r border-gray-200">
          <div className="p-4">
            <ul className="space-y-2">
              <li>
                <a
                  href="#"
                  className="flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-lg"
                >
                  <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2H5a2 2 0 00-2-2z" />
                  </svg>
                  專案總覽
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="flex items-center px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-50 rounded-lg"
                >
                  <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
                  </svg>
                  任務管理
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="flex items-center px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-50 rounded-lg"
                >
                  <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
                  </svg>
                  模板中心
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="flex items-center px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-50 rounded-lg"
                >
                  <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                  </svg>
                  品質中心
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="flex items-center px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-50 rounded-lg"
                >
                  <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v4a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
                  </svg>
                  報表
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="flex items-center px-4 py-2 text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-50 rounded-lg"
                >
                  <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" />
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  </svg>
                  設定
                </a>
              </li>
            </ul>
          </div>
        </nav>

        {/* Main Content Area */}
        <main className="flex-1 p-8">
          {/* 專案統計卡片 */}
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
            <StatCard
              title="進行中專案"
              value="12"
              change="+2"
              changeType="positive"
              bgColor="bg-blue-100"
              icon={
                <svg className="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
                </svg>
              }
            />

            <StatCard
              title="待辦任務"
              value="47"
              change="3 即將逾期"
              changeType="negative"
              bgColor="bg-orange-100"
              icon={
                <svg className="w-6 h-6 text-orange-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
                </svg>
              }
            />

            <StatCard
              title="代碼品質"
              value="8.5"
              change="+0.3"
              changeType="positive"
              bgColor="bg-green-100"
              icon={
                <svg className="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              }
            />

            <StatCard
              title="團隊效率"
              value="92%"
              change="+5%"
              changeType="positive"
              bgColor="bg-purple-100"
              icon={
                <svg className="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
                </svg>
              }
            />
          </div>

          {/* 專案進度概覽 */}
          <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 mb-8">
            <div className="flex items-center justify-between mb-6">
              <h3 className="text-lg font-semibold text-gray-900">專案進度概覽</h3>
              <div className="flex space-x-2">
                <button className="px-3 py-1 text-sm font-medium text-blue-600 bg-blue-50 rounded-md">
                  本週
                </button>
                <button className="px-3 py-1 text-sm font-medium text-gray-600 hover:text-gray-900 rounded-md">
                  本月
                </button>
                <button className="px-3 py-1 text-sm font-medium text-gray-600 hover:text-gray-900 rounded-md">
                  本季
                </button>
              </div>
            </div>

            <div className="space-y-4">
              <ProjectProgressItem
                name="電商平台重構專案"
                progress={85}
                status="進行中"
                statusColor="bg-green-100 text-green-800"
              />
              <ProjectProgressItem
                name="用戶管理系統"
                progress={65}
                status="需關注"
                statusColor="bg-yellow-100 text-yellow-800"
              />
              <ProjectProgressItem
                name="API 文件系統"
                progress={100}
                status="已完成"
                statusColor="bg-gray-100 text-gray-800"
              />
            </div>
          </div>

          {/* 最近活動 & 任務列表 */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
            {/* 最近活動 */}
            <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
              <h3 className="text-lg font-semibold text-gray-900 mb-6">最近活動</h3>
              <div className="space-y-4">
                <ActivityItem
                  user="張三"
                  action="完成了任務：用戶認證API實作"
                  time="2 小時前"
                  type="success"
                />
                <ActivityItem
                  user="李四"
                  action="提交了代碼審查請求"
                  time="4 小時前"
                  type="info"
                />
                <ActivityItem
                  user="王五"
                  action="更新了專案進度至 75%"
                  time="6 小時前"
                  type="info"
                />
                <ActivityItem
                  user="趙六"
                  action="發現了程式碼品質問題"
                  time="8 小時前"
                  type="warning"
                />
              </div>
            </div>

            {/* 我的任務 */}
            <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
              <h3 className="text-lg font-semibold text-gray-900 mb-6">我的任務</h3>
              <div className="space-y-4">
                <div className="flex items-center justify-between p-3 border border-gray-200 rounded-lg">
                  <div className="flex-1">
                    <h4 className="text-sm font-medium text-gray-900">前端登入頁面優化</h4>
                    <p className="text-xs text-gray-600">截止日期: 明天</p>
                  </div>
                  <span className="px-2 py-1 text-xs font-medium bg-red-100 text-red-800 rounded">高</span>
                </div>
                <div className="flex items-center justify-between p-3 border border-gray-200 rounded-lg">
                  <div className="flex-1">
                    <h4 className="text-sm font-medium text-gray-900">API 文件更新</h4>
                    <p className="text-xs text-gray-600">截止日期: 本週五</p>
                  </div>
                  <span className="px-2 py-1 text-xs font-medium bg-yellow-100 text-yellow-800 rounded">中</span>
                </div>
                <div className="flex items-center justify-between p-3 border border-gray-200 rounded-lg">
                  <div className="flex-1">
                    <h4 className="text-sm font-medium text-gray-900">單元測試撰寫</h4>
                    <p className="text-xs text-gray-600">截止日期: 下週一</p>
                  </div>
                  <span className="px-2 py-1 text-xs font-medium bg-blue-100 text-blue-800 rounded">低</span>
                </div>
              </div>
              <div className="mt-4">
                <button className="w-full py-2 text-sm font-medium text-blue-600 border border-blue-600 rounded-md hover:bg-blue-50 transition-colors">
                  查看所有任務
                </button>
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default Dashboard;