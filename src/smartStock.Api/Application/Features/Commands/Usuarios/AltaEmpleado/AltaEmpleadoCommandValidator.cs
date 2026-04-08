using FluentValidation;
using Microsoft.AspNetCore.Identity;
using smartStock.Api.Domain.Models;
using System.Net;

namespace smartStock.Api.Application.Features.Commands.Usuarios.AltaEmpleado;

public sealed class AltaEmpleadoCommandValidator : AbstractValidator<AltaEmpleadoCommand>
{
    private const int DniMinLongitud = 7;
    private const int DniMaxLongitud = 8;

    private readonly UserManager<Usuario> _userManager;

    public AltaEmpleadoCommandValidator(UserManager<Usuario> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .Matches(@"^[A-ZÁÉÍÓÚÑÜ]")
                .WithMessage("El nombre debe comenzar con letra mayúscula.");

        // Unicidad delegada al handler (FA2 → 409). Aquí solo formato y DNS.
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El formato del email no es válido.")
            .MustAsync(DominioExisteAsync)
                .WithMessage("La dirección de correo no existe o su dominio no es alcanzable.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es requerido.")
            .Matches(@"^\+?[0-9]{1,4}[\s\-]?\(?\d{1,5}\)?[\s\-]?\d{4,10}$")
                .WithMessage("El teléfono debe incluir código de zona (internacional o local) y número.");

        // Unicidad delegada al handler (FA2 → 409). Aquí solo formato.
        RuleFor(x => x.Dni)
            .NotEmpty().WithMessage("El DNI es requerido.")
            .Matches(@"^\d+$").WithMessage("El DNI debe contener solo dígitos.")
            .Length(DniMinLongitud, DniMaxLongitud)
                .WithMessage($"El DNI debe tener entre {DniMinLongitud} y {DniMaxLongitud} dígitos.");

        // Identity PasswordValidators: longitud, mayúscula, dígito, especial
        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .CustomAsync(ValidarPasswordConIdentityAsync);

        RuleFor(x => x.Direccion).NotNull().WithMessage("La dirección es requerida.");

        RuleFor(x => x.Direccion.Pais)
            .NotEmpty().WithMessage("El país es requerido.")
            .MaximumLength(60).WithMessage("El país no puede superar los 60 caracteres.");

        RuleFor(x => x.Direccion.Provincia)
            .NotEmpty().WithMessage("La provincia es requerida.")
            .MaximumLength(60).WithMessage("La provincia no puede superar los 60 caracteres.");

        RuleFor(x => x.Direccion.Localidad)
            .NotEmpty().WithMessage("La localidad es requerida.")
            .MaximumLength(60).WithMessage("La localidad no puede superar los 60 caracteres.");

        RuleFor(x => x.Direccion.CodigoPostal)
            .NotEmpty().WithMessage("El código postal es requerido.")
            .Matches(@"^\d{4}$|^[A-Z]\d{4}[A-Z]{3}$")
                .WithMessage("El código postal debe ser numérico de 4 dígitos (ej: 3400) o formato CPA (ej: W3400BRB).");

        RuleFor(x => x.Direccion.Calle)
            .NotEmpty().WithMessage("La calle es requerida.")
            .MaximumLength(100).WithMessage("La calle no puede superar los 100 caracteres.");

        RuleFor(x => x.Direccion.Numero)
            .NotEmpty().WithMessage("El número de la calle es requerido.")
            .Matches(@"^[0-9a-zA-Z\/\-\s]{1,10}$")
                .WithMessage("El número debe ser numérico o 'S/N' (máx. 10 caracteres).");
    }

    private async Task ValidarPasswordConIdentityAsync(
        string password,
        ValidationContext<AltaEmpleadoCommand> context,
        CancellationToken ct)
    {
        var usuario = new Usuario { UserName = context.InstanceToValidate.Email };

        foreach (var validator in _userManager.PasswordValidators)
        {
            var resultado = await validator.ValidateAsync(_userManager, usuario, password);
            foreach (var error in resultado.Errors)
                context.AddFailure(nameof(AltaEmpleadoCommand.Contrasena), error.Description);
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
