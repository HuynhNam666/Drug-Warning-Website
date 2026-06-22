async function getWarningResult(medicineName, diseaseName) {
    return DrugWarningApi.searchWarnings(medicineName, diseaseName);
}

function renderWarningResult(container, result, medicineName, diseaseName) {
    if (!container) return;

    if (!result.found) {
        container.innerHTML = `
            <h5>Thuốc: ${medicineName}</h5>
            <h5>Bệnh nền: ${diseaseName}</h5>
            <div class="alert alert-secondary mt-3">${result.message}</div>`;
        return;
    }

    container.innerHTML = result.warnings.map(warning => `
        <div class="border rounded p-3 mb-3">
            <p><strong>Thuốc:</strong> ${warning.medicineName}</p>
            <p><strong>Bệnh nền:</strong> ${warning.diseaseName}</p>
            <p><strong>Mức độ:</strong> ${warning.riskLevel}</p>
            <p><strong>Cảnh báo:</strong> ${warning.warningContent}</p>
            <p><strong>Khuyến nghị:</strong> ${warning.recommendation || "Hỏi bác sĩ/dược sĩ trước khi dùng."}</p>
        </div>`).join("");
}
