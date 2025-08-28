namespace PayKorea.Net.Toss;

public sealed class TossPaymentsOptions
{
    // 테스트/라이브 시크릿 키 (test_sk_*, live_sk_*)
    public string SecretKey { get; set; } = string.Empty;
    // 기본 API 엔드포인트
    public string BaseUrl { get; set; } = "https://api.tosspayments.com";
    // 테스트 환경 에러 재현용 헤더 값(Optional)
    public string? TestCode { get; set; }
    // 샌드박스/위젯용 클라이언트 키 (test_ck_*, live_ck_*). 서버에서는 사용하지 않으나 데모 페이지용으로 지원
    public string? ClientKey { get; set; }
}
