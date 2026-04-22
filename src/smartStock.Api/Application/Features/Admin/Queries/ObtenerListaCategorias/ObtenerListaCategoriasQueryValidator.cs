using FluentValidation;

namespace smartStock.Api.Application.Features.Admin.Queries.ObtenerListaCategorias;

public sealed class ObtenerListaCategoriasQueryValidator : AbstractValidator<ObtenerListaCategoriasQuery>
{
    public ObtenerListaCategoriasQueryValidator()
    {
        RuleFor(x => x.Busqueda)
            .MaximumLength(100).WithMessage("El término de búsqueda no puede superar los 100 caracteres.")
            .When(x => x.Busqueda is not null);
    }
}
