using FluentValidation;
using Microsoft.AspNetCore.Identity;
using smartStock.Api.Domain.Models;

namespace smartStock.Api.Application.Features.Commands.Usuarios.CambiarContrasena;

public sealed class CambiarContrasenaCommandValidator : AbstractValidator<CambiarContrasenaCommand>
{
    private readonly UserManager<Usuario> _userManager;

    public CambiarContrasenaCommandValidator(UserManager<Usuario> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.ContrasenaActual)
            .NotEmpty().WithMessage("La contraseña actual es requerida.");

        // FA2: nueva contraseña debe cumplir las reglas de seguridad (delegado a Identity PasswordValidators)
        RuleFor(x => x.NuevaContrasena)
            .NotEmpty().WithMessage("La nueva contraseña es requerida.")
            .CustomAsync(ValidarPasswordConIdentityAsync);

        // FA3: nueva contraseña y confirmación deben coincidir
        RuleFor(x => x.ConfirmacionContrasena)
            .NotEmpty().WithMessage("La confirmación de contraseña es requerida.")
            .Equal(x => x.NuevaContrasena)
                .WithMessage("La nueva contraseña y su confirmación no coinciden.");
    }

    private async Task ValidarPasswordConIdentityAsync(
        string password,
        ValidationContext<CambiarContrasenaCommand> context,
        CancellationToken ct)
    {
        var usuario = new Usuario { UserName = string.Empty };

        foreach (var validator in _userManager.PasswordValidators)
        {
            var resultado = await validator.ValidateAsync(_userManager, usuario, password);
            foreach (var error in resultado.Errors)
                context.AddFailure(nameof(CambiarContrasenaCommand.NuevaContrasena), error.Description);
        }
    }
}
