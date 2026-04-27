using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

namespace VisitTracking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutcomeTypeController : ControllerBase
    {
        private readonly IOutcomeTypeService _service;

        public OutcomeTypeController(IOutcomeTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeTypeResponseDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OutcomeTypeResponseDto>> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OutcomeTypeDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Created Successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OutcomeTypeDto dto)
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
            => Ok(await _service.GetDropdownAsync());
    }
}