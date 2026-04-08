using FluentValidation;

namespace smartStock.Api.Application.Features.Commands.Usuarios.IniciarSesion;

public sealed class IniciarSesionCommandValidator : AbstractValidator<IniciarSesionCommand>
{
    public IniciarSesionCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.");

        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es requerida.");
    }
}
