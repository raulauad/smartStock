using FluentValidation;

namespace smartStock.Api.Application.Features.Ventas.Commands.AnularVenta;

public sealed class AnularVentaCommandValidator : AbstractValidator<AnularVentaCommand>
{
    public AnularVentaCommandValidator()
    {
        RuleFor(x => x.MotivoAnulacion)
            .NotEmpty().WithMessage("El motivo de anulación es requerido.")
            .MinimumLength(5).WithMessage("El motivo debe tener al menos 5 caracteres.")
            .MaximumLength(500).WithMessage("El motivo no puede superar los 500 caracteres.");
    }
}
