using GT.Domain.Entites;
using FluentValidation;

namespace GT.Application.Validator;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator(){

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("O nome de usuário é obrigatório.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
    }
}
