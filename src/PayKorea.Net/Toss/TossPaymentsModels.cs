using System.Text.Json.Serialization;

namespace PayKorea.Net.Toss;

public sealed record TossConfirmRequest(
    string paymentKey,
    string orderId,
    long amount
);

public sealed record TossCancelRequest(
    string cancelReason,
    long? cancelAmount = null
);

public sealed record TossPaymentResponse(
    string paymentKey,
    string orderId,
    string status,
    string method
);

public sealed record TossErrorResponse(
    string code,
    string message
);
