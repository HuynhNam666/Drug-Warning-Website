namespace MedicineWarningAPI.DTOs
{
    public class DiseaseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class DiseaseCreateUpdateDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
