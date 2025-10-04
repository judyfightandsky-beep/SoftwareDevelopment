#!/bin/bash

echo "🚀 啟動前端開發環境 (使用 Mock Server)"
echo "============================================="

# 確保在正確的目錄
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"

# 檢查 Node.js
if ! command -v node &> /dev/null; then
    echo "❌ Node.js 未安裝，請先安裝 Node.js"
    exit 1
fi

# 檢查前端依賴
echo "📦 檢查前端依賴..."
cd "$PROJECT_ROOT/frontend"
if [ ! -d "node_modules" ]; then
    echo "安裝前端依賴中..."
    npm run install:all
fi

# 檢查 mockserver 依賴
echo "📦 檢查 Mock Server 依賴..."
cd "$PROJECT_ROOT/mockserver"
if [ ! -d "node_modules" ]; then
    echo "安裝 Mock Server 依賴中..."
    npm install
fi

# 啟動 Mock Server (後台運行)
echo "🔥 啟動 Mock Server..."
npm start &
MOCK_SERVER_PID=$!

# 等待 Mock Server 啟動
sleep 3

# 檢查 Mock Server 是否成功啟動
if curl -s http://localhost:3010/api/v2/health > /dev/null; then
    echo "✅ Mock Server 啟動成功"
else
    echo "❌ Mock Server 啟動失敗"
    kill $MOCK_SERVER_PID 2>/dev/null
    exit 1
fi

# 啟動前端應用
echo "🎯 啟動前端應用..."
cd "$PROJECT_ROOT/frontend"
npm run dev &
FRONTEND_PID=$!

echo ""
echo "🎉 開發環境啟動完成！"
echo "📍 前端應用: http://localhost:3000"
echo "📊 Mock API: http://localhost:3010/api/v2"
echo "🔐 測試登入: ming.zhang@example.com / password123"
echo ""
echo "按 Ctrl+C 停止所有服務..."

# 清理函數
cleanup() {
    echo ""
    echo "🛑 正在停止服務..."
    kill $MOCK_SERVER_PID 2>/dev/null
    kill $FRONTEND_PID 2>/dev/null
    echo "✅ 服務已停止"
    exit 0
}

# 註冊清理函數
trap cleanup SIGINT SIGTERM

# 等待
wait