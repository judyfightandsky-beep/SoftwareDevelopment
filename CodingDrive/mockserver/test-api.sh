#!/bin/bash

BASE_URL="http://localhost:3010/api/v2"

echo "🧪 測試 Mock Server API 端點"
echo "==============================="

# 測試健康檢查
echo "1. 測試健康檢查..."
curl -s "$BASE_URL/health" | jq '.'
echo ""

# 測試登入
echo "2. 測試用戶登入..."
LOGIN_RESPONSE=$(curl -s -X POST -H "Content-Type: application/json" \
  -d '{"email": "ming.zhang@example.com", "password": "password123"}' \
  "$BASE_URL/auth/login")

echo $LOGIN_RESPONSE | jq '.'

# 提取 token
TOKEN=$(echo $LOGIN_RESPONSE | jq -r '.data.token')
echo "Token: $TOKEN"
echo ""

# 測試模板列表
echo "3. 測試模板列表..."
curl -s "$BASE_URL/templates" | jq 'map({id, name, type}) | .[0:2]'
echo ""

# 測試熱門模板
echo "4. 測試熱門模板..."
curl -s "$BASE_URL/templates/popular" | jq 'map({id, name, downloadCount: .metadata.downloadCount}) | .[0:2]'
echo ""

# 測試需要認證的任務端點
echo "5. 測試任務列表 (需要認證)..."
curl -s -H "Authorization: Bearer $TOKEN" "$BASE_URL/tasks" | jq 'map({id, title, status}) | .[0:2]'
echo ""

# 測試品質檢查
echo "6. 測試品質檢查列表..."
curl -s -H "Authorization: Bearer $TOKEN" "$BASE_URL/quality-checks" | jq 'map({id, status, score: .overallScore.score}) | .[0:2]'
echo ""

# 測試工作流程
echo "7. 測試工作流程列表..."
curl -s "$BASE_URL/workflows" | jq 'map({id, name, status}) | .[0:2]'
echo ""

echo "✅ API 測試完成！"