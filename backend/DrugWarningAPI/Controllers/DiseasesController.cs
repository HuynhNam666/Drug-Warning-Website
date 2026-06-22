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
    public class DiseasesController : NamedCrudController<Disease, DiseaseDto, DiseaseCreateUpdateDto>
    {
        public DiseasesController(AppDbContext context) : base(context, "bệnh nền")
        {
        }

        protected override DbSet<Disease> Entities => Context.Diseases;

        protected override DiseaseDto ToDto(Disease entity) => entity.ToDto();
    }
}
