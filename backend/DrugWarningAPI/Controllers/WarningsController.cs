using MedicineWarningAPI.Data;
using MedicineWarningAPI.DTOs;
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
            var query = _context.MedicineWarnings
                .AsNoTracking()
                .Include(x => x.Medicine)
                .Include(x => x.Disease)
                .AsQueryable();

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
                query = query.Where(x => x.RiskLevel == riskLevel.Trim());
            }

            var warnings = await query
                .OrderBy(x => x.Medicine!.Name)
                .ThenBy(x => x.Disease!.Name)
                .Select(x => new MedicineWarningDto
                {
                    Id = x.Id,
                    MedicineId = x.MedicineId,
                    MedicineName = x.Medicine != null ? x.Medicine.Name : string.Empty,
                    DiseaseId = x.DiseaseId,
                    DiseaseName = x.Disease != null ? x.Disease.Name : string.Empty,
                    RiskLevel = x.RiskLevel,
                    WarningContent = x.WarningContent,
                    Recommendation = x.Recommendation
                })
                .ToListAsync();

            return Ok(warnings);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicineWarningDto>> GetWarning(int id)
        {
            var warning = await _context.MedicineWarnings
                .AsNoTracking()
                .Include(x => x.Medicine)
                .Include(x => x.Disease)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (warning == null)
            {
                return NotFound("Không tìm thấy cảnh báo.");
            }

            return Ok(ToDto(warning));
        }

        [HttpGet("check")]
        public async Task<ActionResult<WarningSearchResultDto>> CheckWarning(
            [FromQuery] int medicineId,
            [FromQuery] int diseaseId)
        {
            var warning = await _context.MedicineWarnings
                .AsNoTracking()
                .Include(x => x.Medicine)
                .Include(x => x.Disease)
                .FirstOrDefaultAsync(x =>
                    x.MedicineId == medicineId &&
                    x.DiseaseId == diseaseId
                );

            if (warning == null)
            {
                return Ok(new WarningSearchResultDto
                {
                    Found = false,
                    Message = "Chưa có cảnh báo trong hệ thống cho thuốc và bệnh nền này. Người bệnh vẫn nên hỏi bác sĩ/dược sĩ trước khi dùng thuốc."
                });
            }

            return Ok(new WarningSearchResultDto
            {
                Found = true,
                Message = "Tìm thấy cảnh báo cần lưu ý.",
                Warnings = new List<MedicineWarningDto>
                {
                    ToDto(warning)
                }
            });
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

            var med = medicineName.Trim();
            var dis = diseaseName.Trim();

            var warnings = await _context.MedicineWarnings
                .AsNoTracking()
                .Include(x => x.Medicine)
                .Include(x => x.Disease)
                .Where(x =>
                    x.Medicine != null &&
                    x.Disease != null &&
                    x.Medicine.Name.Contains(med) &&
                    x.Disease.Name.Contains(dis)
                )
                .Select(x => new MedicineWarningDto
                {
                    Id = x.Id,
                    MedicineId = x.MedicineId,
                    MedicineName = x.Medicine != null ? x.Medicine.Name : string.Empty,
                    DiseaseId = x.DiseaseId,
                    DiseaseName = x.Disease != null ? x.Disease.Name : string.Empty,
                    RiskLevel = x.RiskLevel,
                    WarningContent = x.WarningContent,
                    Recommendation = x.Recommendation
                })
                .ToListAsync();

            if (!warnings.Any())
            {
                return Ok(new WarningSearchResultDto
                {
                    Found = false,
                    Message = "Chưa tìm thấy cảnh báo phù hợp. Kết quả này không thay thế tư vấn y tế chuyên môn."
                });
            }

            return Ok(new WarningSearchResultDto
            {
                Found = true,
                Message = $"Tìm thấy {warnings.Count} cảnh báo phù hợp.",
                Warnings = warnings
            });
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

            var exists = await _context.MedicineWarnings
                .AnyAsync(x =>
                    x.MedicineId == request.MedicineId &&
                    x.DiseaseId == request.DiseaseId
                );

            if (exists)
            {
                return Conflict("Cảnh báo cho thuốc và bệnh nền này đã tồn tại.");
            }

            var warning = new MedicineWarning
            {
                MedicineId = request.MedicineId,
                DiseaseId = request.DiseaseId,
                RiskLevel = request.RiskLevel.Trim(),
                WarningContent = request.WarningContent.Trim(),
                Recommendation = request.Recommendation
            };

            _context.MedicineWarnings.Add(warning);
            await _context.SaveChangesAsync();

            warning = await _context.MedicineWarnings
                .Include(x => x.Medicine)
                .Include(x => x.Disease)
                .FirstAsync(x => x.Id == warning.Id);

            return CreatedAtAction(
                nameof(GetWarning),
                new { id = warning.Id },
                ToDto(warning)
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

            var duplicate = await _context.MedicineWarnings
                .AnyAsync(x =>
                    x.Id != id &&
                    x.MedicineId == request.MedicineId &&
                    x.DiseaseId == request.DiseaseId
                );

            if (duplicate)
            {
                return Conflict("Cảnh báo cho thuốc và bệnh nền này đã tồn tại.");
            }

            warning.MedicineId = request.MedicineId;
            warning.DiseaseId = request.DiseaseId;
            warning.RiskLevel = request.RiskLevel.Trim();
            warning.WarningContent = request.WarningContent.Trim();
            warning.Recommendation = request.Recommendation;

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

            if (string.IsNullOrWhiteSpace(request.RiskLevel))
            {
                return BadRequest("Mức độ rủi ro không được để trống.");
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

            var validRiskLevels = new[]
            {
                "Low",
                "Medium",
                "High",
                "Critical"
            };

            if (!validRiskLevels.Contains(
                request.RiskLevel.Trim(),
                StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("RiskLevel chỉ nên là: Low, Medium, High hoặc Critical.");
            }

            request.RiskLevel = validRiskLevels.First(x =>
                x.Equals(request.RiskLevel.Trim(), StringComparison.OrdinalIgnoreCase)
            );

            return null;
        }

        private static MedicineWarningDto ToDto(MedicineWarning warning)
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
    }
}
