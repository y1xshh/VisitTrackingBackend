using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class VisitController : ControllerBase
{
    private readonly IVisitService _service;

    public VisitController(IVisitService service)
    {
        _service = service;
    }

    [HttpGet("getvisits")]
    public async Task<IActionResult> GetAll([FromQuery] int flag)
    {
        var data = await _service.GetAllAsync(flag);
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVisitDto dto)
    {
        await _service.Create(dto);
        return Ok("Visit Created");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateVisitDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok("Visit Updated");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VisitResponseDto>> GetById(int id) 
    {
        var data = await _service.GetByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok("Visit Deleted");
    }

    [HttpPost("{visitId:int}/approve")]
    public async Task<IActionResult> ApproveVisit(int visitId, [FromBody] VisitApprovalRequestDto request)
    {
        var result = await _service.ApproveVisitAsync(visitId, request);
        return Ok(result);
    }
}
