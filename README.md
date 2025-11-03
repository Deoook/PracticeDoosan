# Doosan Control UI (WPF)

![License](https://img.shields.io/badge/license-MIT-green)

## 소개
이 프로젝트는 **Doosan 로봇 제어용 WPF UI**를 구현한 예제입니다.  
주요 기능은 다음과 같습니다:

- Servo On/Off 제어
- Stop 버튼, Jog 버튼 등 로봇 동작 제어
- 버튼 클릭 시 Ripple 이펙트 적용
- 버튼 Hover 및 Pressed 상태 애니메이션
- MVVM 패턴 기반 Command 바인딩

## 기능

| UI 요소          | 기능 설명 |
|-----------------|-----------|
| Servo 버튼       | On/Off 상태 표시, 클릭 시 Servo 제어 |
| Stop 버튼        | 로봇 동작 정지 |
| Jog 버튼         | 로봇 관절 J1~J6 이동 제어 |
| Speed Slider     | 이동 속도 조절 |
| Ripple           | 클릭 시 중앙에서 퍼지는 Ripple 애니메이션 |

## 스크린샷

![UI 스크린샷](./screenshots/ui_preview.png)

## 설치

1. GitHub 저장소 복제:
   ```bash
   git clone https://github.com/Deoook/PracticeDoosan.git
