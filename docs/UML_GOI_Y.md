# Gợi ý UML cho đồ án

## 1. Use Case tổng quát

Tác nhân:
- Bệnh nhân
- Admin
- AI Advisor

Use case chính:
- Tra cứu thuốc theo bệnh nền
- Xem kết quả cảnh báo
- Dùng AI phân tích nhiều thuốc
- Xem hướng dẫn sử dụng
- Đăng nhập admin
- Quản lý thuốc
- Quản lý bệnh nền
- Quản lý cảnh báo

## 2. Use Case cấp 2

### UC01 - Tra cứu thuốc
- Nhập tên thuốc
- Chọn bệnh nền
- Gửi yêu cầu tra cứu
- Hiển thị kết quả cảnh báo

### UC02 - AI hỗ trợ phân tích
- Nhập danh sách thuốc
- Nhập danh sách bệnh nền
- AI nhận diện dữ liệu
- AI so khớp database
- AI tổng hợp rủi ro
- AI đưa khuyến nghị

### UC03 - Quản lý thuốc
- Xem danh sách thuốc
- Thêm thuốc
- Sửa thuốc
- Xóa thuốc

### UC04 - Quản lý bệnh nền
- Xem danh sách bệnh nền
- Thêm bệnh nền
- Sửa bệnh nền
- Xóa bệnh nền

### UC05 - Quản lý cảnh báo
- Xem danh sách cảnh báo
- Thêm cảnh báo thuốc - bệnh nền
- Sửa cảnh báo
- Xóa cảnh báo

## 3. Activity Diagram: Tra cứu thuốc

1. Người dùng mở trang tra cứu.
2. Nhập tên thuốc.
3. Chọn bệnh nền.
4. Hệ thống kiểm tra dữ liệu nhập.
5. Gửi request đến API Warnings/search.
6. Backend truy vấn database.
7. Nếu có cảnh báo, hiển thị mức độ và khuyến nghị.
8. Nếu không có cảnh báo, hiển thị thông báo chưa có dữ liệu.

## 4. Activity Diagram: AI hỗ trợ

1. Người dùng nhập nhiều thuốc và nhiều bệnh nền.
2. Frontend gửi request đến API AiAdvisor/analyze.
3. AI tách chuỗi đầu vào.
4. AI chuẩn hóa tiếng Việt và so khớp dữ liệu.
5. AI truy vấn các cảnh báo liên quan.
6. AI tính điểm rủi ro.
7. AI sắp xếp rủi ro theo mức độ.
8. AI trả về kết quả và khuyến nghị.

## 5. Sequence Diagram: AI phân tích

Bệnh nhân -> Frontend: nhập thuốc, bệnh nền
Frontend -> AiAdvisorController: POST /api/AiAdvisor/analyze
AiAdvisorController -> AiAdvisorService: AnalyzeAsync(request)
AiAdvisorService -> AppDbContext: lấy Medicines, Diseases, MedicineWarnings
AppDbContext -> PostgreSQL: truy vấn dữ liệu
PostgreSQL -> AppDbContext: trả dữ liệu
AiAdvisorService -> AiAdvisorService: nhận diện, tính điểm rủi ro, tạo gợi ý
AiAdvisorService -> AiAdvisorController: trả AiAdvisorResponseDto
AiAdvisorController -> Frontend: JSON kết quả
Frontend -> Bệnh nhân: hiển thị cảnh báo AI

## 6. Class Diagram chính

- Admin
- Medicine
- Disease
- MedicineWarning
- AppDbContext
- AuthController
- MedicinesController
- DiseasesController
- WarningsController
- AiAdvisorController
- IJwtService / JwtService
- IPasswordService / PasswordService
- IAiAdvisorService / AiAdvisorService
