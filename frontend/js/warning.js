// Xử lý cảnh báo thuốc
function showWarning(medicine, disease) {
    const warnings = {
        "aspirin": ["tim", "thận"],
        "metformin": ["thận"]
    };

    if (warnings[medicine] && warnings[medicine].includes(disease)) {
        document.querySelector(".warning-result").innerHTML = `
      <h1>Cảnh báo thuốc</h1>
      <p>Thuốc <strong>${medicine}</strong> có thể gây nguy hiểm cho bệnh <strong>${disease}</strong>.</p>
      <p>Khuyến cáo: Tham khảo ý kiến bác sĩ trước khi sử dụng.</p>
    `;
    } else {
        document.querySelector(".warning-result").innerHTML = `
      <h1>Kết quả</h1>
      <p>Không có cảnh báo đặc biệt cho thuốc <strong>${medicine}</strong> với bệnh <strong>${disease}</strong>.</p>
    `;
    }
}
