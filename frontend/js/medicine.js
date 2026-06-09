// ================================
// Drug Warning System
// medicine.js
// ================================

function searchMedicine() {

    const keyword =
        document
            .getElementById("searchInput")
            .value
            .trim()
            .toLowerCase();

    const disease =
        document
            .getElementById("diseaseSelect")
            .value;

    if (keyword === "") {

        alert("Vui lòng nhập tên thuốc");

        return;
    }

    if (disease === "") {

        alert("Vui lòng chọn bệnh nền");

        return;
    }

    const result =
        medicines.find(
            medicine =>
                medicine.name
                    .toLowerCase()
                    .includes(keyword)
        );

    if (result) {

        localStorage.setItem(
            "medicine",
            JSON.stringify(result)
        );

        localStorage.setItem(
            "selectedDisease",
            disease
        );

        window.location.href =
            "warning-result.html";

    }
    else {

        alert(
            "Không tìm thấy thuốc"
        );

    }
}

// ================================
// Hiển thị danh sách thuốc
// ================================

function getAllMedicines() {

    return medicines;

}

// ================================
// Tìm thuốc theo ID
// ================================

function getMedicineById(id) {

    return medicines.find(
        medicine => medicine.id === id
    );

}

// ================================
// Lưu thuốc đang chọn
// ================================

function saveSelectedMedicine(id) {

    const medicine =
        getMedicineById(id);

    if (medicine) {

        localStorage.setItem(
            "medicine",
            JSON.stringify(medicine)
        );

    }

}

// ================================
// Lấy thuốc từ localStorage
// ================================

function getSelectedMedicine() {

    const medicine =
        localStorage.getItem("medicine");

    if (!medicine) {

        return null;

    }

    return JSON.parse(medicine);

}

// ================================
// Xóa thuốc đã chọn
// ================================

function clearSelectedMedicine() {

    localStorage.removeItem(
        "medicine"
    );

}

// ================================
// Hiển thị thuốc phổ biến
// (có thể dùng sau này)
// ================================

function getPopularMedicines() {

    return medicines.slice(0, 3);

}

// ================================
// Debug dữ liệu
// ================================

console.log(
    "Medicine Data Loaded:",
    medicines
);