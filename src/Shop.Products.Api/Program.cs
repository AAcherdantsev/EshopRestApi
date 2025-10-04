using FastEndpoints;
using FastEndpoints.Swagger;
using NJsonSchema.Generation;
using Shop.Products.Api.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddAutoMapper(e => e.AddProfile<MappingProfile>());

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "v1";
        s.Title = "Products API V1";
        s.Version = "v1";
        s.SchemaSettings.SchemaNameGenerator = new DefaultSchemaNameGenerator();
    };
    o.MaxEndpointVersion = 1;
    o.MinEndpointVersion = 1;
});

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "v2";
        s.Title = "Products API V2";
        s.Version = "v2";
        s.SchemaSettings.SchemaNameGenerator = new DefaultSchemaNameGenerator();
    };
    
    o.MaxEndpointVersion = 2;
    o.MinEndpointVersion = 2;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";
    c.Versioning.PrependToRoute = true;
});
app.UseSwaggerGen();

app.Run();
