using FluentValidation;

namespace smartStock.Api.Application.Features.Empleados.Commands.CambiarContrasena;

public sealed class CambiarContrasenaCommandValidator : AbstractValidator<CambiarContrasenaCommand>
{
    public CambiarContrasenaCommandValidator()
    {
        RuleFor(x => x.ContrasenaActual)
            .NotEmpty().WithMessage("La contraseña actual es requerida.");

        // FA2: nueva contraseña debe cumplir las reglas de seguridad
        RuleFor(x => x.NuevaContrasena)
            .NotEmpty()         .WithMessage("La nueva contraseña es requerida.")
            .MinimumLength(8)   .WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]")   .WithMessage("La contraseña debe contener al menos una mayúscula.")
            .Matches("[0-9]")   .WithMessage("La contraseña debe contener al menos un dígito.")
            .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.");

        // FA3: nueva contraseña y confirmación deben coincidir
        RuleFor(x => x.ConfirmacionContrasena)
            .NotEmpty().WithMessage("La confirmación de contraseña es requerida.")
            .Equal(x => x.NuevaContrasena)
                .WithMessage("La nueva contraseña y su confirmación no coinciden.");
    }
}
