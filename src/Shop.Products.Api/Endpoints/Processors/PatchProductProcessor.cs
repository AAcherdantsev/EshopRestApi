using FastEndpoints;
using FluentValidation.Results;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.Processors;

/// <summary>
///     Validates the incoming <see cref="PatchProductRequest" /> before processing.
/// </summary>
public class PatchProductProcessor : IPreProcessor<PatchProductRequest>
{
    /// <summary>
    ///     Executes pre-processing validation for the product patch request.
    /// </summary>
    /// <param name="context">The pre-processing context containing the request and validation data.</param>
    /// <param name="ct">The cancellation token.</param>
    public async Task PreProcessAsync(IPreProcessorContext<PatchProductRequest> context, CancellationToken ct)
    {
        if (context.Request!.NewQuantity <= 0)
        {
            context.ValidationFailures.Add(new ValidationFailure("BadRequest", "The quantity must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }
    }
}