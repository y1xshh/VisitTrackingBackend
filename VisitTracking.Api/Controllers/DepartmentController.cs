using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

   
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var data = await _service.GetByIdAsync(id);

        if (data == null)
            return NotFound($"Department with ID {id} not found");

        return Ok(data);
    }

   
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.Create(dto);

        return Created("", "Department Created Successfully"); 
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.UpdateAsync(id, dto);

        return Ok("Department Updated Successfully");
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return Ok("Department Deleted Successfully");
    }
}