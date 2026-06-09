using FluentValidation;
using VisitTracking.Application.DTOs;

namespace VisitTracking.Application.Validators
{
    public class VisitApprovalValidator : AbstractValidator<VisitApprovalRequestDto>
    {
        private static readonly string[] ValidActions = { "approve", "approved", "reject", "rejected", "forward", "forwarded", "forwardedtorm", "forwardedtomanager" };

        public VisitApprovalValidator()
        {
            RuleFor(x => x.Action)
                .NotEmpty()
                .WithMessage("Action is required.")
                .Must(x => ValidActions.Contains((x ?? string.Empty).Trim().ToLowerInvariant()))
                .WithMessage($"Action must be one of: {string.Join(", ", ValidActions)}");

            RuleFor(x => x.ForwardTo)
                .MaximumLength(150);

            RuleFor(x => x.Remark)
                .MaximumLength(500);
        }
    }
}
