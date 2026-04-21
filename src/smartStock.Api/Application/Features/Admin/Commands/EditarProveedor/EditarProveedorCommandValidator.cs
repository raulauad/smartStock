using FluentValidation;
using smartStock.Api.Application.Common.Validators;

namespace smartStock.Api.Application.Features.Admin.Commands.EditarProveedor;

public sealed class EditarProveedorCommandValidator : AbstractValidator<EditarProveedorCommand>
{
    public EditarProveedorCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Cuit)
            .Matches(@"^\d{11}$").WithMessage("El CUIT debe contener exactamente 11 dígitos.")
            .When(x => !string.IsNullOrWhiteSpace(x.Cuit));

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es requerido.")
            .Matches(@"^\+?[0-9]{1,4}[\s\-]?\(?\d{1,5}\)?[\s\-]?\d{4,10}$")
                .WithMessage("El teléfono debe incluir código de zona (internacional o local) y número.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El formato del email no es válido.")
            .MustAsync(EmailDomainValidator.DominioExisteAsync)
                .WithMessage("La dirección de correo no existe o su dominio no es alcanzable.");

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

        RuleFor(x => x.Observaciones)
            .MaximumLength(500).WithMessage("Las observaciones no pueden superar los 500 caracteres.")
            .When(x => x.Observaciones is not null);
    }
}
