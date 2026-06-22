using MedicineWarningAPI.DTOs;
using MedicineWarningAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineWarningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiAdvisorController : ControllerBase
    {
        private readonly IAiAdvisorService _aiAdvisorService;

        public AiAdvisorController(IAiAdvisorService aiAdvisorService)
        {
            _aiAdvisorService = aiAdvisorService;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<AiAdvisorResponseDto>> Analyze(AiAdvisorRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.MedicinesText) || string.IsNullOrWhiteSpace(request.DiseasesText))
            {
                return BadRequest("Vui lòng nhập ít nhất một thuốc và một bệnh nền.");
            }

            var result = await _aiAdvisorService.AnalyzeAsync(request);
            return Ok(result);
        }
    }
}
