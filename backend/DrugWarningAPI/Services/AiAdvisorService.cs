using System.Globalization;
using System.Text;
using MedicineWarningAPI.Data;
using MedicineWarningAPI.DTOs;
using MedicineWarningAPI.Helpers;
using MedicineWarningAPI.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MedicineWarningAPI.Services
{
    public class AiAdvisorService : IAiAdvisorService
    {
        private readonly AppDbContext _context;

        public AiAdvisorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AiAdvisorResponseDto> AnalyzeAsync(AiAdvisorRequestDto request)
        {
            var response = new AiAdvisorResponseDto();

            var medicineTokens = SplitInput(request.MedicinesText);
            var diseaseTokens = SplitInput(request.DiseasesText);

            var medicines = await _context.Medicines.AsNoTracking().ToListAsync();
            var diseases = await _context.Diseases.AsNoTracking().ToListAsync();

            foreach (var token in medicineTokens)
            {
                var matched = medicines.FirstOrDefault(x => IsMatch(token, x.Name));
                if (matched == null)
                {
                    response.UnknownMedicines.Add(token);
                }
                else if (!response.DetectedMedicines.Any(x => x.Id == matched.Id))
                {
                    response.DetectedMedicines.Add(matched.ToDetectedDto());
                }
            }

            foreach (var token in diseaseTokens)
            {
                var matched = diseases.FirstOrDefault(x => IsMatch(token, x.Name));
                if (matched == null)
                {
                    response.UnknownDiseases.Add(token);
                }
                else if (!response.DetectedDiseases.Any(x => x.Id == matched.Id))
                {
                    response.DetectedDiseases.Add(matched.ToDetectedDto());
                }
            }

            if (!response.DetectedMedicines.Any() || !response.DetectedDiseases.Any())
            {
                response.Summary = "AI chưa nhận diện đủ tên thuốc và bệnh nền trong cơ sở dữ liệu.";
                response.HighestRiskLevel = "Unknown";
                response.AiSuggestions.Add("Hãy nhập tên thuốc và bệnh nền rõ ràng hơn, ví dụ: Ibuprofen, Aspirin; Tăng huyết áp, Suy thận.");
                response.AiSuggestions.Add("Nếu không chắc tên thuốc, người bệnh nên chụp toa thuốc hoặc hỏi dược sĩ để xác nhận trước khi dùng.");
                return response;
            }

            var medicineIds = response.DetectedMedicines.Select(x => x.Id).ToList();
            var diseaseIds = response.DetectedDiseases.Select(x => x.Id).ToList();

            var warnings = await _context.MedicineWarnings
                .AsNoTracking()
                .Include(x => x.Medicine)
                .Include(x => x.Disease)
                .Where(x => medicineIds.Contains(x.MedicineId) && diseaseIds.Contains(x.DiseaseId))
                .ToListAsync();

            response.Risks = warnings
                .Select(x => new AiRiskItemDto
                {
                    MedicineId = x.MedicineId,
                    MedicineName = x.Medicine?.Name ?? string.Empty,
                    DiseaseId = x.DiseaseId,
                    DiseaseName = x.Disease?.Name ?? string.Empty,
                    RiskLevel = RiskLevelHelper.Normalize(x.RiskLevel),
                    RiskScore = RiskLevelHelper.ToScore(x.RiskLevel),
                    WarningContent = x.WarningContent,
                    Recommendation = x.Recommendation ?? "Nên hỏi bác sĩ/dược sĩ trước khi sử dụng."
                })
                .OrderByDescending(x => x.RiskScore)
                .ThenBy(x => x.MedicineName)
                .ToList();

            response.HighestRiskScore = response.Risks.Any() ? response.Risks.Max(x => x.RiskScore) : 0;
            response.HighestRiskLevel = RiskLevelHelper.FromScore(response.HighestRiskScore);
            response.Summary = BuildSummary(response, request);
            response.AiSuggestions = BuildSuggestions(response, request);

            return response;
        }

        private static List<string> SplitInput(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new List<string>();
            }

            return text
                .Split(new[] { ',', ';', '\n', '\r', '|', '/', '+' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static bool IsMatch(string input, string dataName)
        {
            var a = RemoveDiacritics(input).ToLowerInvariant();
            var b = RemoveDiacritics(dataName).ToLowerInvariant();

            return a == b || a.Contains(b) || b.Contains(a);
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC)
                .Replace('đ', 'd')
                .Replace('Đ', 'D');
        }

        private static string BuildSummary(AiAdvisorResponseDto response, AiAdvisorRequestDto request)
        {
            if (!response.Risks.Any())
            {
                return "AI không tìm thấy cảnh báo trực tiếp giữa thuốc và bệnh nền đã nhập trong cơ sở dữ liệu hiện tại.";
            }

            var criticalCount = response.Risks.Count(x => x.RiskLevel == "Critical");
            var highCount = response.Risks.Count(x => x.RiskLevel == "High");
            var mediumCount = response.Risks.Count(x => x.RiskLevel == "Medium");

            if (criticalCount > 0)
            {
                return $"AI phát hiện {criticalCount} cảnh báo mức Critical. Người bệnh không nên tự ý dùng thuốc trước khi hỏi bác sĩ/dược sĩ.";
            }

            if (highCount > 0)
            {
                return $"AI phát hiện {highCount} cảnh báo mức High và {mediumCount} cảnh báo mức Medium. Cần thận trọng và hỏi chuyên môn y tế trước khi dùng.";
            }

            if (mediumCount > 0)
            {
                return $"AI phát hiện {mediumCount} cảnh báo mức Medium. Người bệnh nên đọc kỹ khuyến nghị và theo dõi khi sử dụng.";
            }

            return "AI chỉ tìm thấy cảnh báo nguy cơ thấp trong cơ sở dữ liệu hiện tại.";
        }

        private static List<string> BuildSuggestions(AiAdvisorResponseDto response, AiAdvisorRequestDto request)
        {
            var suggestions = new List<string>();

            if (response.Risks.Any(x => x.RiskLevel is "Critical" or "High"))
            {
                suggestions.Add("Không tự ý dùng hoặc tăng liều thuốc có cảnh báo High/Critical.");
                suggestions.Add("Mang danh sách thuốc và bệnh nền đến bác sĩ/dược sĩ để được kiểm tra tương tác và chống chỉ định.");
            }
            else if (response.Risks.Any())
            {
                suggestions.Add("Dùng thuốc đúng liều, đúng thời gian và theo dõi biểu hiện bất thường.");
                suggestions.Add("Nếu triệu chứng nặng hơn hoặc xuất hiện tác dụng phụ, ngừng tự dùng và liên hệ cơ sở y tế.");
            }
            else
            {
                suggestions.Add("Không có cảnh báo trực tiếp không có nghĩa là thuốc hoàn toàn an toàn cho mọi trường hợp.");
                suggestions.Add("Nên kiểm tra thêm dị ứng thuốc, thai kỳ, tuổi, chức năng gan thận và thuốc đang dùng kèm.");
            }

            if (!string.IsNullOrWhiteSpace(request.AgeGroup) && request.AgeGroup.Contains("cao", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Người cao tuổi thường nhạy cảm hơn với tác dụng phụ, nên ưu tiên hỏi bác sĩ trước khi dùng thuốc mới.");
            }

            if (response.UnknownMedicines.Any())
            {
                suggestions.Add("Một số thuốc chưa có trong cơ sở dữ liệu: " + string.Join(", ", response.UnknownMedicines) + ". Admin cần bổ sung dữ liệu để AI phân tích tốt hơn.");
            }

            return suggestions.Distinct().ToList();
        }
    }
}
