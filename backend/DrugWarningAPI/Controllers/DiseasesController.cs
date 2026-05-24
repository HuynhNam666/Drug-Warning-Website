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
    public class DiseasesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DiseasesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseases(
            [FromQuery] string? keyword)
        {
            var query = _context.Diseases.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword.Trim()));
            }

            var diseases = await query
                .OrderBy(x => x.Name)
                .Select(x => new DiseaseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return Ok(diseases);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DiseaseDto>> GetDisease(int id)
        {
            var disease = await _context.Diseases
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (disease == null)
            {
                return NotFound("Không tìm thấy bệnh nền.");
            }

            return Ok(ToDto(disease));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DiseaseDto>> CreateDisease(
            DiseaseCreateUpdateDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Tên bệnh nền không được để trống.");
            }

            var name = request.Name.Trim();

            var exists = await _context.Diseases
                .AnyAsync(x => x.Name == name);

            if (exists)
            {
                return Conflict("Tên bệnh nền đã tồn tại.");
            }

            var disease = new Disease
            {
                Name = name,
                Description = request.Description,
                CreatedAt = DateTime.Now
            };

            _context.Diseases.Add(disease);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDisease),
                new { id = disease.Id },
                ToDto(disease)
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDisease(
            int id,
            DiseaseCreateUpdateDto request)
        {
            var disease = await _context.Diseases.FindAsync(id);

            if (disease == null)
            {
                return NotFound("Không tìm thấy bệnh nền.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Tên bệnh nền không được để trống.");
            }

            var name = request.Name.Trim();

            var duplicate = await _context.Diseases
                .AnyAsync(x => x.Id != id && x.Name == name);

            if (duplicate)
            {
                return Conflict("Tên bệnh nền đã tồn tại.");
            }

            disease.Name = name;
            disease.Description = request.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var disease = await _context.Diseases.FindAsync(id);

            if (disease == null)
            {
                return NotFound("Không tìm thấy bệnh nền.");
            }

            _context.Diseases.Remove(disease);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static DiseaseDto ToDto(Disease disease)
        {
            return new DiseaseDto
            {
                Id = disease.Id,
                Name = disease.Name,
                Description = disease.Description,
                CreatedAt = disease.CreatedAt
            };
        }
    }
}