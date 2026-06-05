using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;

namespace VisitTracking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseApprovalController : ControllerBase
    {
        private readonly IExpenseApprovalService _service;

        public ExpenseApprovalController(IExpenseApprovalService service)
        {
            _service = service;
        }

        // GET: api/ExpenseApproval
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        // GET: api/ExpenseApproval/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Expense approval not found"
                });
            }

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        // POST: api/ExpenseApproval
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseApprovalDto dto)
        {
            try
            {
                await _service.CreateAsync(dto);

                return Ok(new
                {
                    Success = true,
                    Message = "Submitted for approval"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // PUT: api/ExpenseApproval/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] ExpenseApprovalDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);

                return Ok(new
                {
                    Success = true,
                    Message = "Updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // POST: api/ExpenseApproval/approve/5
        [HttpPost("approve/{id:int}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Approve(
            int id,
            [FromBody] ApprovalActionDto dto)
        {
            try
            {
                await _service.ApproveAsync(id, dto.ApprovedBy, dto.Remarks);

                return Ok(new
                {
                    Success = true,
                    Message = "Approved Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // POST: api/ExpenseApproval/reject/5
        [HttpPost("reject/{id:int}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Reject(
            int id,
            [FromBody] ApprovalActionDto dto)
        {
            try
            {
                await _service.RejectAsync(
                    id,
                    dto.ApprovedBy,
                    dto.Remarks);

                return Ok(new
                {
                    Success = true,
                    Message = "Expense rejected successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // DELETE: api/ExpenseApproval/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok(new
                {
                    Success = true,
                    Message = "Deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
