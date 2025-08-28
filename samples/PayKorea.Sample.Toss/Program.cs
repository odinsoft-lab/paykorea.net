using PayKorea.Net.AspNetCore;
using PayKorea.Net.Toss;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTossPayments(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Sandbox demo page: simple button to open Toss payment window
app.MapGet("/", (IConfiguration config) =>
{
        var clientKey = config.GetSection("TossPayments")["ClientKey"] ?? "test_ck_replace_me";
        var html = $$"""
        <!doctype html>
        <html lang="ko">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1" />
            <title>Toss Sandbox Demo</title>
            <style>
                body { font-family: system-ui, -apple-system, Segoe UI, Roboto, Noto Sans KR, Helvetica, Arial, sans-serif; margin: 2rem; }
                label { display:block; margin-top: 1rem; }
                input { padding: .5rem; width: 320px; }
                button { margin-top: 1.25rem; padding: .75rem 1rem; font-size: 1rem; }
                .hint { color: #666; font-size: .9rem; }
                .key { font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace; }
            </style>
            <script src="https://js.tosspayments.com/v1"></script>
        </head>
        <body>
            <h1>Toss Sandbox 결제 테스트</h1>
            <p class="hint">이 페이지는 테스트용입니다. 테스트 ClientKey/SecretKey로 실행하세요.</p>
            <p>ClientKey: <span class="key">{{clientKey}}</span></p>
            <p class="hint">현재 Origin: <span id="origin"></span> (가능하면 https 사용)</p>

            <label>주문명
                <input id="orderName" value="샌드박스 테스트 결제" />
            </label>
            <label>결제금액(원)
                <input id="amount" type="number" value="1000" min="0" />
            </label>
            <label>이메일
                <input id="email" type="email" value="buyer@example.com" />
            </label>
            <label>구매자명
                <input id="customerName" value="홍길동" />
            </label>

            <button id="pay">결제하기</button>

            <script>
                const clientKey = "{{clientKey}}";
                const tossPayments = TossPayments(clientKey);
                document.getElementById('origin').textContent = window.location.origin;
                document.getElementById('pay').addEventListener('click', async () => {
                    const amount = Number(document.getElementById('amount').value || 0);
                    const orderName = document.getElementById('orderName').value || '샌드박스 테스트 결제';
                    const customerEmail = document.getElementById('email').value || 'buyer@example.com';
                    const customerName = document.getElementById('customerName').value || '홍길동';
                    const orderId = 'ORD-' + Date.now();
                    const origin = window.location.origin;
                    const successUrl = origin + '/toss/success';
                    const failUrl = origin + '/toss/fail';

                    try {
                        await tossPayments.requestPayment('카드', {
                            amount,
                            orderId,
                            orderName,
                            successUrl,
                            failUrl,
                            customerEmail,
                            customerName
                        });
                    } catch (err) {
                        console.error('결제창 열기 오류', err);
                        const code = err?.code || '';
                        const msg = err?.message || String(err);
                        alert(`결제창 열기 오류\ncode: ${code}\nmessage: ${msg}\n\n1) https 로 접속 중인지 확인\n2) ClientKey 가 테스트 키인지 확인(test_ck_*)\n3) 브라우저 콘솔 에러 참고`);
                    }
                });
            </script>
        </body>
        </html>
        """;
        return Results.Content(html, "text/html; charset=utf-8");
});

// Confirm API (server-side confirm)
app.MapPost("/toss/confirm", async (TossPaymentsClient toss, string paymentKey, string orderId, long amount, CancellationToken ct) =>
{
    var res = await toss.ConfirmAsync(new TossConfirmRequest(paymentKey, orderId, amount), ct);
    return Results.Ok(res);
})
.WithName("TossConfirm").WithOpenApi();

// Cancel API (refund/partial cancel)
app.MapPost("/toss/cancel", async (TossPaymentsClient toss, string paymentKey, string cancelReason, long? cancelAmount, CancellationToken ct) =>
{
    var res = await toss.CancelAsync(paymentKey, new TossCancelRequest(cancelReason, cancelAmount), ct);
    return Results.Ok(res);
})
.WithName("TossCancel").WithOpenApi();

// Redirect success/fail handlers for widget/redirect flows (optional)
app.MapGet("/toss/success", async (HttpRequest req, TossPaymentsClient toss, CancellationToken ct) =>
{
    var paymentKey = req.Query["paymentKey"].ToString();
    var orderId = req.Query["orderId"].ToString();
    var amountStr = req.Query["amount"].ToString();
    if (string.IsNullOrWhiteSpace(paymentKey) || string.IsNullOrWhiteSpace(orderId) || !long.TryParse(amountStr, out var amount))
    {
        return Results.BadRequest(new { error = "Missing or invalid query (paymentKey, orderId, amount)" });
    }
    var res = await toss.ConfirmAsync(new TossConfirmRequest(paymentKey, orderId, amount), ct);
    return Results.Ok(res);
})
.WithName("TossSuccess").WithOpenApi();

app.MapGet("/toss/fail", (HttpRequest req) =>
{
    var code = req.Query["code"].ToString();
    var message = req.Query["message"].ToString();
    var orderId = req.Query["orderId"].ToString();
    return Results.BadRequest(new { orderId, code, message });
})
.WithName("TossFail").WithOpenApi();

app.Run();
