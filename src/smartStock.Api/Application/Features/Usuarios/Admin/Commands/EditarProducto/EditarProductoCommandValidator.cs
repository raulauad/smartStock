using FluentValidation;
using smartStock.Api.Domain.Enums;

namespace smartStock.Api.Application.Features.Usuarios.Admin.Commands.EditarProducto;

public sealed class EditarProductoCommandValidator : AbstractValidator<EditarProductoCommand>
{
    public EditarProductoCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
            .Matches(@"^[\p{Lu}]").WithMessage("El nombre debe comenzar con letra mayúscula.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("La categoría es requerida.");

        RuleFor(x => x.PrecioCosto)
            .GreaterThan(0).WithMessage("El precio de costo debe ser mayor a cero.");

        RuleFor(x => x.PrecioVenta)
            .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a cero.");

        RuleFor(x => x.StockMinimo)
            .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo.")
            .Must((cmd, stock) => cmd.UnidadMedida != UnidadMedida.Unidad || stock == Math.Floor(stock))
            .WithMessage("Para la unidad de medida 'Unidad', el stock mínimo debe ser un número entero.");
    }
}
