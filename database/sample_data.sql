-- Mật khẩu admin là: 123456
INSERT INTO "Admins" ("Username", "PasswordHash", "FullName")
VALUES ('admin', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'Quản trị viên')
ON CONFLICT ("Username") DO NOTHING;

INSERT INTO "Medicines" ("Name", "Description", "Usage", "SideEffects")
VALUES
('Ibuprofen', 'Thuốc giảm đau, hạ sốt, chống viêm.', 'Dùng để giảm đau, hạ sốt.', 'Có thể gây đau dạ dày, tăng nguy cơ xuất huyết tiêu hóa.'),
('Paracetamol', 'Thuốc giảm đau, hạ sốt thông dụng.', 'Dùng khi đau nhẹ hoặc sốt.', 'Dùng quá liều có thể gây hại gan.'),
('Aspirin', 'Thuốc giảm đau, chống viêm, chống kết tập tiểu cầu.', 'Dùng giảm đau hoặc phòng ngừa huyết khối theo chỉ định.', 'Có thể gây chảy máu, kích ứng dạ dày.'),
('Metformin', 'Thuốc điều trị đái tháo đường type 2.', 'Giúp kiểm soát đường huyết.', 'Có thể gây rối loạn tiêu hóa, cần thận trọng ở bệnh thận.'),
('Pseudoephedrine', 'Thuốc giảm nghẹt mũi.', 'Dùng trong cảm cúm, viêm mũi.', 'Có thể làm tăng huyết áp, hồi hộp.'),
('Salbutamol', 'Thuốc giãn phế quản thường dùng trong hen phế quản.', 'Dùng theo chỉ định để giảm co thắt phế quản.', 'Có thể gây run tay, hồi hộp, tăng nhịp tim.')
ON CONFLICT ("Name") DO NOTHING;

INSERT INTO "Diseases" ("Name", "Description")
VALUES
('Tăng huyết áp', 'Bệnh lý huyết áp cao, cần kiểm soát thuốc và chế độ ăn.'),
('Đau dạ dày', 'Bệnh lý liên quan đến viêm loét hoặc kích ứng dạ dày.'),
('Suy thận', 'Chức năng thận suy giảm, cần thận trọng khi dùng thuốc.'),
('Bệnh gan', 'Chức năng gan suy giảm, dễ bị ảnh hưởng bởi thuốc chuyển hóa qua gan.'),
('Đái tháo đường', 'Bệnh rối loạn đường huyết cần theo dõi lâu dài.'),
('Hen suyễn', 'Bệnh hô hấp mạn tính có thể nhạy cảm với một số thuốc.'),
('Tim mạch', 'Nhóm bệnh liên quan đến tim và mạch máu.')
ON CONFLICT ("Name") DO NOTHING;

INSERT INTO "MedicineWarnings" ("MedicineId", "DiseaseId", "RiskLevel", "WarningContent", "Recommendation")
SELECT m."Id", d."Id", 'High',
       'Ibuprofen có thể ảnh hưởng đến chức năng thận, đặc biệt ở người đã có bệnh thận.',
       'Không tự ý dùng. Cần hỏi bác sĩ/dược sĩ trước khi sử dụng.'
FROM "Medicines" m, "Diseases" d
WHERE m."Name"='Ibuprofen' AND d."Name"='Suy thận'
ON CONFLICT ("MedicineId", "DiseaseId") DO NOTHING;

INSERT INTO "MedicineWarnings" ("MedicineId", "DiseaseId", "RiskLevel", "WarningContent", "Recommendation")
SELECT m."Id", d."Id", 'High',
       'Ibuprofen có thể gây kích ứng dạ dày và làm nặng thêm tình trạng đau hoặc viêm loét dạ dày.',
       'Nên hỏi bác sĩ trước khi dùng.'
FROM "Medicines" m, "Diseases" d
WHERE m."Name"='Ibuprofen' AND d."Name"='Đau dạ dày'
ON CONFLICT ("MedicineId", "DiseaseId") DO NOTHING;

INSERT INTO "MedicineWarnings" ("MedicineId", "DiseaseId", "RiskLevel", "WarningContent", "Recommendation")
SELECT m."Id", d."Id", 'Critical',
       'Paracetamol dùng quá liều hoặc dùng kéo dài có thể gây độc gan, nguy hiểm ở người có bệnh gan.',
       'Không dùng quá liều. Cần hỏi bác sĩ để được hướng dẫn liều phù hợp.'
FROM "Medicines" m, "Diseases" d
WHERE m."Name"='Paracetamol' AND d."Name"='Bệnh gan'
ON CONFLICT ("MedicineId", "DiseaseId") DO NOTHING;

INSERT INTO "MedicineWarnings" ("MedicineId", "DiseaseId", "RiskLevel", "WarningContent", "Recommendation")
SELECT m."Id", d."Id", 'High',
       'Pseudoephedrine có thể làm tăng huyết áp và gây hồi hộp.',
       'Người bị tăng huyết áp nên tránh hoặc hỏi bác sĩ trước khi dùng.'
FROM "Medicines" m, "Diseases" d
WHERE m."Name"='Pseudoephedrine' AND d."Name"='Tăng huyết áp'
ON CONFLICT ("MedicineId", "DiseaseId") DO NOTHING;
