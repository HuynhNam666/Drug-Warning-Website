using MedicineWarningAPI.DTOs;
using MedicineWarningAPI.Models;

namespace MedicineWarningAPI.Mappers
{
    public static class DtoMapper
    {
        public static MedicineDto ToDto(this Medicine medicine)
        {
            return new MedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Description = medicine.Description,
                Usage = medicine.Usage,
                SideEffects = medicine.SideEffects,
                CreatedAt = medicine.CreatedAt
            };
        }

        public static DiseaseDto ToDto(this Disease disease)
        {
            return new DiseaseDto
            {
                Id = disease.Id,
                Name = disease.Name,
                Description = disease.Description,
                CreatedAt = disease.CreatedAt
            };
        }

        public static MedicineWarningDto ToDto(this MedicineWarning warning)
        {
            return new MedicineWarningDto
            {
                Id = warning.Id,
                MedicineId = warning.MedicineId,
                MedicineName = warning.Medicine?.Name ?? string.Empty,
                DiseaseId = warning.DiseaseId,
                DiseaseName = warning.Disease?.Name ?? string.Empty,
                RiskLevel = warning.RiskLevel,
                WarningContent = warning.WarningContent,
                Recommendation = warning.Recommendation
            };
        }

        public static AiDetectedItemDto ToDetectedDto(this NamedEntity entity)
        {
            return new AiDetectedItemDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
