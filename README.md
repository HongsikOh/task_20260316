Employee_Hotline Project

## 기술 스택

| 분류 | 사용 기술 |
|---|---|
| 프레임워크 | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| DB | SQLite |

---

## 시작하기

### 요구사항

- .NET 10
- dotnet-ef 10
- SQLite

### .NET 10 설치

https://dotnet.microsoft.com/ko-kr/download/dotnet/10.0

### dotnet-ef 설치

```bash
dotnet tool install --global dotnet-ef --version 10.*
```

### SQLite 설치

https://www.sqlite.org/download.html 에서 환경에 맞는 파일 다운로드 및 환경변수 설정

- 아래 링크 참고
https://m.blog.naver.com/rickman2/222850909657#SE-4d694ae9-1984-4a34-bd75-c1a7fd41009c

### 실행

```bash
# 1. 의존성 복원
dotnet restore

# 2. 실행 (앱 시작 시 마이그레이션 자동 적용)
dotnet run --project Employee_Hotline.API
```

---

## API 엔드포인트

| Method | URL | 설명 |
|---|---|---|
| `GET` | `/api/employee` | 전체 목록 조회 (페이징 처리) |
| `GET` | `/api/employee/{name}` | 이름 조회 |
| `POST` | `/api/employee` | 직원 등록 (csv/json, file/body) |

---

## 유효성 검사 규칙

| 필드 | 규칙 |
|---|---|
| `Name` | 필수, 최대 100자, 중복 불가 |
| `Email` | 필수, 이메일 형식, 최대 200자 |
| `Tel` | 필수, 010으로 시작하는 11자리 숫자 (하이픈 자동 제거) |
| `JoinedAt` | 필수, `yyyy-MM-dd` / `yyyy.MM.dd` 형식 |
| 직원 등록 | 1회 최대 100건 |

---