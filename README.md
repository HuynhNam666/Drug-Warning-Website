# Drug Warning Website

Website hỗ trợ bệnh nhân tra cứu thuốc nên tránh khi đang có bệnh nền. Hệ thống có backend ASP.NET Core Web API, database PostgreSQL, frontend HTML/CSS/JavaScript và module AI nội bộ hỗ trợ sàng lọc rủi ro thuốc - bệnh nền.

## 1. Công nghệ sử dụng

- Backend: ASP.NET Core Web API (.NET 8)
- Database: PostgreSQL
- ORM: Entity Framework Core + Npgsql
- Authentication: JWT cho admin
- Frontend: HTML, CSS, JavaScript, Bootstrap
- API test: Swagger
- AI hỗ trợ: AI Advisor Service dạng expert system, phân tích dữ liệu thuốc - bệnh nền từ database
- Công cụ hiện đại: GitHub, Figma, StarUML, Jira/Trello

## 2. Chức năng chính

### Người dùng/bệnh nhân

- Xem danh sách thuốc
- Xem danh sách bệnh nền
- Tra cứu thuốc theo bệnh nền
- Xem mức cảnh báo: Low, Medium, High, Critical
- Dùng AI hỗ trợ phân tích nhiều thuốc và nhiều bệnh nền cùng lúc
- Nhận khuyến nghị an toàn khi dùng thuốc

### Admin

- Đăng nhập bằng JWT
- Quản lý thuốc
- Quản lý bệnh nền
- Quản lý cảnh báo thuốc - bệnh nền

## 3. Chạy backend

Cài PostgreSQL, tạo database:

```sql
CREATE DATABASE drug_warning_db;
```

Sửa mật khẩu PostgreSQL trong file:

```txt
backend/DrugWarningAPI/appsettings.json
```

Chạy backend:

```bash
cd backend/DrugWarningAPI
dotnet restore
dotnet run
```

Mở Swagger:

```txt
http://localhost:5000/swagger
```

Backend tự tạo bảng và dữ liệu mẫu khi chạy lần đầu.

## 4. Chạy frontend

Mở file:

```txt
frontend/index.html
```

hoặc dùng VS Code Live Server.

Nếu đổi port backend, sửa file:

```txt
frontend/js/config.js
```

## 5. Tài khoản admin mẫu

```txt
Username: admin
Password: 123456
```

## 6. API quan trọng

```txt
GET  /api/Medicines
GET  /api/Diseases
GET  /api/Warnings/search?medicineName=Ibuprofen&diseaseName=Đau dạ dày
POST /api/AiAdvisor/analyze
POST /api/Auth/login
```

Body AI mẫu:

```json
{
  "medicinesText": "Ibuprofen, Paracetamol, Aspirin",
  "diseasesText": "Tăng huyết áp, Suy thận, Đau dạ dày",
  "ageGroup": "Người trưởng thành",
  "note": "Người bệnh đang đau đầu và có bệnh nền"
}
```

## 7. Lưu ý y tế

Kết quả chỉ hỗ trợ sàng lọc ban đầu, không thay thế tư vấn, chẩn đoán hoặc chỉ định của bác sĩ/dược sĩ.

## Refactor clean code

Phiên bản này đã được chỉnh lại theo hướng giảm lặp và có kế thừa:

- Backend có `BaseEntity`, `AuditableEntity`, `NamedEntity` cho các model dùng chung.
- DTO có `BaseDto`, `NamedDto`, `NamedCreateUpdateDto`.
- `MedicinesController` và `DiseasesController` kế thừa `NamedCrudController` để dùng chung CRUD.
- Mapping Entity -> DTO được gom vào `Mappers/DtoMapper.cs`.
- Chuẩn hóa mức rủi ro được gom vào `Helpers/RiskLevelHelper.cs`.
- Frontend admin dùng lớp cha `AdminResourceManager`, các manager con kế thừa để giảm code lặp.

Xem chi tiết tại `docs/REFACTOR_CLEAN_CODE.md`.
