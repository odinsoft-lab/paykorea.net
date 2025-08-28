namespace PayKorea.Net.Toss;

public sealed class TossPaymentProvider : IPaymentProvider
{
    private readonly TossPaymentsClient _client;
    public TossPaymentProvider(TossPaymentsClient client) => _client = client;

    public string Name => "TossPayments";

    public async Task<PaymentResult> AuthorizeAsync(PaymentRequest request, CancellationToken ct = default)
    {
        // Toss는 위젯/리디렉트 플로우에서 paymentKey를 전달받은 뒤 confirm을 호출하는 모델.
        // SDK-only MVP에서는 상점 서버가 paymentKey를 전달해줄 수 있도록 Authorize를 Confirm에 매핑하는 오버로드가 필요하지만,
        // 간단히 orderId/amount 기반의 샘플 확정 호출을 표현합니다(실사용 시 paymentKey 필요).
        throw new NotSupportedException("Use ConfirmAsync with paymentKey in a dedicated method. AuthorizeAsync requires widget/redirect flow.");
    }

    public Task<PaymentResult> CaptureAsync(string orderId, string transactionId, CancellationToken ct = default)
        => Task.FromException<PaymentResult>(new NotSupportedException("Toss does not expose capture separately; use confirm at authorization phase."));

    public Task<PaymentResult> CancelAsync(string orderId, string? transactionId = null, CancellationToken ct = default)
        => Task.FromException<PaymentResult>(new NotSupportedException("Use RefundAsync with paymentKey and amount for refund/cancel."));

    public async Task<PaymentResult> RefundAsync(string orderId, string transactionId, decimal amount, CancellationToken ct = default)
    {
        var res = await _client.CancelAsync(transactionId, new TossCancelRequest("merchant requested", (long)amount), ct);
        return new PaymentResult(orderId, Name, res.status.ToUpperInvariant(), res.paymentKey, null);
    }
}
