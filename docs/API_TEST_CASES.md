# API test cases

## 1. Login admin

POST http://localhost:5000/api/Auth/login

```json
{
  "username": "admin",
  "password": "123456"
}
```

## 2. Lấy danh sách thuốc

GET http://localhost:5000/api/Medicines

## 3. Lấy danh sách bệnh nền

GET http://localhost:5000/api/Diseases

## 4. Tra cứu cảnh báo

GET http://localhost:5000/api/Warnings/search?medicineName=Ibuprofen&diseaseName=Đau%20dạ%20dày

## 5. AI phân tích

POST http://localhost:5000/api/AiAdvisor/analyze

```json
{
  "medicinesText": "Ibuprofen, Paracetamol, Aspirin",
  "diseasesText": "Tăng huyết áp, Suy thận, Đau dạ dày",
  "ageGroup": "Người trưởng thành",
  "note": "Người bệnh đang đau đầu"
}
```
