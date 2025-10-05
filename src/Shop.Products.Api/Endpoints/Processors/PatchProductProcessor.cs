using FastEndpoints;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.Processors;

public class PatchProductProcessor : IPreProcessor<PatchProductRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<PatchProductRequest> context, CancellationToken ct)
    {
        if (context.Request!.NewQuantity <= 0)
        {
            context.ValidationFailures.Add(new("BadRequest", "The quantity must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, statusCode: 400, cancellation: ct);
        }
    }
}