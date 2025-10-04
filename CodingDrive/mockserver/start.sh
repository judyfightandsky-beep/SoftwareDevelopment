#!/bin/bash

echo "🚀 啟動 Software Development Mock Server"
echo "=====================================\n"

# 檢查 Node.js 是否已安裝
if ! command -v node &> /dev/null; then
    echo "❌ Node.js 未安裝，請先安裝 Node.js"
    exit 1
fi

# 檢查是否已安裝依賴
if [ ! -d "node_modules" ]; then
    echo "📦 安裝依賴中..."
    npm install
    if [ $? -ne 0 ]; then
        echo "❌ 依賴安裝失敗"
        exit 1
    fi
    echo "✅ 依賴安裝完成\n"
fi

# 檢查數據庫文件是否存在
if [ ! -f "db.json" ]; then
    echo "❌ 數據庫文件 db.json 不存在"
    exit 1
fi

# 啟動服務器
echo "🔥 啟動 Mock Server..."
echo "📍 服務器地址: http://localhost:3001"
echo "📊 API 基礎路徑: http://localhost:3001/api/v2"
echo "🔐 測試登入: ming.zhang@example.com / password123"
echo "📖 可用資源: users, templates, projects, tasks, qualityChecks, workflows"
echo "\n按 Ctrl+C 停止服務器\n"

npm start