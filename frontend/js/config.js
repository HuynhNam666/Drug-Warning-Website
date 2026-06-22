const API_BASE_URL = "http://localhost:5000/api";

function getCurrentUserFromStorage() {
    return JSON.parse(localStorage.getItem("currentUser") || "null");
}

function getToken() {
    const user = getCurrentUserFromStorage();
    return user?.token || null;
}

function buildHeaders(token = null) {
    const headers = { "Content-Type": "application/json" };
    const authToken = token || getToken();

    if (authToken) {
        headers.Authorization = `Bearer ${authToken}`;
    }

    return headers;
}

async function handleResponse(response) {
    if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Có lỗi xảy ra khi gọi API.");
    }

    if (response.status === 204) {
        return null;
    }

    const contentType = response.headers.get("content-type") || "";
    return contentType.includes("application/json")
        ? response.json()
        : response.text();
}

async function apiRequest(method, path, body = null, token = null) {
    const options = {
        method,
        headers: buildHeaders(token)
    };

    if (body !== null) {
        options.body = JSON.stringify(body);
    }

    const response = await fetch(`${API_BASE_URL}${path}`, options);
    return handleResponse(response);
}

const apiGet = (path, token = null) => apiRequest("GET", path, null, token);
const apiPost = (path, body, token = null) => apiRequest("POST", path, body, token);
const apiPut = (path, body, token = null) => apiRequest("PUT", path, body, token);
const apiDelete = (path, token = null) => apiRequest("DELETE", path, null, token);
