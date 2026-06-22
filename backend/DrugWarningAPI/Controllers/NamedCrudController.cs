using MedicineWarningAPI.Data;
using MedicineWarningAPI.DTOs;
using MedicineWarningAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineWarningAPI.Controllers
{
    public abstract class NamedCrudController<TEntity, TDto, TCreateUpdateDto> : ControllerBase
        where TEntity : NamedEntity, new()
        where TCreateUpdateDto : NamedCreateUpdateDto
    {
        private readonly string _displayName;

        protected NamedCrudController(AppDbContext context, string displayName)
        {
            Context = context;
            _displayName = displayName;
        }

        protected AppDbContext Context { get; }

        protected abstract DbSet<TEntity> Entities { get; }

        protected abstract TDto ToDto(TEntity entity);

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll([FromQuery] string? keyword)
        {
            var query = Entities.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var trimmedKeyword = keyword.Trim();
                query = query.Where(x => x.Name.Contains(trimmedKeyword));
            }

            var entities = await query
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(entities.Select(ToDto));
        }

        [HttpGet("{id:int}")]
        public virtual async Task<ActionResult<TDto>> GetById(int id)
        {
            var entity = await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound($"Không tìm thấy {_displayName}.");
            }

            return Ok(ToDto(entity));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public virtual async Task<ActionResult<TDto>> Create(TCreateUpdateDto request)
        {
            var validation = await ValidateRequestAsync(request);
            if (validation != null)
            {
                return validation;
            }

            var entity = new TEntity();
            ApplyRequest(entity, request);

            Entities.Add(entity);
            await Context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.Id },
                ToDto(entity)
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public virtual async Task<IActionResult> Update(int id, TCreateUpdateDto request)
        {
            var entity = await Entities.FindAsync(id);
            if (entity == null)
            {
                return NotFound($"Không tìm thấy {_displayName}.");
            }

            var validation = await ValidateRequestAsync(request, id);
            if (validation != null)
            {
                return validation;
            }

            ApplyRequest(entity, request);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await Entities.FindAsync(id);
            if (entity == null)
            {
                return NotFound($"Không tìm thấy {_displayName}.");
            }

            Entities.Remove(entity);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        protected virtual async Task<ActionResult?> ValidateRequestAsync(
            TCreateUpdateDto request,
            int? updatingId = null)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest($"Tên {_displayName} không được để trống.");
            }

            var name = request.Name.Trim();
            var duplicate = await Entities.AnyAsync(x =>
                x.Name == name &&
                (!updatingId.HasValue || x.Id != updatingId.Value)
            );

            if (duplicate)
            {
                return Conflict($"Tên {_displayName} đã tồn tại.");
            }

            return null;
        }

        protected virtual void ApplyRequest(TEntity entity, TCreateUpdateDto request)
        {
            entity.Name = request.Name.Trim();
            entity.Description = NormalizeNullableText(request.Description);
        }

        protected static string? NormalizeNullableText(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
