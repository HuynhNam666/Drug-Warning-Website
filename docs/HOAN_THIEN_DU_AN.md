# Hoàn thiện dự án Website hỗ trợ bệnh nhân tra cứu thuốc nên tránh khi đang có bệnh nền

## 1. Phạm vi đã hoàn thiện

Dự án đã được hoàn thiện theo hướng có thể dùng để demo đồ án Công nghệ phần mềm:

- Backend ASP.NET Core Web API .NET 8.
- Database PostgreSQL, có tự tạo bảng và seed dữ liệu mẫu.
- Frontend HTML/CSS/JavaScript tích hợp API thật.
- Chức năng bệnh nhân: xem thuốc, xem bệnh nền, tra cứu cảnh báo, xem kết quả, dùng AI nội bộ.
- Chức năng admin: đăng nhập JWT, quản lý thuốc, quản lý bệnh nền, quản lý cảnh báo thuốc - bệnh nền.
- Tài liệu phân tích: yêu cầu chức năng, phi chức năng, user story, UML gợi ý, test case API.

## 2. Điểm đã chỉnh trong frontend

- Sửa đăng nhập admin để gọi API `/api/Auth/login` thay vì chỉ kiểm tra localStorage.
- Lưu JWT token vào localStorage sau khi đăng nhập thành công.
- Dashboard admin gọi API thật để lấy thuốc, bệnh nền, cảnh báo.
- Bổ sung quản lý cảnh báo thuốc - bệnh nền trên dashboard.
- Bổ sung hàm API dùng chung: GET, POST, PUT, DELETE, tự gắn Bearer token.
- Bổ sung thông báo lỗi khi backend chưa chạy hoặc token không hợp lệ.

## 3. Luồng demo khuyến nghị

1. Chạy PostgreSQL và tạo database `drug_warning_db`.
2. Chạy backend bằng lệnh:

```bash
cd backend/DrugWarningAPI
dotnet restore
dotnet run --urls "http://localhost:5000"
```

3. Mở Swagger: `http://localhost:5000/swagger`.
4. Mở frontend bằng Live Server hoặc mở file `frontend/index.html`.
5. Demo bệnh nhân:
   - Vào `Tra cứu thuốc`.
   - Nhập `Ibuprofen`.
   - Chọn `Suy thận` hoặc `Đau dạ dày`.
   - Xem cảnh báo.
6. Demo AI:
   - Nhập thuốc: `Ibuprofen, Paracetamol, Aspirin`.
   - Nhập bệnh nền: `Tăng huyết áp, Suy thận, Đau dạ dày, Bệnh gan`.
   - Xem hệ thống phân tích rủi ro.
7. Demo admin:
   - Đăng nhập `admin / 123456`.
   - Thêm thuốc, bệnh nền, cảnh báo mới.
   - Quay lại tra cứu để kiểm tra dữ liệu vừa thêm.

## 4. Lưu ý an toàn y tế

Hệ thống chỉ hỗ trợ sàng lọc rủi ro ban đầu. Kết quả không thay thế tư vấn, chẩn đoán hoặc chỉ định điều trị của bác sĩ/dược sĩ. Khi demo hoặc đưa vào báo cáo, cần nhấn mạnh dữ liệu trong hệ thống là dữ liệu mẫu phục vụ học tập.
