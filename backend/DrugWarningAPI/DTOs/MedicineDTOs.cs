namespace MedicineWarningAPI.DTOs
{
    public class MedicineDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Usage { get; set; }

        public string? SideEffects { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class MedicineCreateUpdateDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Usage { get; set; }

        public string? SideEffects { get; set; }
    }
}
