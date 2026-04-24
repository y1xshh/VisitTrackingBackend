using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

[ApiController]
[Route("api/[controller]")]
public class OrganisationController : ControllerBase
{
    private readonly IOrganisationService _service;

    public OrganisationController(IOrganisationService service)
    {
        _service = service;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var data = await _service.GetByIdAsync(id);

        if (data == null)
            return NotFound(new { message = $"Organisation with id {id} not found." });

        return Ok(data);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrganisationDto dto)
    {
        await _service.AddAsync(dto);
        return Ok(new { message = "Organisation created successfully" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrganisationDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(new { message = "Updated successfully" });
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { message = "Deleted successfully" });
    }
}