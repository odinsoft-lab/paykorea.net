using System.Text.Json.Serialization;

namespace PayKorea.Net;

// 공통 DTO
public record PaymentRequest(
	string OrderId,
	decimal Amount,
	string Currency,
	string Description,
	string CustomerEmail
);

public record PaymentResult(
	string OrderId,
	string Provider,
	string Status,
	string? TransactionId,
	string? FailureReason
)
{
	[JsonIgnore]
	public bool IsApproved => Status.Equals("APPROVED", StringComparison.OrdinalIgnoreCase);
}

// Provider 인터페이스
public interface IPaymentProvider
{
	string Name { get; }

	Task<PaymentResult> AuthorizeAsync(PaymentRequest request, CancellationToken ct = default);
	Task<PaymentResult> CaptureAsync(string orderId, string transactionId, CancellationToken ct = default);
	Task<PaymentResult> CancelAsync(string orderId, string? transactionId = null, CancellationToken ct = default);
	Task<PaymentResult> RefundAsync(string orderId, string transactionId, decimal amount, CancellationToken ct = default);
}

// Mock Provider (샘플)
public sealed class MockPaymentProvider : IPaymentProvider
{
	public string Name => "Mock";

	public Task<PaymentResult> AuthorizeAsync(PaymentRequest request, CancellationToken ct = default)
		=> Task.FromResult(new PaymentResult(request.OrderId, Name, "APPROVED", Guid.NewGuid().ToString("N"), null));

	public Task<PaymentResult> CaptureAsync(string orderId, string transactionId, CancellationToken ct = default)
		=> Task.FromResult(new PaymentResult(orderId, Name, "CAPTURED", transactionId, null));

	public Task<PaymentResult> CancelAsync(string orderId, string? transactionId = null, CancellationToken ct = default)
		=> Task.FromResult(new PaymentResult(orderId, Name, "CANCELED", transactionId, null));

	public Task<PaymentResult> RefundAsync(string orderId, string transactionId, decimal amount, CancellationToken ct = default)
		=> Task.FromResult(new PaymentResult(orderId, Name, "REFUNDED", transactionId, null));
}

// SDK 파사드
public sealed class PayKoreaClient
{
	private readonly IPaymentProvider _provider;
	public PayKoreaClient(IPaymentProvider provider) => _provider = provider;

	public Task<PaymentResult> AuthorizeAsync(PaymentRequest request, CancellationToken ct = default)
		=> _provider.AuthorizeAsync(request, ct);
	public Task<PaymentResult> CaptureAsync(string orderId, string transactionId, CancellationToken ct = default)
		=> _provider.CaptureAsync(orderId, transactionId, ct);
	public Task<PaymentResult> CancelAsync(string orderId, string? transactionId = null, CancellationToken ct = default)
		=> _provider.CancelAsync(orderId, transactionId, ct);
	public Task<PaymentResult> RefundAsync(string orderId, string transactionId, decimal amount, CancellationToken ct = default)
		=> _provider.RefundAsync(orderId, transactionId, amount, ct);
}
 
