using FluentValidation;

namespace smartStock.Api.Application.Features.Admin.Commands.AgregarCodigoProducto;

public sealed class AgregarCodigoProductoCommandValidator : AbstractValidator<AgregarCodigoProductoCommand>
{
    public AgregarCodigoProductoCommandValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código no puede estar vacío.")
            .MinimumLength(1).WithMessage("El código debe tener al menos 1 carácter.")
            .MaximumLength(50).WithMessage("El código no puede superar los 50 caracteres.")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("El código solo puede contener caracteres alfanuméricos, guiones o guiones bajos.");

        RuleFor(x => x.Factor)
            .GreaterThan(0).WithMessage("El factor debe ser mayor a cero.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(50).WithMessage("La descripción no puede superar los 50 caracteres.")
            .When(x => x.Descripcion is not null);
    }
}
