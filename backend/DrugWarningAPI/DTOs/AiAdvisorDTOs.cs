namespace MedicineWarningAPI.DTOs
{
    public class AiAdvisorRequestDto
    {
        public string MedicinesText { get; set; } = string.Empty;
        public string DiseasesText { get; set; } = string.Empty;
        public string? AgeGroup { get; set; }
        public string? Note { get; set; }
    }

    public class AiDetectedItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AiRiskItemDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public string WarningContent { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
    }

    public class AiAdvisorResponseDto
    {
        public string Summary { get; set; } = string.Empty;
        public string HighestRiskLevel { get; set; } = "Unknown";
        public int HighestRiskScore { get; set; }
        public List<AiDetectedItemDto> DetectedMedicines { get; set; } = new();
        public List<AiDetectedItemDto> DetectedDiseases { get; set; } = new();
        public List<string> UnknownMedicines { get; set; } = new();
        public List<string> UnknownDiseases { get; set; } = new();
        public List<AiRiskItemDto> Risks { get; set; } = new();
        public List<string> AiSuggestions { get; set; } = new();
        public string MedicalDisclaimer { get; set; } =
            "Kết quả chỉ hỗ trợ sàng lọc ban đầu, không thay thế chẩn đoán hoặc chỉ định của bác sĩ/dược sĩ.";
    }
}
