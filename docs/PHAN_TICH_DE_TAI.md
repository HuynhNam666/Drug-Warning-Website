# Phân tích đề tài: Website hỗ trợ bệnh nhân tra cứu thuốc nên tránh khi đang có bệnh nền

## 1. Mô tả bài toán thực tế

Người bệnh có bệnh nền như tăng huyết áp, suy thận, bệnh gan, đau dạ dày, hen suyễn hoặc đái tháo đường thường phải thận trọng khi dùng thuốc. Một số thuốc thông dụng có thể làm nặng bệnh nền, gây tác dụng phụ hoặc cần điều chỉnh liều. Tuy nhiên, người bệnh thường khó nhớ hoặc khó tự đánh giá thuốc nào nên tránh.

Website được xây dựng nhằm hỗ trợ bệnh nhân tra cứu nhanh thông tin cảnh báo giữa thuốc và bệnh nền. Hệ thống không thay thế bác sĩ, nhưng giúp người dùng có thêm thông tin ban đầu trước khi sử dụng thuốc.

## 2. Mục tiêu hệ thống

- Cung cấp công cụ tra cứu thuốc theo bệnh nền.
- Hiển thị cảnh báo rủi ro rõ ràng theo mức độ Low, Medium, High, Critical.
- Cho phép admin cập nhật thuốc, bệnh nền và cảnh báo.
- Bổ sung AI hỗ trợ phân tích nhiều thuốc và nhiều bệnh nền cùng lúc.
- Giúp người bệnh nâng cao nhận thức khi tự mua hoặc sử dụng thuốc.

## 3. Tác nhân hệ thống

### Khách truy cập / bệnh nhân

- Tra cứu thuốc.
- Xem kết quả cảnh báo.
- Dùng AI hỗ trợ phân tích danh sách thuốc.
- Xem hướng dẫn sử dụng hệ thống.

### Quản trị viên

- Đăng nhập hệ thống.
- Quản lý danh mục thuốc.
- Quản lý danh mục bệnh nền.
- Quản lý dữ liệu cảnh báo.
- Cập nhật nội dung khuyến nghị.

### Hệ thống AI

- Nhận diện tên thuốc và bệnh nền từ dữ liệu người dùng nhập.
- So khớp với database.
- Tổng hợp rủi ro.
- Đưa ra gợi ý an toàn.

## 4. Yêu cầu chức năng

| Mã | Chức năng | Mô tả |
|---|---|---|
| F01 | Tra cứu thuốc | Người dùng nhập tên thuốc và chọn bệnh nền để xem cảnh báo |
| F02 | Xem danh sách thuốc | Hiển thị danh sách thuốc có trong hệ thống |
| F03 | Xem danh sách bệnh nền | Hiển thị danh sách bệnh nền |
| F04 | Xem kết quả cảnh báo | Hiển thị mức độ rủi ro, nội dung cảnh báo và khuyến nghị |
| F05 | AI phân tích | AI nhận nhiều thuốc và nhiều bệnh nền, sau đó tổng hợp rủi ro |
| F06 | Đăng nhập admin | Admin đăng nhập bằng username/password |
| F07 | Quản lý thuốc | Admin thêm, sửa, xóa thuốc |
| F08 | Quản lý bệnh nền | Admin thêm, sửa, xóa bệnh nền |
| F09 | Quản lý cảnh báo | Admin thêm, sửa, xóa mối quan hệ thuốc - bệnh nền |
| F10 | Kiểm thử API | Dùng Swagger để test API |

## 5. Yêu cầu phi chức năng

| Mã | Yêu cầu | Mô tả |
|---|---|---|
| NF01 | Dễ sử dụng | Giao diện đơn giản, người bệnh dễ nhập và xem kết quả |
| NF02 | Bảo mật | Chức năng quản trị cần JWT Authentication |
| NF03 | Hiệu năng | Tra cứu nhanh trên database PostgreSQL |
| NF04 | Dễ mở rộng | Có thể bổ sung thuốc, bệnh nền, cảnh báo mới |
| NF05 | An toàn y tế | Luôn hiển thị cảnh báo không thay thế bác sĩ/dược sĩ |
| NF06 | Bảo trì | Code chia theo Controller, DTO, Service, Model, Data |

## 6. User Story

