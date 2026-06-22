using MedicineWarningAPI.Data;
using MedicineWarningAPI.DTOs;
using MedicineWarningAPI.Helpers;
using MedicineWarningAPI.Mappers;
using MedicineWarningAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineWarningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarningsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WarningsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineWarningDto>>> GetWarnings(
            [FromQuery] int? medicineId,
            [FromQuery] int? diseaseId,
            [FromQuery] string? riskLevel)
        {
            var query = BuildWarningQuery();

            if (medicineId.HasValue)
            {
                query = query.Where(x => x.MedicineId == medicineId.Value);
            }

            if (diseaseId.HasValue)
            {
                query = query.Where(x => x.DiseaseId == diseaseId.Value);
            }

            if (!string.IsNullOrWhiteSpace(riskLevel))
            {
                var normalizedRisk = RiskLevelHelper.Normalize(riskLevel);
                query = query.Where(x => x.RiskLevel == normalizedRisk);
            }

            var warnings = await query
                .OrderBy(x => x.Medicine!.Name)
                .ThenBy(x => x.Disease!.Name)
                .ToListAsync();

            return Ok(warnings.Select(x => x.ToDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicineWarningDto>> GetWarning(int id)
        {
            var warning = await BuildWarningQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (warning == null)
            {
                return NotFound("Không tìm thấy cảnh báo.");
            }

            return Ok(warning.ToDto());
        }

        [HttpGet("check")]
        public async Task<ActionResult<WarningSearchResultDto>> CheckWarning(
            [FromQuery] int medicineId,
            [FromQuery] int diseaseId)
        {
            var warning = await BuildWarningQuery()
                .FirstOrDefaultAsync(x =>
                    x.MedicineId == medicineId &&
                    x.DiseaseId == diseaseId
                );

            if (warning == null)
            {
                return Ok(NotFoundWarningResult(
                    "Chưa có cảnh báo trong hệ thống cho thuốc và bệnh nền này. Người bệnh vẫn nên hỏi bác sĩ/dược sĩ trước khi dùng thuốc."
                ));
            }

            return Ok(FoundWarningResult("Tìm thấy cảnh báo cần lưu ý.", new[] { warning }));
        }

        [HttpGet("search")]
        public async Task<ActionResult<WarningSearchResultDto>> SearchWarning(
            [FromQuery] string medicineName,
            [FromQuery] string diseaseName)
        {
            if (string.IsNullOrWhiteSpace(medicineName) ||
                string.IsNullOrWhiteSpace(diseaseName))
            {
                return BadRequest("Vui lòng nhập tên thuốc và tên bệnh nền.");
            }

            var medicineKeyword = medicineName.Trim();
            var diseaseKeyword = diseaseName.Trim();

            var warnings = await BuildWarningQuery()
                .Where(x =>
                    x.Medicine != null &&
                    x.Disease != null &&
                    x.Medicine.Name.Contains(medicineKeyword) &&
                    x.Disease.Name.Contains(diseaseKeyword)
                )
                .ToListAsync();

            if (!warnings.Any())
            {
                return Ok(NotFoundWarningResult(
                    "Chưa tìm thấy cảnh báo phù hợp. Kết quả này không thay thế tư vấn y tế chuyên môn."
                ));
            }

            return Ok(FoundWarningResult($"Tìm thấy {warnings.Count} cảnh báo phù hợp.", warnings));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MedicineWarningDto>> CreateWarning(
            MedicineWarningCreateUpdateDto request)
        {
            var validation = await ValidateWarningRequest(request);
            if (validation != null)
            {
                return validation;
            }

            var exists = await WarningPairExistsAsync(request.MedicineId, request.DiseaseId);
            if (exists)
            {
                return Conflict("Cảnh báo cho thuốc và bệnh nền này đã tồn tại.");
            }

            var warning = new MedicineWarning();
            ApplyRequest(warning, request);

            _context.MedicineWarnings.Add(warning);
            await _context.SaveChangesAsync();

            warning = await BuildWarningQuery()
                .FirstAsync(x => x.Id == warning.Id);

            return CreatedAtAction(
                nameof(GetWarning),
                new { id = warning.Id },
                warning.ToDto()
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateWarning(
            int id,
            MedicineWarningCreateUpdateDto request)
        {
            var warning = await _context.MedicineWarnings.FindAsync(id);
            if (warning == null)
            {
                return NotFound("Không tìm thấy cảnh báo.");
            }

            var validation = await ValidateWarningRequest(request);
            if (validation != null)
            {
                return validation;
            }

            var duplicate = await WarningPairExistsAsync(request.MedicineId, request.DiseaseId, id);
            if (duplicate)
            {
                return Conflict("Cảnh báo cho thuốc và bệnh nền này đã tồn tại.");
            }

            ApplyRequest(warning, request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWarning(int id)
        {
            var warning = await _context.MedicineWarnings.FindAsync(id);
            if (warning == null)
            {
                return NotFound("Không tìm thấy cảnh báo.");
            }

            _context.MedicineWarnings.Remove(warning);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private IQueryable<MedicineWarning> BuildWarningQuery()
        {
            return _context.MedicineWarnings
                .AsNoTracking()
                .Include(x => x.Medicine)
                .Include(x => x.Disease);
        }

        private async Task<ActionResult?> ValidateWarningRequest(
            MedicineWarningCreateUpdateDto request)
        {
            if (request.MedicineId <= 0)
            {
                return BadRequest("MedicineId không hợp lệ.");
            }

            if (request.DiseaseId <= 0)
            {
                return BadRequest("DiseaseId không hợp lệ.");
            }

            if (!RiskLevelHelper.IsValid(request.RiskLevel))
            {
                return BadRequest("RiskLevel chỉ nên là: Low, Medium, High hoặc Critical.");
            }

            if (string.IsNullOrWhiteSpace(request.WarningContent))
            {
                return BadRequest("Nội dung cảnh báo không được để trống.");
            }

            var medicineExists = await _context.Medicines
                .AnyAsync(x => x.Id == request.MedicineId);

            if (!medicineExists)
            {
                return BadRequest("Thuốc không tồn tại.");
            }

            var diseaseExists = await _context.Diseases
                .AnyAsync(x => x.Id == request.DiseaseId);

            if (!diseaseExists)
            {
                return BadRequest("Bệnh nền không tồn tại.");
            }

            return null;
        }

        private async Task<bool> WarningPairExistsAsync(
            int medicineId,
            int diseaseId,
            int? excludingId = null)
        {
            return await _context.MedicineWarnings.AnyAsync(x =>
                x.MedicineId == medicineId &&
                x.DiseaseId == diseaseId &&
                (!excludingId.HasValue || x.Id != excludingId.Value)
            );
        }

        private static void ApplyRequest(
            MedicineWarning warning,
            MedicineWarningCreateUpdateDto request)
        {
            warning.MedicineId = request.MedicineId;
            warning.DiseaseId = request.DiseaseId;
            warning.RiskLevel = RiskLevelHelper.Normalize(request.RiskLevel);
            warning.WarningContent = request.WarningContent.Trim();
            warning.Recommendation = NormalizeNullableText(request.Recommendation);
        }

        private static WarningSearchResultDto FoundWarningResult(
            string message,
            IEnumerable<MedicineWarning> warnings)
        {
            return new WarningSearchResultDto
            {
                Found = true,
                Message = message,
                Warnings = warnings.Select(x => x.ToDto()).ToList()
            };
        }

        private static WarningSearchResultDto NotFoundWarningResult(string message)
        {
            return new WarningSearchResultDto
            {
                Found = false,
                Message = message
            };
        }

        private static string? NormalizeNullableText(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
