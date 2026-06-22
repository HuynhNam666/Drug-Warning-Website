# Hướng dẫn chạy Backend Drug Warning API

Backend dùng ASP.NET Core .NET 8 và PostgreSQL.

## 1. Cài đặt cần có

- .NET 8 SDK
- PostgreSQL

## 2. Tạo database trong PostgreSQL

Mở pgAdmin hoặc SQL Shell và chạy:

```sql
CREATE DATABASE drug_warning_db;
```

## 3. Sửa chuỗi kết nối nếu cần

Mở file:

```txt
backend/DrugWarningAPI/appsettings.json
```

Sửa `Password=123456` thành mật khẩu PostgreSQL trên máy bạn.

## 4. Chạy backend

Mở Terminal tại thư mục project rồi chạy:

```bash
cd backend/DrugWarningAPI
dotnet restore
dotnet run --urls "http://localhost:5000"
```

## 5. Mở Swagger để test API

```txt
http://localhost:5000/swagger
```

Backend đã được sửa để tự tạo bảng và tự thêm dữ liệu mẫu khi chạy lần đầu.

## 6. Tài khoản admin mẫu

```txt
username: admin
password: 123456
```

## 7. API test nhanh

```txt
GET  http://localhost:5000/api/Medicines
GET  http://localhost:5000/api/Diseases
GET  http://localhost:5000/api/Warnings
GET  http://localhost:5000/api/Warnings/search?medicineName=Ibuprofen&diseaseName=Suy%20th%E1%BA%ADn
POST http://localhost:5000/api/Auth/login
```

Body login:

```json
{
  "username": "admin",
  "password": "123456"
}
```

Sau khi login, copy `token` → bấm nút Authorize trên Swagger → nhập:

```txt
Bearer token_cua_ban
```

Rồi mới test được các API thêm/sửa/xóa có `[Authorize]`.
