const jsonServer = require('json-server');
const express = require('express');
const cors = require('cors');
const jwt = require('jsonwebtoken');
const { v4: uuidv4 } = require('uuid');
const fs = require('fs');

// 創建應用
const app = express();
const PORT = process.env.PORT || 3010;
const JWT_SECRET = 'your-secret-key-change-in-production';

// 讀取數據庫
const getDb = () => JSON.parse(fs.readFileSync('./db.json', 'utf8'));
const saveDb = (db) => fs.writeFileSync('./db.json', JSON.stringify(db, null, 2));

// 中間件
app.use(cors({
  origin: ['http://localhost:3000', 'http://localhost:5173', 'http://127.0.0.1:3000', 'http://localhost:3010'],
  credentials: true
}));
app.use(express.json());

// JWT 驗證中間件
function verifyToken(req, res, next) {
  const authHeader = req.headers.authorization;
  const token = authHeader && authHeader.split(' ')[1];

  if (!token) {
    return res.status(401).json({
      success: false,
      message: '缺少認證令牌'
    });
  }

  try {
    const decoded = jwt.verify(token, JWT_SECRET);
    req.user = decoded;
    next();
  } catch (error) {
    return res.status(401).json({
      success: false,
      message: '無效的認證令牌'
    });
  }
}

// 認證路由
app.post('/api/v2/auth/login', (req, res) => {
  const { email, password } = req.body;
  const db = getDb();
  const user = db.users.find(u => u.email === email);

  if (!user || password !== 'password123') {
    return res.status(401).json({
      success: false,
      message: '用戶名或密碼錯誤'
    });
  }

  const token = jwt.sign(
    { userId: user.id, email: user.email, role: user.role },
    JWT_SECRET,
    { expiresIn: '24h' }
  );

  const { password: _, ...userWithoutPassword } = user;

  res.json({
    success: true,
    data: { token, user: userWithoutPassword }
  });
});

app.post('/api/v2/auth/refresh', verifyToken, (req, res) => {
  const db = getDb();
  const user = db.users.find(u => u.id === req.user.userId);

  if (!user) {
    return res.status(404).json({ success: false, message: '用戶不存在' });
  }

  const token = jwt.sign(
    { userId: user.id, email: user.email, role: user.role },
    JWT_SECRET,
    { expiresIn: '24h' }
  );

  const { password: _, ...userWithoutPassword } = user;

  res.json({
    success: true,
    data: { token, user: userWithoutPassword }
  });
});

app.post('/api/v2/auth/register', (req, res) => {
  const { username, email, firstName, lastName, password, confirmPassword } = req.body;

  if (password !== confirmPassword) {
    return res.status(400).json({
      success: false,
      message: '密碼確認不匹配'
    });
  }

  const db = getDb();
  const existingUser = db.users.find(u => u.email === email || u.name === username);

  if (existingUser) {
    return res.status(409).json({
      success: false,
      message: '用戶已存在'
    });
  }

  const newUser = {
    id: uuidv4(),
    name: username,
    email,
    firstName,
    lastName,
    role: 'Developer',
    status: 'Active',
    password: '$2a$10$K8f5QnWg.uFqAGO4dE7C6O4gX7HLhE7.4BxFqgGZ8oCXd1pD3jLTW',
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString()
  };

  db.users.push(newUser);
  saveDb(db);

  const { password: _, ...userResponse } = newUser;

  res.status(201).json({
    success: true,
    data: userResponse
  });
});

app.get('/api/v2/auth/check-availability', (req, res) => {
  const { username, email } = req.query;
  const db = getDb();

  let available = true;
  let message = '可用';

  if (username && db.users.find(u => u.name === username)) {
    available = false;
    message = '用戶名已被使用';
  }

  if (email && db.users.find(u => u.email === email)) {
    available = false;
    message = '郵箱已被使用';
  }

  res.json({ available, message });
});

// 項目模板路由
app.get('/api/v2/templates', (req, res) => {
  const db = getDb();
  res.json(db.templates);
});

app.get('/api/v2/templates/popular', (req, res) => {
  const db = getDb();
  const popularTemplates = db.templates
    .sort((a, b) => b.metadata.downloadCount - a.metadata.downloadCount)
    .slice(0, 5);
  res.json(popularTemplates);
});

app.get('/api/v2/templates/:id', (req, res) => {
  const db = getDb();
  const template = db.templates.find(t => t.id === req.params.id);

  if (!template) {
    return res.status(404).json({ success: false, message: '模板不存在' });
  }

  res.json(template);
});

app.post('/api/v2/templates', verifyToken, (req, res) => {
  const db = getDb();
  const newTemplate = {
    id: uuidv4(),
    ...req.body,
    status: 'Draft',
    metadata: {
      downloadCount: 0,
      averageRating: 0,
      lastUsed: new Date().toISOString(),
      tags: req.body.tags || []
    },
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    createdBy: req.user.userId,
    updatedBy: req.user.userId
  };

  db.templates.push(newTemplate);
  saveDb(db);

  res.status(201).json(newTemplate);
});

app.post('/api/v2/templates/:id/generate', verifyToken, (req, res) => {
  const config = req.body;

  // 模擬項目生成過程
  setTimeout(() => {
    const projectId = uuidv4();
    const repositoryUrl = `https://github.com/generated/${config.projectName.toLowerCase().replace(/\s+/g, '-')}`;
    res.json({ projectId, repositoryUrl });
  }, 2000);
});

