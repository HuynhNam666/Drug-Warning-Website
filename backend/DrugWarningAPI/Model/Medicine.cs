namespace MedicineWarningAPI.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Usage { get; set; }

        public string? SideEffects { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<MedicineWarning>? MedicineWarnings { get; set; }
    }
}