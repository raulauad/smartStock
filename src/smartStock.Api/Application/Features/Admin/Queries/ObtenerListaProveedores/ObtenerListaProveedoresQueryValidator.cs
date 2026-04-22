using FluentValidation;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerListaProveedores;

public sealed class ObtenerListaProveedoresQueryValidator : AbstractValidator<ObtenerListaProveedoresQuery>
{
    public ObtenerListaProveedoresQueryValidator()
    {
        RuleFor(x => x.Busqueda)
            .MaximumLength(100).WithMessage("El término de búsqueda no puede superar los 100 caracteres.")
            .When(x => x.Busqueda is not null);
    }
}
