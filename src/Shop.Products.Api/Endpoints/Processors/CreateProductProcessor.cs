using FastEndpoints;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.Processors;

public class CreateProductProcessor : IPreProcessor<CreateProductRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<CreateProductRequest> context, CancellationToken ct)
    {
        if (context.Request!.Quantity <= 0)
        {
            context.ValidationFailures.Add(new("BadRequest", "The quantity must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, statusCode: 400, cancellation: ct);
        }
        
        if (context.Request!.Price <= 0)
        {
            context.ValidationFailures.Add(new("BadRequest", "The price must be greater than 0."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, statusCode: 400, cancellation: ct);
        }
        
        if (string.IsNullOrEmpty(context.Request!.Name))
        {
            context.ValidationFailures.Add(new("BadRequest", "The name must not be empty."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, statusCode: 400, cancellation: ct);
        }
        
        if (string.IsNullOrEmpty(context.Request!.ImageUrl))
        {
            context.ValidationFailures.Add(new("BadRequest", "The image url must not be empty."));
            await context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, statusCode: 400, cancellation: ct);
        }
    }
}