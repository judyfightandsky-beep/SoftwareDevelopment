document.addEventListener('DOMContentLoaded', function() {
    // 側邊欄控制
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.sidebar');
    const mainContent = document.querySelector('.main-content');

    // 導航選單項目
    const navItems = document.querySelectorAll('.nav-item');

    // 內容區域
    const contentSections = document.querySelectorAll('.content-section');

    // 使用者列表和分頁
    const userTableBody = document.querySelector('.user-table tbody');
    const paginationContainer = document.querySelector('.pagination');

    // 模態框
    const userModal = document.getElementById('userModal');
    const modalOverlay = document.getElementById('modalOverlay');
    const closeModalButtons = document.querySelectorAll('.close-modal');

    // 表單元素
    const userForm = document.getElementById('userForm');
    const modalTitle = document.getElementById('modalTitle');

    // 搜尋和篩選
    const searchInput = document.querySelector('.search-input');
    const roleFilter = document.querySelector('.filter-select');

    // 使用者資料
    let users = [
        {
            id: 1,
            username: 'johndoe',
            email: 'john@example.com',
            role: 'User',
            status: 'Active',
            lastLogin: '2024-01-20 10:30',
            createdAt: '2024-01-01'
        },
        {
            id: 2,
            username: 'janesmit',
            email: 'jane@example.com',
            role: 'Admin',
            status: 'Active',
            lastLogin: '2024-01-20 09:15',
            createdAt: '2024-01-02'
        },
        {
            id: 3,
            username: 'bobwilson',
            email: 'bob@example.com',
            role: 'User',
            status: 'Inactive',
            lastLogin: '2024-01-19 16:45',
            createdAt: '2024-01-03'
        },
        {
            id: 4,
            username: 'alicejohnson',
            email: 'alice@example.com',
            role: 'User',
            status: 'Active',
            lastLogin: '2024-01-20 11:20',
            createdAt: '2024-01-04'
        },
        {
            id: 5,
            username: 'charliebrwn',
            email: 'charlie@example.com',
            role: 'Moderator',
            status: 'Active',
            lastLogin: '2024-01-20 08:30',
            createdAt: '2024-01-05'
        }
    ];

    let filteredUsers = [...users];
    let currentPage = 1;
    const itemsPerPage = 10;

    // 初始化
    init();

    function init() {
        setupEventListeners();
        updateStatistics();
        renderUserTable();
        setupPagination();
    }

    // 設定事件監聽器
    function setupEventListeners() {
        // 側邊欄切換
        sidebarToggle.addEventListener('click', toggleSidebar);

        // 導航選單
        navItems.forEach(item => {
            item.addEventListener('click', handleNavigation);
        });

        // 模態框控制
        closeModalButtons.forEach(button => {
            button.addEventListener('click', closeModal);
        });

        modalOverlay.addEventListener('click', closeModal);

        // 使用者表單
        userForm.addEventListener('submit', handleUserSubmit);

        // 搜尋和篩選
        searchInput.addEventListener('input', debounce(handleSearch, 300));
        roleFilter.addEventListener('change', handleRoleFilter);

        // 新增使用者按鈕
        const addUserButton = document.querySelector('.add-user-btn');
        if (addUserButton) {
            addUserButton.addEventListener('click', () => openUserModal());
        }
    }

    // 側邊欄切換
    function toggleSidebar() {
        sidebar.classList.toggle('collapsed');
        mainContent.classList.toggle('expanded');
    }

    // 導航處理
    function handleNavigation(e) {
        e.preventDefault();
        const targetSection = this.dataset.section;

        // 移除所有 active 狀態
        navItems.forEach(item => item.classList.remove('active'));
        contentSections.forEach(section => section.classList.remove('active'));

        // 添加 active 狀態
        this.classList.add('active');
        const targetElement = document.getElementById(targetSection);
        if (targetElement) {
            targetElement.classList.add('active');
        }

        // 如果是使用者管理，重新渲染表格
        if (targetSection === 'users') {
            renderUserTable();
        }
    }

    // 更新統計資料
    function updateStatistics() {
        const totalUsers = users.length;
        const activeUsers = users.filter(user => user.status === 'Active').length;
        const adminUsers = users.filter(user => user.role === 'Admin' || user.role === 'Moderator').length;

        // 更新統計卡片
        const statsCards = document.querySelectorAll('.stat-card');
        if (statsCards[0]) {
            statsCards[0].querySelector('.stat-number').textContent = totalUsers;
        }
        if (statsCards[1]) {
            statsCards[1].querySelector('.stat-number').textContent = activeUsers;
        }
        if (statsCards[2]) {
            statsCards[2].querySelector('.stat-number').textContent = adminUsers;
        }
    }

    // 渲染使用者表格
    function renderUserTable() {
        if (!userTableBody) return;

        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        const pageUsers = filteredUsers.slice(startIndex, endIndex);

        userTableBody.innerHTML = pageUsers.map(user => `
            <tr data-user-id="${user.id}">
                <td>
                    <div class="user-info">
                        <div class="user-avatar">
                            ${user.username.charAt(0).toUpperCase()}
                        </div>
                        <div>
                            <div class="user-name">${user.username}</div>
                            <div class="user-email">${user.email}</div>
                        </div>
                    </div>
                </td>
                <td>
                    <span class="role-badge role-${user.role.toLowerCase()}">${user.role}</span>
                </td>
                <td>
                    <span class="status-badge status-${user.status.toLowerCase()}">${user.status}</span>
                </td>
                <td>${user.lastLogin}</td>
                <td>
                    <div class="action-buttons">
                        <button class="action-btn edit-btn" onclick="editUser(${user.id})">
                            <svg viewBox="0 0 20 20">
                                <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"/>
                            </svg>
                        </button>
                        <button class="action-btn delete-btn" onclick="deleteUser(${user.id})">
                            <svg viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9zM4 5a2 2 0 012-2v1a3 3 0 003 3h2a3 3 0 003-3V3a2 2 0 012 2v1H4V5zM3 8h14v8a2 2 0 01-2 2H5a2 2 0 01-2-2V8z" clip-rule="evenodd"/>
                            </svg>
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');

        updatePagination();
    }

    // 設定分頁
    function setupPagination() {
        updatePagination();
    }

    // 更新分頁
    function updatePagination() {
        if (!paginationContainer) return;

        const totalPages = Math.ceil(filteredUsers.length / itemsPerPage);

        let paginationHTML = `
            <button class="pagination-btn ${currentPage === 1 ? 'disabled' : ''}"
                    onclick="changePage(${currentPage - 1})" ${currentPage === 1 ? 'disabled' : ''}>
                <svg viewBox="0 0 20 20">
                    <path d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z"/>
                </svg>
            </button>
        `;

        // 頁碼按鈕
        for (let i = 1; i <= totalPages; i++) {
            if (i === currentPage || i === 1 || i === totalPages || Math.abs(i - currentPage) <= 1) {
                paginationHTML += `
                    <button class="pagination-btn ${i === currentPage ? 'active' : ''}"
                            onclick="changePage(${i})">${i}</button>
                `;
            } else if (i === currentPage - 2 || i === currentPage + 2) {
                paginationHTML += `<span class="pagination-ellipsis">...</span>`;
            }
        }

        paginationHTML += `
            <button class="pagination-btn ${currentPage === totalPages ? 'disabled' : ''}"
                    onclick="changePage(${currentPage + 1})" ${currentPage === totalPages ? 'disabled' : ''}>
                <svg viewBox="0 0 20 20">
                    <path d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"/>
                </svg>
            </button>
        `;

        paginationContainer.innerHTML = paginationHTML;
    }

    // 搜尋處理
    function handleSearch() {
        const searchTerm = searchInput.value.toLowerCase().trim();
        filteredUsers = users.filter(user =>
            user.username.toLowerCase().includes(searchTerm) ||
            user.email.toLowerCase().includes(searchTerm)
        );

        // 套用角色篩選
        const selectedRole = roleFilter.value;
        if (selectedRole) {
            filteredUsers = filteredUsers.filter(user => user.role === selectedRole);
        }

        currentPage = 1;
        renderUserTable();
    }

    // 角色篩選處理
    function handleRoleFilter() {
        handleSearch(); // 重新執行搜尋以套用篩選
    }

    // 開啟使用者模態框
    function openUserModal(user = null) {
        if (user) {
            modalTitle.textContent = '編輯使用者';
            // 填入現有資料
            document.getElementById('modalUsername').value = user.username;
            document.getElementById('modalEmail').value = user.email;
            document.getElementById('modalRole').value = user.role;
            document.getElementById('modalStatus').value = user.status;
            userForm.dataset.userId = user.id;
        } else {
            modalTitle.textContent = '新增使用者';
            userForm.reset();
            delete userForm.dataset.userId;
        }

        userModal.classList.add('active');
        modalOverlay.classList.add('active');
    }

    // 關閉模態框
    function closeModal() {
        userModal.classList.remove('active');
        modalOverlay.classList.remove('active');
    }

    // 使用者表單提交
    function handleUserSubmit(e) {
        e.preventDefault();

        const formData = new FormData(userForm);
        const userData = {
            username: formData.get('username'),
            email: formData.get('email'),
            role: formData.get('role'),
            status: formData.get('status')
        };

        const userId = userForm.dataset.userId;

        if (userId) {
            // 編輯使用者
            const userIndex = users.findIndex(user => user.id == userId);
            if (userIndex !== -1) {
                users[userIndex] = { ...users[userIndex], ...userData };
                showMessage('使用者資料已更新', 'success');
            }
        } else {
            // 新增使用者
            const newUser = {
                id: users.length + 1,
                ...userData,
                lastLogin: '從未登入',
                createdAt: new Date().toISOString().split('T')[0]
            };
            users.push(newUser);
            showMessage('使用者已成功新增', 'success');
        }

        closeModal();
        updateStatistics();
        handleSearch(); // 重新渲染表格
    }

    // 防抖函數
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    // 顯示訊息
    function showMessage(message, type = 'info') {
        const messageElement = document.createElement('div');
        messageElement.className = `message message-${type}`;
        messageElement.textContent = message;

        document.body.appendChild(messageElement);

        setTimeout(() => {
            messageElement.remove();
        }, 3000);
    }

    // 全域函數（供HTML調用）
    window.changePage = function(page) {
        const totalPages = Math.ceil(filteredUsers.length / itemsPerPage);
        if (page >= 1 && page <= totalPages) {
            currentPage = page;
            renderUserTable();
        }
    };

    window.editUser = function(userId) {
        const user = users.find(u => u.id === userId);
        if (user) {
            openUserModal(user);
        }
    };

    window.deleteUser = function(userId) {
        if (confirm('確定要刪除此使用者嗎？')) {
            const userIndex = users.findIndex(u => u.id === userId);
            if (userIndex !== -1) {
                users.splice(userIndex, 1);
                filteredUsers = filteredUsers.filter(u => u.id !== userId);
                updateStatistics();
                renderUserTable();
                showMessage('使用者已刪除', 'success');
            }
        }
    };
});