using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.CQRS.Users.Validators;

public class RegisterValidator : AbstractValidator<Register.Command>
{
    public RegisterValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(255).WithMessage("Cannot be more than 255");

        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(70)
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty()
            .MaximumLength(50);
    }
}
