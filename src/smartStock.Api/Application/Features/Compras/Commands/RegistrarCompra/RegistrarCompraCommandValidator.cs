using FluentValidation;
using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Application.Features.Compras.Commands.RegistrarCompra;

public sealed class RegistrarCompraCommandValidator : AbstractValidator<RegistrarCompraCommand>
{
    public RegistrarCompraCommandValidator()
    {
        RuleFor(x => x.ProveedorId)
            .NotEmpty().WithMessage("El proveedor es requerido.");

        RuleFor(x => x.FechaCompra)
            .NotEmpty().WithMessage("La fecha de la compra es requerida.")
            .LessThanOrEqualTo(_ => DateTime.UtcNow.Date.AddDays(1).AddTicks(-1))
            .WithMessage("La fecha de la compra no puede ser futura.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Debe agregar al menos un ítem a la compra.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductoId)
                .NotEmpty().WithMessage("El producto del ítem es requerido.");

            item.RuleFor(i => i.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad del ítem debe ser mayor a cero.");

            item.RuleFor(i => i.PrecioCompra)
                .GreaterThan(0).WithMessage("El precio de compra del ítem debe ser mayor a cero.");
        });

        RuleFor(x => x.NumeroComprobante)
            .MaximumLength(30).WithMessage("El número de comprobante no puede superar los 30 caracteres.")
            .MinimumLength(1).WithMessage("El número de comprobante no puede estar vacío si se indica.")
            .When(x => x.NumeroComprobante is not null);

        // NumeroComprobante y TipoComprobante son co-dependientes
        RuleFor(x => x.TipoComprobante)
            .NotNull().WithMessage("El tipo de comprobante es requerido cuando se indica número de comprobante.")
            .When(x => x.NumeroComprobante is not null);

        RuleFor(x => x.NumeroComprobante)
            .NotNull().WithMessage("El número de comprobante es requerido cuando se indica tipo de comprobante.")
            .When(x => x.TipoComprobante.HasValue);

        RuleFor(x => x.FechaComprobante)
            .LessThanOrEqualTo(_ => DateTime.UtcNow.Date.AddDays(1).AddTicks(-1))
            .WithMessage("La fecha del comprobante no puede ser futura.")
            .When(x => x.FechaComprobante.HasValue);
    }
}
