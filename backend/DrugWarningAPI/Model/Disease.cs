namespace MedicineWarningAPI.Models
{
    public class Disease
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<MedicineWarning>? MedicineWarnings { get; set; }
    }
}