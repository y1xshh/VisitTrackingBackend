using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Filter;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

namespace VisitTracking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize] // ✅ LOGIN REQUIRED
    [ServiceFilter(typeof(FirstLoginCheckFilter))] // ✅ FORCE PASSWORD CHANGE CHECK

    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return NotFound("Employee not found");

            return Ok(data);
        }

        // ✅ CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeDto dto)
        {
            await _service.AddAsync(dto);
            return Ok("Employee created successfully");
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Employee updated successfully");
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Employee deleted successfully");
        }
    }
}