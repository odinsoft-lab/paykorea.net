using PayKorea.Net;

namespace PayKorea.Net.Tests;

public class PayKoreaClientTests
{
    [Fact]
    public async Task Authorize_WithMockProvider_ReturnsApproved()
    {
        var provider = new MockPaymentProvider();
        var client = new PayKoreaClient(provider);
        var request = new PaymentRequest(
            OrderId: Guid.NewGuid().ToString("N"),
            Amount: 1000m,
            Currency: "KRW",
            Description: "Test",
            CustomerEmail: "user@example.com"
        );

        var result = await client.AuthorizeAsync(request);

        Assert.True(result.IsApproved);
        Assert.Equal(request.OrderId, result.OrderId);
        Assert.Equal("Mock", result.Provider);
    }
}
