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
    public class MedicinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicinesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMedicines(
            [FromQuery] string? keyword)
        {
            var query = _context.Medicines.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword.Trim()));
            }

            var medicines = await query
                .OrderBy(x => x.Name)
                .Select(x => new MedicineDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Usage = x.Usage,
                    SideEffects = x.SideEffects,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return Ok(medicines);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicineDto>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (medicine == null)
            {
                return NotFound("Không tìm thấy thuốc.");
            }

            return Ok(ToDto(medicine));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MedicineDto>> CreateMedicine(
            MedicineCreateUpdateDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Tên thuốc không được để trống.");
            }

            var name = request.Name.Trim();

            var exists = await _context.Medicines
                .AnyAsync(x => x.Name == name);

            if (exists)
            {
                return Conflict("Tên thuốc đã tồn tại.");
            }

            var medicine = new Medicine
            {
                Name = name,
                Description = request.Description,
                Usage = request.Usage,
                SideEffects = request.SideEffects,
                CreatedAt = DateTime.Now
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMedicine),
                new { id = medicine.Id },
                ToDto(medicine)
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMedicine(
            int id,
            MedicineCreateUpdateDto request)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound("Không tìm thấy thuốc.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Tên thuốc không được để trống.");
            }

            var name = request.Name.Trim();

            var duplicate = await _context.Medicines
                .AnyAsync(x => x.Id != id && x.Name == name);

            if (duplicate)
            {
                return Conflict("Tên thuốc đã tồn tại.");
            }

            medicine.Name = name;
            medicine.Description = request.Description;
            medicine.Usage = request.Usage;
            medicine.SideEffects = request.SideEffects;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound("Không tìm thấy thuốc.");
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static MedicineDto ToDto(Medicine medicine)
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
    }
}
