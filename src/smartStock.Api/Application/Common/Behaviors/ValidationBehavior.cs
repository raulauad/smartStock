using FluentValidation;
using MediatR;

namespace smartStock.Api.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var contexto = new ValidationContext<TRequest>(request);

        var resultados = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(contexto, cancellationToken)));

        var fallos = resultados
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (fallos.Count > 0)
            throw new ValidationException(fallos);

        return await next();
    }
}
