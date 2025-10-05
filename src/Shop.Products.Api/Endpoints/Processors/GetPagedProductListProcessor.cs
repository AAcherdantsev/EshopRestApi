using FastEndpoints;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.Processors;

public class GetPagedProductListProcessor : IPreProcessor<GetPagedProductListRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<GetPagedProductListRequest> context, CancellationToken ct)
    {
        if (context.Request!.PageNumber < -1)
        {
            context.ValidationFailures.Add(new("BadRequest", "The page number must be equal or greater than -1."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, statusCode: 400, cancellation: ct);
        }
        if (context.Request!.PageSize <= 0)
        {
            context.ValidationFailures.Add(new("BadRequest", "The page size must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, statusCode: 400, cancellation: ct);
        }
    }
}