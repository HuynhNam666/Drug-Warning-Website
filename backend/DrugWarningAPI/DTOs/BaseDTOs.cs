namespace MedicineWarningAPI.DTOs
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
    }

    public abstract class NamedDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public abstract class NamedCreateUpdateDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
