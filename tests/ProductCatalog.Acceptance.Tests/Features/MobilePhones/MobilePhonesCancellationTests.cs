using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones;

public sealed class MobilePhonesCancellationTests
{
    [Fact]
    public async Task CancelingHttpRequestCancelsMediatorHandler()
    {
        var handler = new CancellableTopMobilePhonesHandler();
        using var baseFactory = new ApplicationFactory(
            "Server=localhost;Database=ProductsCancellationTests;TrustServerCertificate=True");
        using var factory = baseFactory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IRequestHandler<GetTopMobilePhonesQuery, IReadOnlyList<TopMobilePhoneDto>>>();
                services.AddSingleton<IRequestHandler<GetTopMobilePhonesQuery, IReadOnlyList<TopMobilePhoneDto>>>(handler);
            }));
        using var client = factory.CreateClient();
        using var cancellation = new CancellationTokenSource();

        var responseTask = client.GetAsync("/mobile-phones/top", cancellation.Token);
        await handler.Started.Task.WaitAsync(TimeSpan.FromSeconds(10));

        cancellation.Cancel();

        await handler.Canceled.Task.WaitAsync(TimeSpan.FromSeconds(10));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => responseTask);
    }

    private sealed class CancellableTopMobilePhonesHandler
        : IRequestHandler<GetTopMobilePhonesQuery, IReadOnlyList<TopMobilePhoneDto>>
    {
        public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource Canceled { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public async Task<IReadOnlyList<TopMobilePhoneDto>> Handle(
            GetTopMobilePhonesQuery request,
            CancellationToken cancellationToken)
        {
            Started.SetResult();

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Canceled.SetResult();
                throw;
            }

            return [];
        }
    }
}
