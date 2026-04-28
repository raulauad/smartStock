using FluentValidation;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarCategoria;

public sealed class EditarCategoriaCommandValidator : AbstractValidator<EditarCategoriaCommand>
{
    public EditarCategoriaCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.")
            .Matches(@"^[\p{Lu}]").WithMessage("El nombre debe comenzar con letra mayúscula.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(250).WithMessage("La descripción no puede superar los 250 caracteres.")
            .When(x => x.Descripcion is not null);
    }
}