// 任務管理路由
app.get('/api/v2/tasks', verifyToken, (req, res) => {
  const { projectId, status, assignedTo } = req.query;
  const db = getDb();
  let tasks = db.tasks;

  if (projectId) tasks = tasks.filter(task => task.projectId === projectId);
  if (status) tasks = tasks.filter(task => task.status === status);
  if (assignedTo) tasks = tasks.filter(task => task.assignedTo === assignedTo);

  res.json(tasks);
});

app.post('/api/v2/tasks', verifyToken, (req, res) => {
  const db = getDb();
  const newTask = {
    id: uuidv4(),
    taskNumber: `TASK-${String(db.tasks.length + 1).padStart(3, '0')}`,
    ...req.body,
    createdBy: req.user.userId,
    createdAt: new Date().toISOString(),
    status: 'Todo',
    progress: 0
  };

  db.tasks.push(newTask);
  saveDb(db);

  res.status(201).json(newTask);
});

app.put('/api/v2/tasks/:id', verifyToken, (req, res) => {
  const db = getDb();
  const taskIndex = db.tasks.findIndex(t => t.id === req.params.id);

  if (taskIndex === -1) {
    return res.status(404).json({ success: false, message: '任務不存在' });
  }

  db.tasks[taskIndex] = {
    ...db.tasks[taskIndex],
    ...req.body,
    updatedAt: new Date().toISOString()
  };

  saveDb(db);
  res.json(db.tasks[taskIndex]);
});

// 代碼品質檢查路由
app.get('/api/v2/quality-checks', verifyToken, (req, res) => {
  const { projectId } = req.query;
  const db = getDb();
  let checks = db.qualityChecks;

  if (projectId) checks = checks.filter(check => check.projectId === projectId);

  res.json(checks);
});

app.post('/api/v2/quality-checks', verifyToken, (req, res) => {
  const db = getDb();
  const newCheck = {
    id: uuidv4(),
    ...req.body,
    status: 'Running',
    startedAt: new Date().toISOString(),
    requestedBy: req.user.userId
  };

  db.qualityChecks.push(newCheck);
  saveDb(db);

  // 模擬檢查過程
  setTimeout(() => {
    const updatedDb = getDb();
    const checkIndex = updatedDb.qualityChecks.findIndex(c => c.id === newCheck.id);

    if (checkIndex !== -1) {
      updatedDb.qualityChecks[checkIndex] = {
        ...updatedDb.qualityChecks[checkIndex],
        status: 'Completed',
        completedAt: new Date().toISOString(),
        overallScore: {
          score: Math.floor(Math.random() * 30) + 70,
          codeStyle: Math.floor(Math.random() * 30) + 70,
          complexity: Math.floor(Math.random() * 30) + 70,
          security: Math.floor(Math.random() * 30) + 70,
          performance: Math.floor(Math.random() * 30) + 70,
          testCoverage: Math.floor(Math.random() * 30) + 70,
          grade: ['A', 'B', 'C'][Math.floor(Math.random() * 3)]
        },
        issues: [],
        metrics: [
          {
            type: 'coverage',
            name: '代碼覆蓋率',
            value: Math.floor(Math.random() * 30) + 70,
            unit: '%',
            trend: 'Improving'
          }
        ]
      };

      saveDb(updatedDb);
    }
  }, 5000);

  res.status(201).json(newCheck);
});

// 工作流程路由
app.get('/api/v2/workflows', (req, res) => {
  const db = getDb();
  res.json(db.workflows);
});

app.get('/api/v2/workflow-instances', verifyToken, (req, res) => {
  const { workflowDefinitionId, status } = req.query;
  const db = getDb();
  let instances = db.workflowInstances;

  if (workflowDefinitionId) instances = instances.filter(i => i.workflowDefinitionId === workflowDefinitionId);
  if (status) instances = instances.filter(i => i.status === status);

  res.json(instances);
});

app.post('/api/v2/workflows/:id/start', verifyToken, (req, res) => {
  const db = getDb();
  const workflow = db.workflows.find(w => w.id === req.params.id);

  if (!workflow) {
    return res.status(404).json({ success: false, message: '工作流程不存在' });
  }

  const newInstance = {
    id: uuidv4(),
    workflowDefinitionId: req.params.id,
    ...req.body,
    status: 'Running',
    nodeExecutions: [],
    startedAt: new Date().toISOString(),
    startedBy: req.user.userId
  };

  db.workflowInstances.push(newInstance);
  saveDb(db);

  res.status(201).json(newInstance);
});

// 健康檢查
app.get('/api/v2/health', (req, res) => {
  res.json({
    status: 'healthy',
    timestamp: new Date().toISOString(),
    version: '1.0.0'
  });
});

// 啟動服務器
app.listen(PORT, () => {
  console.log(`🚀 Mock Server is running on http://localhost:${PORT}`);
  console.log(`📊 API endpoints available at http://localhost:${PORT}/api/v2`);
  console.log(`📖 Resources: users, templates, projects, tasks, qualityChecks, workflows, workflowInstances`);
  console.log(`🔐 Use email: ming.zhang@example.com, password: password123 to login`);
  console.log(`🏥 Health check: http://localhost:${PORT}/api/v2/health`);
});