using MedicineWarningAPI.Helpers;
using MedicineWarningAPI.Models;
using MedicineWarningAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace MedicineWarningAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

            await context.Database.EnsureCreatedAsync();

            if (!await context.Admins.AnyAsync())
            {
                context.Admins.Add(new Admin
                {
                    Username = "admin",
                    PasswordHash = passwordService.HashPassword("123456"),
                    FullName = "Quản trị viên"
                });
            }

            if (!await context.Medicines.AnyAsync())
            {
                context.Medicines.AddRange(
                    new Medicine
                    {
                        Name = "Ibuprofen",
                        Description = "Thuốc giảm đau, hạ sốt, chống viêm.",
                        Usage = "Dùng để giảm đau, hạ sốt, chống viêm theo hướng dẫn.",
                        SideEffects = "Có thể gây kích ứng dạ dày, tăng nguy cơ xuất huyết tiêu hóa, ảnh hưởng chức năng thận.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Medicine
                    {
                        Name = "Paracetamol",
                        Description = "Thuốc giảm đau, hạ sốt thông dụng.",
                        Usage = "Dùng khi đau nhẹ hoặc sốt.",
                        SideEffects = "Dùng quá liều có thể gây độc gan.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Medicine
                    {
                        Name = "Aspirin",
                        Description = "Thuốc giảm đau, chống viêm, chống kết tập tiểu cầu.",
                        Usage = "Dùng giảm đau hoặc phòng ngừa huyết khối theo chỉ định.",
                        SideEffects = "Có thể gây chảy máu, kích ứng dạ dày.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Medicine
                    {
                        Name = "Metformin",
                        Description = "Thuốc điều trị đái tháo đường type 2.",
                        Usage = "Giúp kiểm soát đường huyết theo chỉ định bác sĩ.",
                        SideEffects = "Có thể gây rối loạn tiêu hóa, cần thận trọng ở bệnh thận.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Medicine
                    {
                        Name = "Pseudoephedrine",
                        Description = "Thuốc giảm nghẹt mũi.",
                        Usage = "Dùng trong cảm cúm, viêm mũi theo hướng dẫn.",
                        SideEffects = "Có thể làm tăng huyết áp, hồi hộp.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Medicine
                    {
                        Name = "Salbutamol",
                        Description = "Thuốc giãn phế quản thường dùng trong hen phế quản.",
                        Usage = "Dùng theo chỉ định để giảm co thắt phế quản.",
                        SideEffects = "Có thể gây run tay, hồi hộp, tăng nhịp tim.",
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            if (!await context.Diseases.AnyAsync())
            {
                context.Diseases.AddRange(
                    new Disease
                    {
                        Name = "Tăng huyết áp",
                        Description = "Bệnh lý huyết áp cao, cần thận trọng với thuốc có thể làm tăng huyết áp.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Disease
                    {
                        Name = "Đau dạ dày",
                        Description = "Bệnh lý liên quan đến viêm loét hoặc kích ứng dạ dày.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Disease
                    {
                        Name = "Suy thận",
                        Description = "Chức năng thận suy giảm, cần thận trọng khi dùng thuốc.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Disease
                    {
                        Name = "Bệnh gan",
                        Description = "Chức năng gan suy giảm, dễ bị ảnh hưởng bởi thuốc chuyển hóa qua gan.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Disease
                    {
                        Name = "Đái tháo đường",
                        Description = "Bệnh rối loạn đường huyết cần theo dõi lâu dài.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Disease
                    {
                        Name = "Hen suyễn",
                        Description = "Bệnh hô hấp mạn tính có thể nhạy cảm với một số thuốc.",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Disease
                    {
                        Name = "Tim mạch",
                        Description = "Nhóm bệnh liên quan đến tim và mạch máu.",
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            await context.SaveChangesAsync();

            if (!await context.MedicineWarnings.AnyAsync())
            {
                var medicines = await context.Medicines.ToDictionaryAsync(x => x.Name, x => x.Id);
                var diseases = await context.Diseases.ToDictionaryAsync(x => x.Name, x => x.Id);

                var warnings = new List<MedicineWarning>();

                void Add(string medicine, string disease, string risk, string content, string recommendation)
                {
                    if (medicines.ContainsKey(medicine) && diseases.ContainsKey(disease))
                    {
                        warnings.Add(new MedicineWarning
                        {
                            MedicineId = medicines[medicine],
                            DiseaseId = diseases[disease],
                            RiskLevel = risk,
                            WarningContent = content,
                            Recommendation = recommendation
                        });
                    }
                }

                Add(
                    "Ibuprofen",
                    "Đau dạ dày",
                    RiskLevelHelper.High,
                    "Ibuprofen có thể gây kích ứng dạ dày và làm nặng thêm tình trạng đau hoặc viêm loét dạ dày.",
                    "Nên hỏi bác sĩ trước khi dùng. Có thể cân nhắc thuốc khác an toàn hơn cho dạ dày."
                );

                Add(
                    "Ibuprofen",
                    "Suy thận",
                    RiskLevelHelper.High,
                    "Ibuprofen có thể ảnh hưởng đến chức năng thận, đặc biệt ở người đã có bệnh thận.",
                    "Không tự ý dùng. Cần hỏi bác sĩ hoặc dược sĩ trước khi sử dụng."
                );

                Add(
                    "Ibuprofen",
                    "Tăng huyết áp",
                    RiskLevelHelper.Medium,
                    "Ibuprofen có thể làm giảm hiệu quả một số thuốc huyết áp và gây giữ nước.",
                    "Người bệnh tăng huyết áp nên hỏi bác sĩ nếu cần dùng nhiều ngày."
                );

                Add(
                    "Paracetamol",
                    "Bệnh gan",
                    RiskLevelHelper.Critical,
                    "Paracetamol dùng quá liều hoặc dùng kéo dài có thể gây độc gan, nguy hiểm ở người có bệnh gan.",
                    "Không dùng quá liều. Cần hỏi bác sĩ để được hướng dẫn liều phù hợp."
                );

                Add(
                    "Aspirin",
                    "Đau dạ dày",
                    RiskLevelHelper.High,
                    "Aspirin có thể gây kích ứng dạ dày và tăng nguy cơ xuất huyết tiêu hóa.",
                    "Tránh tự ý sử dụng nếu có tiền sử viêm loét dạ dày."
                );

                Add(
                    "Aspirin",
                    "Hen suyễn",
                    RiskLevelHelper.High,
                    "Một số người bệnh hen có thể nhạy cảm với Aspirin, gây khó thở hoặc co thắt phế quản.",
                    "Nên hỏi bác sĩ trước khi sử dụng."
                );

                Add(
                    "Pseudoephedrine",
                    "Tăng huyết áp",
                    RiskLevelHelper.High,
                    "Pseudoephedrine có thể làm tăng huyết áp và gây hồi hộp.",
                    "Người bị tăng huyết áp nên tránh hoặc hỏi bác sĩ trước khi dùng."
                );

                Add(
                    "Metformin",
                    "Suy thận",
                    RiskLevelHelper.Medium,
                    "Metformin cần thận trọng ở người suy thận vì có nguy cơ tích lũy thuốc.",
                    "Cần kiểm tra chức năng thận và dùng theo chỉ định bác sĩ."
                );

                Add(
                    "Salbutamol",
                    "Tim mạch",
                    RiskLevelHelper.Medium,
                    "Salbutamol có thể gây hồi hộp, tăng nhịp tim, cần thận trọng ở người có bệnh tim mạch.",
                    "Dùng đúng liều và hỏi ý kiến bác sĩ nếu có bệnh tim mạch."
                );

                context.MedicineWarnings.AddRange(warnings);
                await context.SaveChangesAsync();
            }
        }
    }
}