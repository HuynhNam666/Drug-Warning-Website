// Các hàm tra cứu thuốc dùng API thật thay cho dữ liệu tĩnh.
async function getAllMedicines(keyword = "") {
    return DrugWarningApi.getMedicines(keyword);
}

async function getMedicineById(id) {
    return apiGet(`/Medicines/${id}`);
}

function saveSelectedMedicine(medicine) {
    if (medicine) {
        localStorage.setItem("medicine", JSON.stringify(medicine));
    }
}

function getSelectedMedicine() {
    const medicine = localStorage.getItem("medicine");
    return medicine ? JSON.parse(medicine) : null;
}

function clearSelectedMedicine() {
    localStorage.removeItem("medicine");
}

async function getPopularMedicines() {
    const medicines = await getAllMedicines();
    return medicines.slice(0, 3);
}
