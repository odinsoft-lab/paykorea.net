# Tasks (Short-term)

단기 작업 목록입니다. 진행 시 체크하고, 완료되면 PR/커밋에 링크하세요.

## 0. 정리
- [x] PayKBridge.* 제거 및 PayKorea.* 정착
- [x] README를 SDK-only 기준으로 갱신
- [x] 샘플 엔드포인트(Authorize/Webhook/ApplePay 스텁) 추가

## 1. 테스트/품질
- [ ] PayKorea.Net 테스트 보강(Mock: Capture/Cancel/Refund 케이스)
- [ ] 샘플 API 최소 연기능 E2E 스모크 테스트 추가
- [ ] CI 파이프라인 기본 구성 (build + test)

## 2. 문서
- [ ] README에서 실행/설정 가이드 간결화 및 링크 정리
- [ ] Webhook/Apple Pay 구현 가이드 초안 작성

## 3. SDK/ASP.NET Core
- [ ] PayKoreaOptions 스키마 확장 (향후 Provider별 자격증명/엔드포인트)
- [ ] AddPayKorea 서비스 등록 고도화 (명시적 Provider 선택/확장 포인트)

## 4. 샘플 고도화
- [ ] appsettings.json 예시 추가 (PayKorea 섹션)
- [ ] Swagger 예시 스키마/설명 정리
- [ ] 예제 요청/응답 파일(HTTP 파일) 추가
