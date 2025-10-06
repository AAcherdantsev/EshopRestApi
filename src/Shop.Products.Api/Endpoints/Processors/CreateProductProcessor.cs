using FastEndpoints;
using FluentValidation.Results;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.Processors;

/// <summary>
///     Validates the incoming <see cref="CreateProductRequest" /> before processing.
/// </summary>
/// <remarks>
///     Ensures that quantity, price, name, and image URL meet minimum requirements.
/// </remarks>
public class CreateProductProcessor : IPreProcessor<CreateProductRequest>
{
    /// <summary>
    ///     Executes pre-processing validation for the incoming request.
    /// </summary>
    /// <param name="context">The pre-processing context containing the request and validation information.</param>
    /// <param name="ct">The cancellation token.</param>
    public async Task PreProcessAsync(IPreProcessorContext<CreateProductRequest> context, CancellationToken ct)
    {
        if (context.Request!.Quantity <= 0)
        {
            context.ValidationFailures.Add(new ValidationFailure(nameof(CreateProductRequest.Quantity), "The quantity must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }

        if (context.Request!.Price <= 0)
        {
            context.ValidationFailures.Add(new ValidationFailure(nameof(CreateProductRequest.Price), "The price must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }

        if (string.IsNullOrEmpty(context.Request!.Name))
        {
            context.ValidationFailures.Add(new ValidationFailure(nameof(CreateProductRequest.Name), "The name must not be empty."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }

        if (!Uri.TryCreate(context.Request!.ImageUrl, UriKind.Absolute, out _))
        {
            context.ValidationFailures.Add(new ValidationFailure(nameof(CreateProductRequest.ImageUrl), "The image url is not valid."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }
    }
}