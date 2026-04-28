using FluentValidation;
using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Application.Features.Ventas.Commands.RegistrarVenta;

public sealed class RegistrarVentaCommandValidator : AbstractValidator<RegistrarVentaCommand>
{
    public RegistrarVentaCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Debe incluir al menos un ítem en la venta.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductoId)
                .NotEmpty().WithMessage("El ID del producto es requerido.");

            item.RuleFor(i => i.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");
        });

        RuleFor(x => x.FormaPago)
            .IsInEnum().WithMessage("La forma de pago no es válida.");

        RuleFor(x => x.MontoRecibido)
            .GreaterThanOrEqualTo(0).WithMessage("El monto recibido no puede ser negativo.")
            .When(x => x.MontoRecibido.HasValue);

        // FA5: MontoRecibido < Total se valida en el handler donde se conoce el total calculado
    }
}
