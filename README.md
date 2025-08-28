# PayKorea.Net

국내 PG 연동을 위한 .NET SDK, 리디렉트/팝업 기반 결제 흐름을 상정하며, 웹훅 처리와 도메인 검증(예: Apple Pay)은 상점 서버가 구현합니다.

## Monorepo 구조
- src/PayKorea.Net: 코어 SDK (DTO, IPaymentProvider, Mock, PayKoreaClient)
- src/PayKorea.Net.AspNetCore: ASP.NET Core DI 확장(AddPayKorea, PayKoreaOptions)
- src/PayKorea.Sample: 샘플 Minimal API (Authorize/Webhook/ApplePay 스텁)
- tests/PayKorea.Net.Tests: 코어 SDK 단위 테스트

## 요구 사항
- .NET SDK 8 / 9
- Windows/ macOS/ Linux 모두 지원 (샘플과 테스트 기준)

## 설치/빌드/테스트
PowerShell에서:

```pwsh
dotnet restore
dotnet build .\paykorea.net.sln -c Debug
dotnet test .\paykorea.net.sln -c Debug
```

## 샘플 실행 (PayKorea.Sample)
샘플 앱은 Mock Provider를 통해 승인 흐름을 시연합니다.

1) 앱 실행

```pwsh
dotnet run --project .\samples\PayKorea.Sample\PayKorea.Sample.csproj
```

2) Swagger UI 열기
- http://localhost:5000/swagger 또는 콘솔에 표시된 실제 URL

3) 엔드포인트
- POST `/payments/authorize`
	- Body 예시
		```json
		{
			"orderId": "ORD-001",
			"amount": 1000,
			"currency": "KRW",
			"description": "테스트 결제",
			"customerEmail": "buyer@example.com"
		}
		```
- POST `/webhooks/pg` (서명 검증/멱등키 처리 TODO)
- POST `/apple-pay/merchant-validation` (501 Not Implemented 템플릿)

## 앱 설정(AppSettings)
`PayKorea.Net.AspNetCore`는 `PayKorea` 섹션을 바인딩합니다.

```json
{
	"PayKorea": {
		"Provider": "Mock"
	}
}
```

## SDK 사용 방법
DI를 사용하는 ASP.NET Core 예:

```csharp
// Program.cs
using PayKorea.Net.AspNetCore;
builder.Services.AddPayKorea(builder.Configuration);

app.MapPost("/payments/authorize", async (PayKoreaClient client, PaymentRequest req, CancellationToken ct) =>
{
		var res = await client.AuthorizeAsync(req, ct);
		return Results.Ok(res);
});
```

## 계획
- 중장기 계획: [docs/ROADMAP.md](./docs/ROADMAP.md)
- 단기 작업: [docs/TASK.md](./docs/TASK.md)

## 라이선스
MIT. 자세한 내용은 `LICENSE.md` 참조.
