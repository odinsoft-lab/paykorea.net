using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using PayKorea.Net.Toss;

namespace PayKorea.Tests.Toss;

public class TossPaymentsClientTests
{
    [Fact]
    public async Task ConfirmAsync_ErrorResponse_ThrowsHttpRequestException()
    {
        // Arrange: Fake HttpMessageHandler returning error JSON
        var handler = new FakeHandler((req, ct) =>
        {
            var json = "{\"code\":\"REJECT_CARD_PAYMENT\",\"message\":\"한도초과 혹은 잔액부족으로 결제에 실패했습니다.\"}";
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            });
        });
        var http = new HttpClient(handler);
        var opts = new TossPaymentsOptions { SecretKey = "test_sk_xxx" };
        var client = new TossPaymentsClient(http, opts);

        // Act / Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => client.ConfirmAsync(new TossConfirmRequest("pk", "ord", 1000)));
    }

    private sealed class FakeHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;
        public FakeHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler) => _handler = handler;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => _handler(request, cancellationToken);
    }
}
