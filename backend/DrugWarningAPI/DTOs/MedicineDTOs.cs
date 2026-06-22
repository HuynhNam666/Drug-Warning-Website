namespace MedicineWarningAPI.DTOs
{
    public class MedicineDto : NamedDto
    {
        public string? Usage { get; set; }

        public string? SideEffects { get; set; }
    }

    public class MedicineCreateUpdateDto : NamedCreateUpdateDto
    {
        public string? Usage { get; set; }

        public string? SideEffects { get; set; }
    }
}
