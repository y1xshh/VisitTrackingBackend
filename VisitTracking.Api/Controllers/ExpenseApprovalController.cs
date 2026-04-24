using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

namespace VisitTracking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseApprovalController : ControllerBase
    {
        private readonly IExpenseApprovalService _service;

        public ExpenseApprovalController(IExpenseApprovalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseApprovalDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Submitted for Approval");
        }

       
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id, int approvedBy, string? remarks)
        {
            await _service.ApproveAsync(id, approvedBy, remarks);
            return Ok("Approved");
        }

        
        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id, int approvedBy, string? remarks)
        {
            await _service.RejectAsync(id, approvedBy, remarks);
            return Ok("Rejected");
        }
    }
}
