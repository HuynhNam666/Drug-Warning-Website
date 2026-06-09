/* ==========================
auth.js
========================== */

const ADMIN = {
    username: "admin",
    password: "123456",
    role: "admin"
};

/* ==========================
ĐĂNG KÝ
========================== */

function register() {
    const username =
        document.getElementById("username").value.trim();

    const email =
        document.getElementById("email").value.trim();

    const password =
        document.getElementById("password").value.trim();

    if (
        username === "" ||
        email === "" ||
        password === ""
    ) {

        alert("Vui lòng nhập đầy đủ thông tin");

        return;
    }

    let users =
        JSON.parse(
            localStorage.getItem("users")
        ) || [];

    const existUser =
        users.find(
            user =>
                user.username === username
        );

    if (existUser) {

        alert("Tên đăng nhập đã tồn tại");

        return;
    }

    const newUser = {

        username: username,
        email: email,
        password: password,
        role: "user"

    };

    users.push(newUser);

    localStorage.setItem(
        "users",
        JSON.stringify(users)
    );

    alert("Đăng ký thành công");

    window.location.href =
        "login.html";

}

/* ==========================
ĐĂNG NHẬP
========================== */

function login() {
    const username =
        document.getElementById("username").value.trim();

    const password =
        document.getElementById("password").value.trim();

    if (
        username === ADMIN.username &&
        password === ADMIN.password
    ) {

        localStorage.setItem(
            "currentUser",
            JSON.stringify(ADMIN)
        );

        window.location.href =
            "admin-dashboard.html";

        return;
    }

    const users =
        JSON.parse(
            localStorage.getItem("users")
        ) || [];

    const user =
        users.find(

            u =>
                u.username === username &&
                u.password === password

        );

    if (user) {

        localStorage.setItem(
            "currentUser",
            JSON.stringify(user)
        );

        window.location.href =
            "index.html";

    } else {

        alert(
            "Sai tài khoản hoặc mật khẩu"
        );

    }


}

/* ==========================
ĐĂNG XUẤT
========================== */

function logout() {

    localStorage.removeItem(
        "currentUser"
    );

    window.location.href =
        "login.html";


}

/* ==========================
LẤY USER HIỆN TẠI
========================== */

function getCurrentUser() {

    return JSON.parse(
        localStorage.getItem(
            "currentUser"
        )
    );


}

/* ==========================
KIỂM TRA ADMIN
========================== */

function checkAdmin() {
    const currentUser =
        getCurrentUser();

    if (
        !currentUser ||
        currentUser.role !== "admin"
    ) {

        alert(
            "Bạn không có quyền truy cập"
        );

        window.location.href =
            "login.html";

    }

}

/* ==========================
HIỂN THỊ TÊN USER
========================== */

function showUserInfo() {

    const currentUser =
        getCurrentUser();

    const userInfo =
        document.getElementById(
            "userInfo"
        );

    if (
        currentUser &&
        userInfo
    ) {

        userInfo.innerHTML =
            `Xin chào, ${currentUser.username} `;

    }
}
