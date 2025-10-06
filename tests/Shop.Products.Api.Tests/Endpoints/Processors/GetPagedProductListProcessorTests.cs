using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Tests.Endpoints.Processors;

[TestFixture]
public class GetPagedProductListProcessorTests
{
    [SetUp]
    public void SetUp()
    {
        _processor = new GetPagedProductListProcessor();
        _failures = new List<ValidationFailure>();
        _responseMock = new Mock<HttpResponse>();
        _httpContextMock = new Mock<HttpContext>();
        _httpContextMock.SetupGet(x => x.Response).Returns(_responseMock.Object);
        _contextMock = new Mock<IPreProcessorContext<GetPagedProductListRequest>>();
        _contextMock.SetupGet(x => x.ValidationFailures).Returns(_failures);
        _contextMock.SetupGet(x => x.HttpContext).Returns(_httpContextMock.Object);
        _validRequest = new GetPagedProductListRequest
        {
            PageNumber = 1,
            PageSize = 10
        };
    }

    private GetPagedProductListProcessor _processor;
    private Mock<IPreProcessorContext<GetPagedProductListRequest>> _contextMock;
    private GetPagedProductListRequest _validRequest;
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
    public async Task PreProcessAsync_PageNumberLessThanMinusOne_AddsFailure()
    {
        var req = new GetPagedProductListRequest
        {
            PageNumber = -2,
            PageSize = _validRequest.PageSize
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

        Assert.That(_failures, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "PageNumber"));
    }

    [Test]
    public async Task PreProcessAsync_PageSizeLessThanOrEqualZero_AddsFailure()
    {
        var req = new GetPagedProductListRequest
        {
            PageNumber = _validRequest.PageNumber,
            PageSize = 0
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

        Assert.That(_failures, Has.Some.Matches<ValidationFailure>(f => f.PropertyName == "PageSize"));
    }
}