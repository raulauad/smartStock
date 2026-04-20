using FluentValidation;
using System.Net;

namespace smartStock.Api.Application.Features.Admin.Commands.RegistrarAdmin;

public sealed class RegistrarAdministradorCommandValidator
    : AbstractValidator<RegistrarAdministradorCommand>
{
    private const int DniLongitud = 8;

    public RegistrarAdministradorCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .Matches(@"^[A-ZÁÉÍÓÚÑÜ]")
                .WithMessage("El nombre debe comenzar con letra mayúscula.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El formato del email no es válido.")
            .MustAsync(DominioExisteAsync)
                .WithMessage("La dirección de correo no existe o su dominio no es alcanzable.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es requerido.")
            .Matches(@"^\+?[0-9]{1,4}[\s\-]?\(?\d{1,5}\)?[\s\-]?\d{4,10}$")
                .WithMessage("El teléfono debe incluir código de zona (internacional o local) y número.");

        RuleFor(x => x.Dni)
            .NotEmpty().WithMessage("El DNI es requerido.")
            .Length(DniLongitud)
                .WithMessage($"El DNI debe tener exactamente {DniLongitud} caracteres.");

        RuleFor(x => x.Contrasena)
            .NotEmpty()         .WithMessage("La contraseña es requerida.")
            .MinimumLength(8)   .WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]")   .WithMessage("La contraseña debe contener al menos una mayúscula.")
            .Matches("[0-9]")   .WithMessage("La contraseña debe contener al menos un dígito.")
            .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial.");
    }

    private static async Task<bool> DominioExisteAsync(string email, CancellationToken ct)
    {
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(3));
            var dominio     = email.Split('@').Last();
            var direcciones = await Dns.GetHostAddressesAsync(dominio, cts.Token);
            return direcciones.Length > 0;
        }
        catch
        {
            return false;
        }
    }
}
