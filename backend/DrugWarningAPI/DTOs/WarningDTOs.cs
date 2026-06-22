namespace MedicineWarningAPI.DTOs
{
    public class MedicineWarningDto : BaseDto
    {
        public int MedicineId { get; set; }

        public string MedicineName { get; set; } = string.Empty;

        public int DiseaseId { get; set; }

        public string DiseaseName { get; set; } = string.Empty;

        public string RiskLevel { get; set; } = string.Empty;

        public string WarningContent { get; set; } = string.Empty;

        public string? Recommendation { get; set; }
    }

    public class MedicineWarningCreateUpdateDto
    {
        public int MedicineId { get; set; }

        public int DiseaseId { get; set; }

        public string RiskLevel { get; set; } = string.Empty;

        public string WarningContent { get; set; } = string.Empty;

        public string? Recommendation { get; set; }
    }

    public class WarningSearchResultDto
    {
        public bool Found { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<MedicineWarningDto> Warnings { get; set; } = new();
    }
}
