using FastEndpoints;
using FluentValidation.Results;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.Processors;

/// <summary>
///     Validates the incoming <see cref="GetPagedProductListRequest" /> before processing.
/// </summary>
public class GetPagedProductListProcessor : IPreProcessor<GetPagedProductListRequest>
{
    /// <summary>
    ///     Executes pre-processing validation for pagination request parameters.
    /// </summary>
    /// <param name="context">The pre-processing context containing the request and validation errors.</param>
    /// <param name="ct">The cancellation token.</param>
    public async Task PreProcessAsync(IPreProcessorContext<GetPagedProductListRequest> context, CancellationToken ct)
    {
        if (context.Request!.PageNumber < -1)
        {
            context.ValidationFailures.Add(new ValidationFailure(nameof(GetPagedProductListRequest.PageNumber),
                "The page number must be equal or greater than -1."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }

        if (context.Request!.PageSize <= 0)
        {
            context.ValidationFailures.Add(new ValidationFailure(nameof(GetPagedProductListRequest.PageSize),
                "The page size must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }
    }
}