# Ghi chú refactor clean code

## 1. Backend đã tách kế thừa

### Model
- `BaseEntity`: chứa `Id` dùng chung.
- `AuditableEntity`: kế thừa `BaseEntity`, bổ sung `CreatedAt`.
- `NamedEntity`: kế thừa `AuditableEntity`, bổ sung `Name`, `Description`.
- `Medicine` và `Disease` kế thừa `NamedEntity` để tránh lặp thuộc tính.
- `Admin` và `MedicineWarning` kế thừa `BaseEntity`.

### DTO
- `BaseDto`: chứa `Id`.
- `NamedDto`: chứa `Name`, `Description`, `CreatedAt`.
- `NamedCreateUpdateDto`: chứa dữ liệu thêm/sửa chung.
- `MedicineDto`, `DiseaseDto`, `MedicineCreateUpdateDto`, `DiseaseCreateUpdateDto` kế thừa các DTO nền.

### Controller
- Tạo `NamedCrudController<TEntity, TDto, TCreateUpdateDto>` xử lý CRUD dùng chung:
  - Lấy danh sách
  - Lấy chi tiết theo ID
  - Thêm mới
  - Cập nhật
  - Xóa
  - Kiểm tra trùng tên
  - Chuẩn hóa dữ liệu nhập
- `MedicinesController` và `DiseasesController` chỉ còn phần riêng của từng nghiệp vụ.

### Mapper và Helper
- Tạo `DtoMapper` để gom code chuyển Entity sang DTO.
- Tạo `RiskLevelHelper` để gom logic chuẩn hóa mức rủi ro: `Low`, `Medium`, `High`, `Critical`.

## 2. Frontend đã giảm lặp

- `config.js` dùng một hàm `apiRequest()` chung cho GET/POST/PUT/DELETE.
- `admin.js` tách lớp cha `AdminResourceManager`.
- `MedicineManager`, `DiseaseManager`, `WarningManager` kế thừa lớp cha để dùng chung logic render, thêm, xóa, thông báo lỗi.

## 3. Lợi ích sau refactor

- Code ngắn hơn, dễ đọc hơn.
- Khi thêm module mới như `Allergy`, `DrugGroup`, `Contraindication`, chỉ cần kế thừa controller/model/DTO có sẵn.
- Giảm lỗi do copy-paste giữa thuốc và bệnh nền.
- Dễ bảo trì, phù hợp yêu cầu môn Công nghệ phần mềm và phân tích thiết kế hướng đối tượng.
