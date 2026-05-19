using FluentValidation;
using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Validators
{
    public class VisitApprovalValidator : AbstractValidator<VisitApprovalRequestDto>
    {
        public VisitApprovalValidator()
        {
            RuleFor(x => x.Remark)
                .MaximumLength(500);

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Remark))
                .When(x => !x.IsApproved)
                .WithMessage("Remark is required when rejecting visit.");
        }
    }
}
