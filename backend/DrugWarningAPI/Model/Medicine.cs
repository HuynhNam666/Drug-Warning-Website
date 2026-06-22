namespace MedicineWarningAPI.Models
{
    public class Medicine : NamedEntity
    {
        public string? Usage { get; set; }

        public string? SideEffects { get; set; }

        public ICollection<MedicineWarning>? MedicineWarnings { get; set; }
    }
}
