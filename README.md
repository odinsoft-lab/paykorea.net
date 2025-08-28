# PayKorea.Net

국내 PG 연동을 위한 .NET SDK, 리디렉트/팝업 기반 결제 흐름을 상정하며, 웹훅 처리와 도메인 검증(예: Apple Pay)은 상점 서버가 구현합니다.

## Monorepo 구조
- src/PayKorea.Net: 코어 SDK (DTO, IPaymentProvider, Mock, PayKoreaClient)
- src/PayKorea.Net.AspNetCore: ASP.NET Core DI 확장(AddPayKorea, PayKoreaOptions, AddTossPayments)
- samples/PayKorea.Sample.Toss: Toss 전용 샘플 Minimal API (Confirm/Cancel, Redirect success/fail)
- tests/PayKorea.Tests.Toss: Toss 전용 단위 테스트

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

## 샘플 실행 (Toss 전용)
Toss Confirm/Cancel 및 Redirect 핸들러를 확인할 수 있습니다.

1) 설정: `samples/PayKorea.Sample.Toss/appsettings.json`
```json
{
	"TossPayments": {
	"SecretKey": "test_sk_replace_me",
	"ClientKey": "test_ck_replace_me",
	"BaseUrl": "https://api.tosspayments.com"
	}
}
```

2) 실행
```pwsh
dotnet run --project .\samples\PayKorea.Sample.Toss\PayKorea.Sample.Toss.csproj
```

3) Swagger UI
- https://localhost:5001/swagger (개발 환경 기본)

4) HTTP 예제 파일
- `samples/PayKorea.Sample.Toss/Toss.sample.http`

5) 샌드박스 데모 페이지(위젯)
- https://localhost:5001/ 로 접속 후 “결제하기” 클릭 → Toss 결제창 → 성공시 서버에서 /toss/success에서 Confirm 수행

## 앱 설정(AppSettings)
`PayKorea.Net.AspNetCore`는 `PayKorea` 섹션(코어)과 `TossPayments` 섹션(토스)을 바인딩합니다.

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
