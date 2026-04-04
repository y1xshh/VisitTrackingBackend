using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;


[ApiController]
[Route("api/[controller]")]
public class VisitPurposeController : ControllerBase
{
    private readonly IVisitPurposeService _service;

    public VisitPurposeController(IVisitPurposeService service)
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
    public async Task<IActionResult> Create([FromBody] VisitPurposeDto dto)
    {
        await _service.Create(dto);
        return Ok("Created Successfully");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VisitPurposeDto dto)
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
}