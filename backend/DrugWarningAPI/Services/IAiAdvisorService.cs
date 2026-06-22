using MedicineWarningAPI.DTOs;

namespace MedicineWarningAPI.Services
{
    public interface IAiAdvisorService
    {
        Task<AiAdvisorResponseDto> AnalyzeAsync(AiAdvisorRequestDto request);
    }
}
