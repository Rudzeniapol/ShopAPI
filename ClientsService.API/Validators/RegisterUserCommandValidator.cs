using ClientsService.Application.Commands;
using FluentValidation;

namespace ClientsService.API.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private static readonly string[] AllowedRoles = { "ADMIN", "USER" };

    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.RegisterUser.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        
        RuleFor(x => x.RegisterUser.Role)
            .NotEmpty().WithMessage("Роль обязательна")
            .Must(role => AllowedRoles.Contains(role.ToUpper()))
            .WithMessage("Невалидная роль. Разрешённые роли: ADMIN, USER");
    }
}