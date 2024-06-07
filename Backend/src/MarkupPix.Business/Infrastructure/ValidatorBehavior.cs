using FluentValidation;

using MediatR;

namespace MarkupPix.Business.Infrastructure;

/// <summary>
/// Mediatr command validation.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    /// <summary>
    /// Initializes a new instance of the class <see cref="ValidatorBehavior{TRequest,TResponse}"/>.
    /// </summary>
    /// <param name="validator">Validator.</param>
    public ValidatorBehavior(IEnumerable<IValidator<TRequest>?> validators)
    {
        _validator = validators.FirstOrDefault();
    }

    /// <summary>
    /// Command validation.
    /// </summary>
    /// <param name="request">Command.</param>
    /// <param name="next">The handler of the command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _validator?.ValidateAndThrow(request);
        return next();
    }
}