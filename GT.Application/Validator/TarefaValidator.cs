using GT.Domain.Entites;
using FluentValidation;

namespace GT.Application.Validator;

public class TarefaValidator : AbstractValidator<Tarefa>
{
    public TarefaValidator()
    {
        RuleFor(tarefa => tarefa.Titulo)
            .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
            .MaximumLength(100).WithMessage("O título da tarefa não pode exceder 100 caracteres.");

        RuleFor(tarefa => tarefa.Status)
            .IsInEnum().WithMessage("O status da tarefa é inválido.");
    }
}