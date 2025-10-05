using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using NJsonSchema.Generation;
using Shop.Products.Api.Mapping;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Infrastructure.Persistence;
using Shop.Products.Infrastructure.Persistence.Repositories;
using Shop.Products.Infrastructure.Services;

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

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ProductsDb");
    options
        .UseSqlServer(connectionString, o =>
        {
            o.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";
    c.Versioning.PrependToRoute = true;
});
app.UseSwaggerGen();

using (var scope = app.Services.CreateScope())
{
    await DatabaseInitializer.InitializeAsync(scope.ServiceProvider, addProductExamples: true);
}

app.Run();
