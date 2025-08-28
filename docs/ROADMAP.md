# Roadmap

장기/중기 계획을 추적합니다. (Option B: SDK-only 기준)

## 1. Core SDK
- [ ] 공통 오류/예외 체계 정립 (에러 코드, 재시도 가능성, 사용자 메시지)
- [ ] 서명/보안 유틸리티 (HMAC, 타임스탬프 검증)
- [ ] HttpClient 재시도/회로차단(Polly) 가이드 및 확장 포인트
- [ ] 강건성/성능: 대량 트래픽/타임아웃/취소 토큰 처리

## 2. Provider 어댑터
- [ ] Toss Payments Redirect/Widget 연동
- [ ] Inicis/KSNET 등 주요 PG 연동
- [ ] 샌드박스/라이브 환경 스위치와 구성 스키마
- [ ] Webhook 페이로드 모델 및 검증

## 3. ASP.NET Core 지원
- [ ] AddPayKorea 확장 고도화 (Options 밸리데이션, Health Checks)
- [ ] Webhook 미들웨어/필터 템플릿 제공 (서명 검증, 멱등 처리)
- [ ] Apple Pay Merchant Validation 프록시 유틸/문서화

## 4. 개발 경험(DX)
- [ ] 템플릿(dotnet new) 제공: SDK-only, Sample API, Webhook 전용
- [ ] 샘플 확장: 쿠폰/부분취소/부분환불 플로우
- [ ] 테스트 확장: 통합 테스트, 컨트랙트 테스트

## 5. 문서화
- [ ] 통합 시작 가이드(Quickstart)
- [ ] 마이그레이션 가이드(PayKBridge → PayKorea)
- [ ] 에러 처리/보안 가이드
- [ ] FAQ/트러블슈팅

## 6. 배포
- [ ] NuGet 패키지 배포(PayKorea.Net, PayKorea.Net.AspNetCore)
- [ ] 버전 정책(SemVer)/릴리즈 노트 자동화
- [ ] 샘플 앱 배포 옵션(컨테이너)
