let medicines = [];
let diseases = [];
let warnings = [];

function showAdminAlert(message, type = "success") {
    const box = document.getElementById("adminAlert");

    if (!box) {
        alert(message);
        return;
    }

    box.innerHTML = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${escapeHtml(message)}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>`;
}

function escapeHtml(value) {
    return String(value || "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}

function riskBadge(level) {
    const map = {
        Critical: "danger",
        High: "danger",
        Medium: "warning text-dark",
        Low: "success"
    };

    return `<span class="badge bg-${map[level] || "secondary"}">${escapeHtml(level)}</span>`;
}

function clearInputs(ids) {
    ids.forEach(id => {
        const element = document.getElementById(id);
        if (element) element.value = "";
    });
}

class AdminResourceManager {
    constructor(options) {
        this.endpoint = options.endpoint;
        this.tableId = options.tableId;
        this.emptyColspan = options.emptyColspan;
        this.emptyText = options.emptyText;
        this.deleteConfirm = options.deleteConfirm;
        this.deleteSuccess = options.deleteSuccess;
    }

    getItems() {
        return [];
    }

    renderRow() {
        throw new Error("renderRow() phải được cài đặt ở lớp con.");
    }

    render() {
        const table = document.getElementById(this.tableId);
        if (!table) return;

        const items = this.getItems();
        table.innerHTML = items.length
            ? items.map(item => this.renderRow(item)).join("")
            : `<tr><td colspan="${this.emptyColspan}" class="text-center text-muted">${this.emptyText}</td></tr>`;
    }

    async create(body, resetInputIds, successMessage) {
        await apiPost(this.endpoint, body);
        clearInputs(resetInputIds);
        showAdminAlert(successMessage);
        await loadAdminData();
    }

    async delete(id) {
        if (!confirm(this.deleteConfirm)) return;

        await apiDelete(`${this.endpoint}/${id}`);
        showAdminAlert(this.deleteSuccess);
        await loadAdminData();
    }
}

class MedicineManager extends AdminResourceManager {
    constructor() {
        super({
            endpoint: "/Medicines",
            tableId: "medicineTable",
            emptyColspan: 5,
            emptyText: "Chưa có dữ liệu thuốc",
            deleteConfirm: "Xóa thuốc này sẽ xóa cả các cảnh báo liên quan. Bạn chắc chắn chứ?",
            deleteSuccess: "Đã xóa thuốc."
        });
    }

    getItems() {
        return medicines;
    }

    renderRow(medicine) {
        return `
            <tr>
                <td>${medicine.id}</td>
                <td>${escapeHtml(medicine.name)}</td>
                <td>${escapeHtml(medicine.usage)}</td>
                <td>${escapeHtml(medicine.sideEffects)}</td>
                <td><button class="btn btn-danger btn-sm" onclick="deleteMedicine(${medicine.id})">Xóa</button></td>
            </tr>`;
    }

    async add() {
        const body = {
            name: document.getElementById("medicineName").value.trim(),
            description: document.getElementById("medicineDescription").value.trim(),
            usage: document.getElementById("medicineUsage").value.trim(),
            sideEffects: document.getElementById("medicineSideEffects").value.trim()
        };

        if (!body.name) {
            showAdminAlert("Vui lòng nhập tên thuốc.", "warning");
            return;
        }

        await this.create(
            body,
            ["medicineName", "medicineDescription", "medicineUsage", "medicineSideEffects"],
            "Đã thêm thuốc mới."
        );
    }
}

class DiseaseManager extends AdminResourceManager {
    constructor() {
        super({
            endpoint: "/Diseases",
            tableId: "diseaseTable",
            emptyColspan: 4,
            emptyText: "Chưa có dữ liệu bệnh nền",
            deleteConfirm: "Xóa bệnh nền này sẽ xóa cả các cảnh báo liên quan. Bạn chắc chắn chứ?",
            deleteSuccess: "Đã xóa bệnh nền."
        });
    }

    getItems() {
        return diseases;
    }

    renderRow(disease) {
        return `
            <tr>
                <td>${disease.id}</td>
                <td>${escapeHtml(disease.name)}</td>
                <td>${escapeHtml(disease.description)}</td>
                <td><button class="btn btn-danger btn-sm" onclick="deleteDisease(${disease.id})">Xóa</button></td>
            </tr>`;
    }

    async add() {
        const body = {
            name: document.getElementById("diseaseName").value.trim(),
            description: document.getElementById("diseaseDescription").value.trim()
        };

        if (!body.name) {
            showAdminAlert("Vui lòng nhập tên bệnh nền.", "warning");
            return;
        }

        await this.create(
            body,
            ["diseaseName", "diseaseDescription"],
            "Đã thêm bệnh nền mới."
        );
    }
}

class WarningManager extends AdminResourceManager {
    constructor() {
        super({
            endpoint: "/Warnings",
            tableId: "warningTable",
            emptyColspan: 7,
            emptyText: "Chưa có dữ liệu cảnh báo",
            deleteConfirm: "Bạn chắc chắn muốn xóa cảnh báo này?",
            deleteSuccess: "Đã xóa cảnh báo."
        });
    }

    getItems() {
        return warnings;
    }

    renderRow(warning) {
        return `
            <tr>
                <td>${warning.id}</td>
                <td>${escapeHtml(warning.medicineName)}</td>
                <td>${escapeHtml(warning.diseaseName)}</td>
                <td>${riskBadge(warning.riskLevel)}</td>
                <td>${escapeHtml(warning.warningContent)}</td>
                <td>${escapeHtml(warning.recommendation)}</td>
                <td><button class="btn btn-danger btn-sm" onclick="deleteWarning(${warning.id})">Xóa</button></td>
            </tr>`;
    }

    async add() {
        const body = {
            medicineId: Number(document.getElementById("warningMedicineId").value),
            diseaseId: Number(document.getElementById("warningDiseaseId").value),
            riskLevel: document.getElementById("warningRiskLevel").value,
            warningContent: document.getElementById("warningContent").value.trim(),
            recommendation: document.getElementById("warningRecommendation").value.trim()
        };

        if (!body.medicineId || !body.diseaseId || !body.warningContent) {
            showAdminAlert("Vui lòng chọn thuốc, bệnh nền và nhập nội dung cảnh báo.", "warning");
            return;
        }

        await this.create(
            body,
            ["warningContent", "warningRecommendation"],
            "Đã thêm cảnh báo mới."
        );
    }
}

const medicineManager = new MedicineManager();
const diseaseManager = new DiseaseManager();
const warningManager = new WarningManager();

async function loadAdminData() {
    try {
        [medicines, diseases, warnings] = await Promise.all([
            apiGet("/Medicines"),
            apiGet("/Diseases"),
            apiGet("/Warnings")
        ]);

        renderAdminData();
    } catch (error) {
        showAdminAlert("Không tải được dữ liệu. Hãy kiểm tra backend và token đăng nhập.", "danger");
        console.error(error);
    }
}

function renderAdminData() {
    medicineManager.render();
    diseaseManager.render();
    warningManager.render();
    renderWarningSelects();
    updateCounts();
}

function updateCounts() {
    document.getElementById("medicineCount").innerText = medicines.length;
    document.getElementById("diseaseCount").innerText = diseases.length;
    document.getElementById("warningCount").innerText = warnings.length;
}

function renderWarningSelects() {
    const medSelect = document.getElementById("warningMedicineId");
    const disSelect = document.getElementById("warningDiseaseId");

    medSelect.innerHTML = medicines
        .map(medicine => `<option value="${medicine.id}">${escapeHtml(medicine.name)}</option>`)
        .join("");

    disSelect.innerHTML = diseases
        .map(disease => `<option value="${disease.id}">${escapeHtml(disease.name)}</option>`)
        .join("");
}

async function runAdminAction(action) {
    try {
        await action();
    } catch (error) {
        showAdminAlert(error.message, "danger");
        console.error(error);
    }
}

function addMedicine() {
    runAdminAction(() => medicineManager.add());
}

function deleteMedicine(id) {
    runAdminAction(() => medicineManager.delete(id));
}

function addDisease() {
    runAdminAction(() => diseaseManager.add());
}

function deleteDisease(id) {
    runAdminAction(() => diseaseManager.delete(id));
}

function addWarning() {
    runAdminAction(() => warningManager.add());
}

function deleteWarning(id) {
    runAdminAction(() => warningManager.delete(id));
}
