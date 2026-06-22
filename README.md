# 대전시공사 홈페이지

ASP.NET Core MVC 8 + MySQL + Bootstrap 5 기반의 실서비스용 어닝·데크 전문 시공업체 홈페이지입니다.

---

## 개발 환경

| 항목 | 버전 |
|------|------|
| .NET | 8.0 |
| IDE | Visual Studio 2022 (권장) |
| DB | MySQL 8.0+ |
| ORM | Entity Framework Core 8 + Pomelo MySQL Provider |

---

## 폴더 구조

```
DaejeonConstruction/
├── DaejeonConstruction.sln
├── database/
│   └── init.sql                   ← 수동 DB 생성용 SQL
└── DaejeonConstruction.Web/
    ├── Areas/
    │   └── Admin/                 ← 관리자 영역 (/admin)
    │       ├── Controllers/
    │       └── Views/
    ├── Controllers/               ← 사용자 영역
    ├── Data/                      ← DbContext, DbInitializer
    ├── Migrations/                ← EF Core 마이그레이션
    ├── Models/
    │   ├── Enums/
    │   └── ViewModels/
    │       └── Admin/
    ├── Services/                  ← PasswordHasher, FileUploadService
    ├── Views/                     ← 사용자 뷰
    └── wwwroot/
        ├── css/
        │   ├── style.css          ← 원본 디자인 그대로
        │   └── admin.css
        ├── js/
        │   └── script.js
        └── uploads/
            ├── banners/           ← 배너 이미지 저장 폴더
            ├── works/             ← 시공사례 이미지 저장 폴더
            └── estimates/         ← 견적문의 첨부 이미지 저장 폴더
```

---

## 실행 방법

### 1) MySQL DB 준비
```sql
-- MySQL Workbench 또는 CLI 에서 실행
CREATE DATABASE daejeon_construction
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```
또는 `database/init.sql` 파일을 실행해도 됩니다.

### 2) 연결문자열 설정
`appsettings.json` 또는 `appsettings.Development.json` 의 `DefaultConnection` 수정:
```json
"Server=localhost;Port=3306;Database=daejeon_construction;Uid=root;Pwd=YOUR_PASSWORD;"
```

### 3) Visual Studio 2022 에서 실행
1. `DaejeonConstruction.sln` 더블클릭으로 솔루션 열기
2. NuGet 패키지 복원 (자동)
3. `F5` 또는 실행 버튼 클릭
4. 앱이 시작되면 **EF Core 마이그레이션이 자동 적용**되고
   **초기 관리자 계정(admin / admin1234)** 이 자동 생성됩니다.

### 4) CLI 에서 실행 (선택)
```bash
cd DaejeonConstruction.Web
dotnet run
```

---

## 관리자 페이지

| 항목 | 내용 |
|------|------|
| URL | `/admin` |
| 초기 아이디 | `admin` |
| 초기 비밀번호 | `admin1234` |

> ⚠️ **운영 서버 배포 전 반드시 비밀번호를 변경하세요!**
> 관리자 페이지에서 변경 기능을 추후 추가하거나, DB에서 직접 수정하세요.

### 관리자 기능 목록
- **대시보드** - 통계 요약, 최근 견적문의 5건
- **배너관리** - 메인 롤링배너 등록/수정/삭제, 이미지 교체
- **시공사례관리** - 어닝/데크 분류, 시공 전·후 이미지 다중업로드, 공개 여부
- **견적문의관리** - 전체/접수/상담중/완료 필터, 상태변경, 관리자 메모

---

## DB 테이블

| 테이블 | 용도 |
|--------|------|
| `ADMIN_USER` | 관리자 계정 (PBKDF2 해시 비밀번호) |
| `MAIN_BANNER` | 메인 롤링배너 |
| `WORK_CASE` | 시공사례 |
| `WORK_IMAGE` | 시공사례 이미지 (시공 전/후/갤러리) |
| `ESTIMATE_REQUEST` | 견적문의 |
| `ESTIMATE_FILE` | 견적문의 첨부파일 |

---

## 주요 URL 구조

| URL | 설명 |
|-----|------|
| `/` | 메인 (롤링배너, 회사소개, 시공품목, 시공사례, 견적문의) |
| `/works` | 시공사례 목록 |
| `/works/details/{id}` | 시공사례 상세 |
| `/estimate/create` | 견적문의 등록 (POST) |
| `/estimate/complete/{id}` | 견적문의 접수 완료 |
| `/admin` | 관리자 대시보드 |
| `/admin/account/login` | 관리자 로그인 |
| `/admin/banner` | 배너관리 |
| `/admin/workcase` | 시공사례관리 |
| `/admin/estimate` | 견적문의관리 |

---

## 실사진 교체 방법

1. 관리자 로그인 → **배너관리** → 기존 배너 수정 → 이미지 교체 업로드
2. 관리자 로그인 → **시공사례관리** → 시공사례 등록 → 실제 시공사진 업로드

> 업로드된 파일은 `wwwroot/uploads/` 하위 폴더에 날짜+UUID 형식으로 저장됩니다.

---

## 연락처

- 전화: 042-222-2222
- 카카오톡: https://open.kakao.com/o/sAa6exwi
