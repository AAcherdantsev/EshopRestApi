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
        s.SchemaSettings.SchemaNameGenerator = new DefaultSchemaNameGenerator();
    };
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
