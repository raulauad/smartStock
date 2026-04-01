using FluentValidation;

namespace smartStock.Application.Features.Commands.Usuarios.IniciarSesion;

public sealed class IniciarSesionAdminCommandValidator : AbstractValidator<IniciarSesionAdminCommand>
{
    public IniciarSesionAdminCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.");

        RuleFor(x => x.Contrasena)
            .NotEmpty().WithMessage("La contraseña es requerida.");
    }
}
