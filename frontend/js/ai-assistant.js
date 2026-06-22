function riskBadge(level) {
    const map = {
        Critical: 'danger',
        High: 'danger',
        Medium: 'warning text-dark',
        Low: 'success',
        'No warning found': 'secondary',
        Unknown: 'secondary'
    };
    return `<span class="badge bg-${map[level] || 'secondary'} p-2">${level}</span>`;
}

async function analyzeByAi() {
    const box = document.getElementById('aiResult');
    const body = {
        medicinesText: document.getElementById('medicinesText').value.trim(),
        diseasesText: document.getElementById('diseasesText').value.trim(),
        ageGroup: document.getElementById('ageGroup').value,
        note: document.getElementById('note').value.trim()
    };

    if (!body.medicinesText || !body.diseasesText) {
        alert('Vui lòng nhập thuốc và bệnh nền.');
        return;
    }

    box.innerHTML = '<div class="alert alert-info">AI đang phân tích dữ liệu...</div>';

    try {
        const result = await apiPost('/AiAdvisor/analyze', body);
        const risksHtml = result.risks.length === 0
            ? '<div class="alert alert-secondary">Không tìm thấy cảnh báo trực tiếp trong cơ sở dữ liệu.</div>'
            : result.risks.map(r => `
                <div class="border rounded p-3 mb-3">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <strong>${r.medicineName} + ${r.diseaseName}</strong>
                        ${riskBadge(r.riskLevel)}
                    </div>
                    <p><strong>Cảnh báo:</strong> ${r.warningContent}</p>
                    <p><strong>Khuyến nghị:</strong> ${r.recommendation}</p>
                </div>
            `).join('');

        box.innerHTML = `
            <div class="card border-0 bg-light p-3">
                <h4>Kết quả AI</h4>
                <p>${result.summary}</p>
                <p>Mức rủi ro cao nhất: ${riskBadge(result.highestRiskLevel)}</p>
                <hr>
                <h5>Thuốc AI nhận diện</h5>
                <p>${result.detectedMedicines.map(x => x.name).join(', ') || 'Chưa nhận diện'}</p>
                <h5>Bệnh nền AI nhận diện</h5>
                <p>${result.detectedDiseases.map(x => x.name).join(', ') || 'Chưa nhận diện'}</p>
                ${result.unknownMedicines.length ? `<div class="alert alert-warning">Thuốc chưa có dữ liệu: ${result.unknownMedicines.join(', ')}</div>` : ''}
                ${result.unknownDiseases.length ? `<div class="alert alert-warning">Bệnh nền chưa có dữ liệu: ${result.unknownDiseases.join(', ')}</div>` : ''}
                <h5>Chi tiết rủi ro</h5>
                ${risksHtml}
                <h5>Gợi ý của AI</h5>
                <ul>${result.aiSuggestions.map(s => `<li>${s}</li>`).join('')}</ul>
                <div class="alert alert-warning">${result.medicalDisclaimer}</div>
            </div>`;
    } catch (error) {
        box.innerHTML = `<div class="alert alert-danger">Không kết nối được AI API. Hãy chạy backend tại http://localhost:5000</div>`;
    }
}