| Mã | User Story | Tiêu chí chấp nhận |
|---|---|---|
| US01 | Là bệnh nhân, tôi muốn nhập tên thuốc để tra cứu cảnh báo | Hệ thống trả về kết quả theo thuốc và bệnh nền |
| US02 | Là bệnh nhân, tôi muốn chọn bệnh nền của mình | Hệ thống có danh sách bệnh nền từ database |
| US03 | Là bệnh nhân, tôi muốn biết mức độ nguy hiểm | Kết quả có Low/Medium/High/Critical |
| US04 | Là bệnh nhân, tôi muốn dùng AI nhập nhiều thuốc cùng lúc | AI trả về danh sách rủi ro được sắp xếp theo mức nguy hiểm |
| US05 | Là admin, tôi muốn đăng nhập để quản lý dữ liệu | Đăng nhập thành công trả về JWT token |
| US06 | Là admin, tôi muốn thêm thuốc mới | Thuốc mới được lưu vào database |
| US07 | Là admin, tôi muốn thêm bệnh nền mới | Bệnh nền mới được lưu vào database |
| US08 | Là admin, tôi muốn thêm cảnh báo cho thuốc và bệnh nền | Cảnh báo được lưu và có thể tra cứu |
| US09 | Là admin, tôi muốn sửa cảnh báo | Nội dung cảnh báo được cập nhật |
| US10 | Là người dùng, tôi muốn xem khuyến nghị | Hệ thống hiển thị lời khuyên rõ ràng |

## 7. Thiết kế dữ liệu

### Bảng Admins

- Id
- Username
- PasswordHash
- FullName

### Bảng Medicines

- Id
- Name
- Description
- Usage
- SideEffects
- CreatedAt

### Bảng Diseases

- Id
- Name
- Description
- CreatedAt

### Bảng MedicineWarnings

- Id
- MedicineId
- DiseaseId
- RiskLevel
- WarningContent
- Recommendation

Quan hệ:

- Một thuốc có nhiều cảnh báo.
- Một bệnh nền có nhiều cảnh báo.
- MedicineWarnings là bảng trung gian giữa Medicines và Diseases.

## 8. Kiến trúc phần mềm

Hệ thống sử dụng mô hình 3-layer:

### Presentation Layer

- Frontend HTML/CSS/JavaScript.
- Người dùng nhập thuốc, bệnh nền, xem kết quả và dùng AI.

### Business Logic Layer

- ASP.NET Core Controllers.
- Services xử lý JWT, mật khẩu và AI Advisor.
- Xử lý nghiệp vụ tra cứu, phân quyền, đánh giá rủi ro.

### Data Access Layer

- Entity Framework Core.
- PostgreSQL lưu trữ dữ liệu thuốc, bệnh nền, cảnh báo và admin.

## 9. AI được tích hợp như thế nào?

Module AI trong dự án là `AiAdvisorService`. AI này hoạt động theo hướng expert system kết hợp truy xuất dữ liệu nội bộ:

1. Nhận danh sách thuốc và bệnh nền người dùng nhập.
2. Tách chuỗi đầu vào thành nhiều thuốc và nhiều bệnh nền.
3. Chuẩn hóa tiếng Việt, bỏ dấu để tăng khả năng so khớp.
4. So khớp thuốc và bệnh nền với database.
5. Truy xuất các cảnh báo tương ứng.
6. Tính điểm rủi ro theo mức Low, Medium, High, Critical.
7. Tổng hợp kết quả, cảnh báo cao nhất và khuyến nghị.

Ưu điểm: không cần Internet, không cần API key, phù hợp đồ án môn Công nghệ phần mềm và dễ demo.

## 10. Quy trình phát triển đề xuất

Nhóm có thể dùng Scrum/Kanban:

- Sprint 1: phân tích yêu cầu, user story, use case.
- Sprint 2: thiết kế database, UML, wireframe Figma.
- Sprint 3: xây backend API và PostgreSQL.
- Sprint 4: xây frontend và tích hợp API.
- Sprint 5: thêm AI Advisor, kiểm thử, hoàn thiện báo cáo và slide.

## 11. Công cụ sử dụng

- GitHub: quản lý mã nguồn.
- Figma: thiết kế giao diện.
- StarUML/draw.io: vẽ UML và ERD.
- Swagger: kiểm thử API.
- ChatGPT: hỗ trợ phân tích yêu cầu, user story, thiết kế backend, gợi ý AI.
