using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

namespace VisitTracking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FunnelStageController : ControllerBase
    {
        private readonly IFunnelStageService _service;

        public FunnelStageController(IFunnelStageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FunnelStageResponseDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FunnelStageResponseDto>> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FunnelStageDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Created Successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FunnelStageDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Deleted Successfully");
        }

        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            return Ok(await _service.GetDropdownAsync());
        }
    }
}