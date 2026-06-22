/* ==========================
   auth.js - đăng nhập admin bằng JWT API
========================== */

async function login() {
    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();
    const errorBox = document.getElementById("loginError");

    if (!username || !password) {
        showLoginError("Vui lòng nhập tài khoản và mật khẩu.");
        return;
    }

    try {
        const result = await apiPost("/Auth/login", { username, password });

        localStorage.setItem("currentUser", JSON.stringify({
            username: result.username,
            fullName: result.fullName || result.username,
            token: result.token,
            expiresAt: result.expiresAt,
            role: "admin"
        }));

        window.location.href = "admin-dashboard.html";
    } catch (error) {
        showLoginError("Sai tài khoản, mật khẩu hoặc backend chưa chạy tại http://localhost:5000.");
        console.error(error);
    }

    function showLoginError(message) {
        if (errorBox) {
            errorBox.classList.remove("d-none");
            errorBox.innerText = message;
        } else {
            alert(message);
        }
    }
}

function logout() {
    localStorage.removeItem("currentUser");
    window.location.href = "login.html";
}

function getCurrentUser() {
    return typeof getCurrentUserFromStorage === "function"
        ? getCurrentUserFromStorage()
        : JSON.parse(localStorage.getItem("currentUser") || "null");
}

function checkAdmin() {
    const currentUser = getCurrentUser();

    if (!currentUser || currentUser.role !== "admin" || !currentUser.token) {
        alert("Bạn cần đăng nhập admin để truy cập trang này.");
        window.location.href = "login.html";
        return false;
    }

    return true;
}

function showUserInfo() {
    const currentUser = getCurrentUser();
    const userInfo = document.getElementById("userInfo");

    if (currentUser && userInfo) {
        userInfo.innerHTML = `Xin chào, ${currentUser.fullName || currentUser.username}`;
    }
}

function register() {
    alert("Dự án hiện chỉ hỗ trợ tài khoản quản trị mẫu. Vui lòng đăng nhập: admin / 123456.");
    window.location.href = "login.html";
}
