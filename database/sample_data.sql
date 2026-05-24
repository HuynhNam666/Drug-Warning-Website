USE DrugWarningDb;
GO

INSERT INTO Admins (Username, PasswordHash, FullName)
VALUES 
('admin', 'pmWkWSBCL51Bq9rgt6i+0RoAVyROtNeDhvR9c4nCu0E=', N'Quản trị viên');
GO

INSERT INTO Medicines (Name, Description, Usage, SideEffects)
VALUES
(N'Ibuprofen', N'Thuốc giảm đau, hạ sốt, chống viêm.', N'Dùng để giảm đau, hạ sốt.', N'Có thể gây đau dạ dày, tăng nguy cơ xuất huyết tiêu hóa.'),
(N'Paracetamol', N'Thuốc giảm đau, hạ sốt thông dụng.', N'Dùng khi đau nhẹ hoặc sốt.', N'Dùng quá liều có thể gây hại gan.'),
(N'Aspirin', N'Thuốc giảm đau, chống viêm, chống kết tập tiểu cầu.', N'Dùng giảm đau hoặc phòng ngừa huyết khối theo chỉ định.', N'Có thể gây chảy máu, kích ứng dạ dày.'),
(N'Metformin', N'Thuốc điều trị đái tháo đường type 2.', N'Giúp kiểm soát đường huyết.', N'Có thể gây rối loạn tiêu hóa, cần thận trọng ở bệnh thận.'),
(N'Pseudoephedrine', N'Thuốc giảm nghẹt mũi.', N'Dùng trong cảm cúm, viêm mũi.', N'Có thể làm tăng huyết áp, hồi hộp.');
GO

INSERT INTO Diseases (Name, Description)
VALUES
(N'Tăng huyết áp', N'Bệnh lý huyết áp cao, cần kiểm soát thuốc và chế độ ăn.'),
(N'Đau dạ dày', N'Bệnh lý liên quan đến viêm loét hoặc kích ứng dạ dày.'),
(N'Suy thận', N'Chức năng thận suy giảm, cần thận trọng khi dùng thuốc.'),
(N'Bệnh gan', N'Chức năng gan suy giảm, dễ bị ảnh hưởng bởi thuốc chuyển hóa qua gan.'),
(N'Đái tháo đường', N'Bệnh rối loạn đường huyết cần theo dõi lâu dài.');
GO

INSERT INTO MedicineWarnings 
(MedicineId, DiseaseId, RiskLevel, WarningContent, Recommendation)
VALUES
(1, 2, N'High', 
 N'Ibuprofen có thể gây kích ứng dạ dày và làm nặng thêm tình trạng đau hoặc viêm loét dạ dày.',
 N'Nên hỏi bác sĩ trước khi dùng. Có thể cân nhắc thuốc khác an toàn hơn cho dạ dày.'),

(1, 3, N'High',
 N'Ibuprofen có thể ảnh hưởng đến chức năng thận, đặc biệt ở người đã có bệnh thận.',
 N'Không tự ý dùng. Cần hỏi bác sĩ/dược sĩ trước khi sử dụng.'),

(2, 4, N'Critical',
 N'Paracetamol dùng quá liều hoặc dùng kéo dài có thể gây độc gan, nguy hiểm ở người có bệnh gan.',
 N'Không dùng quá liều. Cần hỏi bác sĩ để được hướng dẫn liều phù hợp.'),

(3, 2, N'High',
 N'Aspirin có thể gây kích ứng dạ dày và tăng nguy cơ xuất huyết tiêu hóa.',
 N'Tránh tự ý sử dụng nếu có tiền sử viêm loét dạ dày.'),

(5, 1, N'High',
 N'Pseudoephedrine có thể làm tăng huyết áp và gây hồi hộp.',
 N'Người bị tăng huyết áp nên tránh hoặc hỏi bác sĩ trước khi dùng.'),

(4, 3, N'Medium',
 N'Metformin cần thận trọng ở người suy thận vì có nguy cơ tích lũy thuốc.',
 N'Cần kiểm tra chức năng thận và dùng theo chỉ định bác sĩ.');
GO
