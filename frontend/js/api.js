/*
 * File này được giữ lại để tương thích với các trang cũ.
 * API helper chính hiện nằm trong config.js: apiGet, apiPost, apiPut, apiDelete.
 */
class DrugWarningApi {
    static getMedicines(keyword = "") {
        const query = keyword ? `?keyword=${encodeURIComponent(keyword)}` : "";
        return apiGet(`/Medicines${query}`);
    }

    static getDiseases(keyword = "") {
        const query = keyword ? `?keyword=${encodeURIComponent(keyword)}` : "";
        return apiGet(`/Diseases${query}`);
    }

    static searchWarnings(medicineName, diseaseName) {
        return apiGet(
            `/Warnings/search?medicineName=${encodeURIComponent(medicineName)}&diseaseName=${encodeURIComponent(diseaseName)}`
        );
    }
}
