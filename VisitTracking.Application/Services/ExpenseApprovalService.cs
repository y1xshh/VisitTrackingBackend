using VisitTracking.Application.DTOs;
using VisitTracking.Application.Interface;
using VisitTracking.Domain.Entities;

public class ExpenseApprovalService : IExpenseApprovalService
{
    private readonly IExpenseApprovalRepository _repo;

    public ExpenseApprovalService(IExpenseApprovalRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ExpenseApprovalDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new ExpenseApprovalDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            SubmittedBy = x.SubmittedBy,
            ApprovedBy = x.ApprovedBy,
            ApprovalStatus = x.ApprovalStatus,
            ApprovalRemarks = x.ApprovalRemarks,
            SubmittedAt = x.SubmittedAt,
            ApprovedAt = x.ApprovedAt,
            IsActive = x.IsActive
        });
    }

    public async Task<ExpenseApprovalDto?> GetByIdAsync(int id)
    {
        var x = await _repo.GetByIdAsync(id);
        if (x == null) return null;

        return new ExpenseApprovalDto
        {
            Id = x.Id,
            VisitId = x.VisitId,
            SubmittedBy = x.SubmittedBy,
            ApprovedBy = x.ApprovedBy,
            ApprovalStatus = x.ApprovalStatus,
            ApprovalRemarks = x.ApprovalRemarks,
            SubmittedAt = x.SubmittedAt,
            ApprovedAt = x.ApprovedAt,
            IsActive = x.IsActive
        };
    }

    public async Task CreateAsync(ExpenseApprovalDto dto)
    {
        var entity = new Expenseapproval
        {
            VisitId = dto.VisitId,
            SubmittedBy = dto.SubmittedBy,
            ApprovalStatus = "Pending",   // 🔥 default
            SubmittedAt = DateTime.Now,
            IsActive = true,
            InsertedDate = DateTime.Now
        };

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(ExpenseApprovalDto dto)
    {
        var entity = new Expenseapproval
        {
            Id = dto.Id,
            VisitId = dto.VisitId,
            SubmittedBy = dto.SubmittedBy,
            ApprovedBy = dto.ApprovedBy,
            ApprovalStatus = dto.ApprovalStatus,
            ApprovalRemarks = dto.ApprovalRemarks,
            SubmittedAt = dto.SubmittedAt,
            ApprovedAt = dto.ApprovedAt,
            IsActive = dto.IsActive,
            UpdatedDate = DateTime.Now
        };

        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _repo.DeleteAsync(id);
    }

    // 🔥 APPROVE
    public async Task ApproveAsync(int id, int approvedBy, string? remarks)
    {
        var data = await _repo.GetByIdAsync(id);
        if (data == null) throw new Exception("Request not found");

        data.ApprovalStatus = "Approved";
        data.ApprovedBy = approvedBy;
        data.ApprovalRemarks = remarks;
        data.ApprovedAt = DateTime.Now;

        await _repo.UpdateAsync(data);
    }

    // 🔥 REJECT
    public async Task RejectAsync(int id, int approvedBy, string? remarks)
    {
        var data = await _repo.GetByIdAsync(id);
        if (data == null) throw new Exception("Request not found");

        data.ApprovalStatus = "Rejected";
        data.ApprovedBy = approvedBy;
        data.ApprovalRemarks = remarks;
        data.ApprovedAt = DateTime.Now;

        await _repo.UpdateAsync(data);
    }
}