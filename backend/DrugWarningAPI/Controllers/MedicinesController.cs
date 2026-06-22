using MedicineWarningAPI.Data;
using MedicineWarningAPI.DTOs;
using MedicineWarningAPI.Mappers;
using MedicineWarningAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineWarningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicinesController : NamedCrudController<Medicine, MedicineDto, MedicineCreateUpdateDto>
    {
        public MedicinesController(AppDbContext context) : base(context, "thuốc")
        {
        }

        protected override DbSet<Medicine> Entities => Context.Medicines;

        protected override MedicineDto ToDto(Medicine entity) => entity.ToDto();

        protected override void ApplyRequest(Medicine entity, MedicineCreateUpdateDto request)
        {
            base.ApplyRequest(entity, request);
            entity.Usage = NormalizeNullableText(request.Usage);
            entity.SideEffects = NormalizeNullableText(request.SideEffects);
        }
    }
}
