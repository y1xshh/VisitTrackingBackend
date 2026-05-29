using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitTracking.Domain.Entities
{
    [Table("expenseapprovals")]
    public class ExpenseApproval
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("VisitId")]
        public int? VisitId { get; set; }

        [Column("SubmittedBy")]
        public int? SubmittedBy { get; set; }

        [Column("ApprovedBy")]
        public int? ApprovedBy { get; set; }

        [Column("ApprovalStatus")]
        public string? ApprovalStatus { get; set; }

        [Column("ApprovalRemarks")]
        public string? ApprovalRemarks { get; set; }

        [Column("SubmittedAt")]
        public DateTime? SubmittedAt { get; set; }

        [Column("ApprovedAt")]
        public DateTime? ApprovedAt { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; }

        [Column("inserted_by")]
        public string? InsertedBy { get; set; }

        [Column("inserted_date")]
        public DateTime? InsertedDate { get; set; }

        [Column("updated_by")]
        public string? UpdatedBy { get; set; }

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
