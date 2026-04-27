using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

[ApiController]
[Route("api/[controller]")]
public class VehicleTypeController : ControllerBase
{
    private readonly IVehicleTypeService _service;

    public VehicleTypeController(IVehicleTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleTypeResponseDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleTypeResponseDto>> Get(int id)
    {
        var data = await _service.GetByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VehicleTypeDto dto)
    {
        await _service.Create(dto);
        return Ok("Vehicle Type Created");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VehicleTypeDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok("Deleted");
    }
}