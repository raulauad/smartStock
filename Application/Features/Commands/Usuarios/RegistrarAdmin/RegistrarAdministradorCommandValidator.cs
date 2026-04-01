using FluentValidation;
using Microsoft.AspNetCore.Identity;
using smartStock.Domain.Models;
using System.Net;

namespace smartStock.Application.Features.Commands.Usuarios.RegistrarAdmin;

public sealed class RegistrarAdministradorCommandValidator
    : AbstractValidator<RegistrarAdministradorCommand>
{
    private const int DniLongitud = 8;

    private readonly UserManager<Usuario> _userManager;

    public RegistrarAdministradorCommandValidator(UserManager<Usuario> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .Matches(@"^[A-ZÁÉÍÓÚÑÜ]")
                .WithMessage("El nombre debe comenzar con letra mayúscula.");

        // Identity UserValidators cubre: formato de email y unicidad
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .MustAsync(DominioExisteAsync)
                .WithMessage("La dirección de correo no existe o su dominio no es alcanzable.")
            .CustomAsync(ValidarEmailConIdentityAsync);

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es requerido.")
            .Matches(@"^\+?[0-9]{1,4}[\s\-]?\(?\d{1,5}\)?[\s\-]?\d{4,10}$")
                .WithMessage("El teléfono debe incluir código de zona (internacional o local) y número.");

        RuleFor(x => x.Dni)
            .NotEmpty().WithMessage("El DNI es requerido.")
            .Length(DniLongitud)
                .WithMessage($"El DNI debe tener exactamente {DniLongitud} caracteres.");

        // Identity PasswordValidators cubre: longitud, mayúscula, dígito, no-alfanumérico
        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .CustomAsync(ValidarPasswordConIdentityAsync);
    }

    private async Task ValidarEmailConIdentityAsync(
        string email,
        ValidationContext<RegistrarAdministradorCommand> context,
        CancellationToken ct)
    {
        var usuario = new Usuario { UserName = email, Email = email };

        foreach (var validator in _userManager.UserValidators)
        {
            var resultado = await validator.ValidateAsync(_userManager, usuario);
            foreach (var error in resultado.Errors)
                context.AddFailure(nameof(RegistrarAdministradorCommand.Email), error.Description);
        }
    }

    private async Task ValidarPasswordConIdentityAsync(
        string password,
        ValidationContext<RegistrarAdministradorCommand> context,
        CancellationToken ct)
    {
        var usuario = new Usuario { UserName = context.InstanceToValidate.Email };

        foreach (var validator in _userManager.PasswordValidators)
        {
            var resultado = await validator.ValidateAsync(_userManager, usuario, password);
            foreach (var error in resultado.Errors)
                context.AddFailure(nameof(RegistrarAdministradorCommand.Contrasena), error.Description);
        }
    }

    private static async Task<bool> DominioExisteAsync(string email, CancellationToken ct)
    {
        try
        {
            var dominio = email.Split('@').Last();
            var direcciones = await Dns.GetHostAddressesAsync(dominio, ct);
            return direcciones.Length > 0;
        }
        catch
        {
            return false;
        }
    }
}
