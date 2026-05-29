using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

    [HttpGet("/{flag}")]
    public async Task<IActionResult> GetAll(int flag)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateVisitDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok("Visit Updated");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VisitResponseDto>> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok("Visit Deleted");
    }

    [HttpPost("{visitId}/approve")]
    public async Task<IActionResult> ApproveVisit(int visitId, [FromBody] VisitApprovalRequestDto request)
    {
        var result = await _service.ApproveVisitAsync(visitId, request);

        return Ok(result);
    }
}
