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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
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

        [HttpPut]
        public async Task<IActionResult> Update(OutcomeTypeDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Deleted Successfully");
        }
    }
}