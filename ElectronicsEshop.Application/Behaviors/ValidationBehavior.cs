using Ardalis.Specification;
using FluentValidation;
using MediatR;

namespace ElectronicsEshop.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var failtures = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f is not null)
            .ToList();

        if(failtures.Count != 0)
        {
            throw new ValidationException(failtures);
        }

        return await next(cancellationToken);
    }
}
