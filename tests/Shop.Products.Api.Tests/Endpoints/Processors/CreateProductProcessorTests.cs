using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Tests.Endpoints.Processors;

[TestFixture]
public class CreateProductProcessorTests
{
    [SetUp]
    public void SetUp()
    {
        _processor = new CreateProductProcessor();
        _failures = [];
        _responseMock = new Mock<HttpResponse>();
        _httpContextMock = new Mock<HttpContext>();
        _httpContextMock.SetupGet(x => x.Response).Returns(_responseMock.Object);
        _contextMock = new Mock<IPreProcessorContext<CreateProductRequest>>();
        _contextMock.SetupGet(x => x.ValidationFailures).Returns(_failures);
        _contextMock.SetupGet(x => x.HttpContext).Returns(_httpContextMock.Object);
        _validRequest = new CreateProductRequest
        {
            Name = "Test Product",
            Price = 10.0m,
            Quantity = 5,
            ImageUrl = "https://example.com/image.jpg",
            Description = "desc"
        };
    }

    private CreateProductProcessor _processor;
    private Mock<IPreProcessorContext<CreateProductRequest>> _contextMock;
    private CreateProductRequest _validRequest;
    private List<ValidationFailure> _failures;
    private Mock<HttpResponse> _responseMock;
    private Mock<HttpContext> _httpContextMock;

    [Test]
    public async Task PreProcessAsync_ValidRequest_NoFailures()
    {
        _contextMock.SetupGet(x => x.Request).Returns(_validRequest);
        await _processor.PreProcessAsync(_contextMock.Object, CancellationToken.None);
        Assert.IsEmpty(_failures);
    }

    [Test]
    public async Task PreProcessAsync_QuantityLessThanOrEqualZero_AddsFailure()
    {
        var req = new CreateProductRequest
        {
            Name = _validRequest.Name,
            Price = _validRequest.Price,
            Quantity = 0,
            ImageUrl = _validRequest.ImageUrl,
            Description = _validRequest.Description
        };
        _contextMock.SetupGet(x => x.Request).Returns(req);

        try
        {
            await _processor.PreProcessAsync(_contextMock.Object, CancellationToken.None);
        }
        catch
        {
            // ignored
        }
        Assert.That(_failures, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "Quantity"));
    }

    [Test]
    public async Task PreProcessAsync_PriceLessThanOrEqualZero_AddsFailure()
    {
        var req = new CreateProductRequest
        {
            Name = _validRequest.Name,
            Price = 0,
            Quantity = _validRequest.Quantity,
            ImageUrl = _validRequest.ImageUrl,
            Description = _validRequest.Description
        };
        _contextMock.SetupGet(x => x.Request).Returns(req);
        try
        {
            await _processor.PreProcessAsync(_contextMock.Object, CancellationToken.None);
        }
        catch
        {
            // ignored
        }

        Assert.That(_failures, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "Price"));
    }

    [Test]
    public async Task PreProcessAsync_NameIsNullOrEmpty_AddsFailure()
    {
        var req = new CreateProductRequest
        {
            Name = "",
            Price = _validRequest.Price,
            Quantity = _validRequest.Quantity,
            ImageUrl = _validRequest.ImageUrl,
            Description = _validRequest.Description
        };
        _contextMock.SetupGet(x => x.Request).Returns(req);
        try
        {
            await _processor.PreProcessAsync(_contextMock.Object, CancellationToken.None);
        }
        catch
        {
            // ignored
        }

        Assert.That(_failures, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "Name"));
    }

    [Test]
    public async Task PreProcessAsync_ImageUrlInvalid_AddsFailure()
    {
        var req = new CreateProductRequest
        {
            Name = _validRequest.Name,
            Price = _validRequest.Price,
            Quantity = _validRequest.Quantity,
            ImageUrl = "not-a-url",
            Description = _validRequest.Description
        };
        _contextMock.SetupGet(x => x.Request).Returns(req);
        try
        {
            await _processor.PreProcessAsync(_contextMock.Object, CancellationToken.None);
        }
        catch
        {
            // ignored
        }

        Assert.That(_failures, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "ImageUrl"));
    }
}