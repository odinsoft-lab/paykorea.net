using PayKorea.Net;
using PayKorea.Net.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PayKorea SDK 등록
builder.Services.AddPayKorea(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 샘플: 결제 승인 (Mock)
app.MapPost("/payments/authorize", async (PayKoreaClient client, PaymentRequest req, CancellationToken ct) =>
{
    var result = await client.AuthorizeAsync(req, ct);
    return Results.Ok(result);
})
.WithName("AuthorizePayment")
.WithOpenApi();

// 샘플: 웹훅 엔드포인트 (서명 검증/멱등키는 실제 PG 스펙에 맞게 구현하세요)
app.MapPost("/webhooks/pg", async (HttpRequest http, ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("Webhook");
    // TODO: header 서명 검증, 멱등키 처리, 본문 파싱 및 비즈니스 처리
    using var reader = new StreamReader(http.Body);
    var body = await reader.ReadToEndAsync();
    logger.LogInformation("Webhook received: {length} bytes", body.Length);
    return Results.Ok(new { received = true });
})
.WithName("PgWebhook")
.WithOpenApi();

// 샘플: Apple Pay Merchant Validation Proxy (도메인 검증용)
app.MapPost("/apple-pay/merchant-validation", () =>
{
    // TODO: Apple Pay 세션 검증 요청을 애플 서버로 프록시. 인증서/키 설정 필요.
    return Results.StatusCode(StatusCodes.Status501NotImplemented);
})
.WithName("ApplePayMerchantValidation")
.WithOpenApi();

app.Run();
