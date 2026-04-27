using FluentValidation;
using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Application.Features.Compras.Commands.AjusteManualStock;

public sealed class AjusteManualStockCommandValidator : AbstractValidator<AjusteManualStockCommand>
{
    public AjusteManualStockCommandValidator()
    {
        RuleFor(x => x.ProductoId)
            .NotEmpty().WithMessage("El producto es requerido.");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo es requerido.")
            .MinimumLength(5).WithMessage("El motivo debe tener al menos 5 caracteres.")
            .MaximumLength(250).WithMessage("El motivo no puede superar los 250 caracteres.");
    }
}
