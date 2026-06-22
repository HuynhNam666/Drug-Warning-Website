namespace MedicineWarningAPI.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }

    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public abstract class NamedEntity : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
