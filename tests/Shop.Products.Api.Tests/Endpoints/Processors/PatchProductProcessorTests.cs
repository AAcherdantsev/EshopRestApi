using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Tests.Endpoints.Processors;

[TestFixture]
public class PatchProductProcessorTests
{
    [SetUp]
    public void SetUp()
    {
        _processor = new PatchProductProcessor();
        _failures = new List<ValidationFailure>();
        _responseMock = new Mock<HttpResponse>();
        _httpContextMock = new Mock<HttpContext>();
        _httpContextMock.SetupGet(x => x.Response).Returns(_responseMock.Object);
        _contextMock = new Mock<IPreProcessorContext<PatchProductRequest>>();
        _contextMock.SetupGet(x => x.ValidationFailures).Returns(_failures);
        _contextMock.SetupGet(x => x.HttpContext).Returns(_httpContextMock.Object);
        _validRequest = new PatchProductRequest
        {
            NewQuantity = 5
        };
    }

    private PatchProductProcessor _processor;
    private Mock<IPreProcessorContext<PatchProductRequest>> _contextMock;
    private PatchProductRequest _validRequest;
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
    public async Task PreProcessAsync_NewQuantityLessThanOrEqualZero_AddsFailure()
    {
        var req = new PatchProductRequest
        {
            NewQuantity = 0
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

        Assert.That(_failures, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "NewQuantity"));
    }
}