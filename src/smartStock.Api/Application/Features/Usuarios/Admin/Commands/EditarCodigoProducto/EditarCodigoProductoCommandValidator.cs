using FluentValidation;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarCodigoProducto;

public sealed class EditarCodigoProductoCommandValidator : AbstractValidator<EditarCodigoProductoCommand>
{
    public EditarCodigoProductoCommandValidator()
    {
        RuleFor(x => x.Factor)
            .GreaterThan(0).WithMessage("El factor debe ser mayor a cero.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(50).WithMessage("La descripción no puede superar los 50 caracteres.")
            .When(x => x.Descripcion is not null);
    }
}
