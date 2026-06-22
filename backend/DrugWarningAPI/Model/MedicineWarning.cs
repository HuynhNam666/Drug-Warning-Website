namespace MedicineWarningAPI.Models
{
    public class MedicineWarning : BaseEntity
    {
        public int MedicineId { get; set; }

        public Medicine? Medicine { get; set; }

        public int DiseaseId { get; set; }

        public Disease? Disease { get; set; }

        public string RiskLevel { get; set; } = string.Empty;

        public string WarningContent { get; set; } = string.Empty;

        public string? Recommendation { get; set; }
    }
}
