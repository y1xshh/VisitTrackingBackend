using FluentValidation;
using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Validators
{
    public class VisitApprovalValidator : AbstractValidator<VisitApprovalRequestDto>
    {
        public VisitApprovalValidator()
        {
            RuleFor(x => x.ForwardTo)
                .MaximumLength(150);

            RuleFor(x => x.Remark)
                .MaximumLength(500);
        }
    }
}
