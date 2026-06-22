namespace MedicineWarningAPI.Models
{
    public class Disease : NamedEntity
    {
        public ICollection<MedicineWarning>? MedicineWarnings { get; set; }
    }
}
