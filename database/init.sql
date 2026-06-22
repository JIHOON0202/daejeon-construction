-- =============================================================
-- 대전시공사 홈페이지 - 초기 DB 생성 SQL (MySQL 8.0+)
-- 사용법: MySQL Workbench 또는 CLI에서 실행
--   mysql -u root -p < init.sql
-- =============================================================

CREATE DATABASE IF NOT EXISTS daejeon_construction
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE daejeon_construction;

-- -------------------------------------------------------------
-- 1) 관리자 계정
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ADMIN_USER (
    Id          INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Username    VARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash VARCHAR(200) NOT NULL,
    DisplayName VARCHAR(100) NULL,
    IsActive    TINYINT(1)   NOT NULL DEFAULT 1,
    CreatedAt   DATETIME(6)  NOT NULL,
    LastLoginAt DATETIME(6)  NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -------------------------------------------------------------
-- 2) 메인 롤링배너
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS MAIN_BANNER (
    Id          INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Eyebrow     VARCHAR(100) NOT NULL,
    Title       VARCHAR(200) NOT NULL,
    SubText     VARCHAR(200) NULL,
    ButtonText  VARCHAR(50)  NOT NULL DEFAULT '견적문의 바로가기 →',
    ButtonLink  VARCHAR(300) NOT NULL DEFAULT '#quote',
    ImagePath   VARCHAR(300) NOT NULL,
    SortOrder   INT          NOT NULL DEFAULT 0,
    IsActive    TINYINT(1)   NOT NULL DEFAULT 1,
    CreatedAt   DATETIME(6)  NOT NULL,
    UpdatedAt   DATETIME(6)  NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -------------------------------------------------------------
-- 3) 시공사례
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS WORK_CASE (
    Id            INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Title         VARCHAR(150)  NOT NULL,
    Category      VARCHAR(20)   NOT NULL COMMENT 'Awning | Deck',
    Location      VARCHAR(100)  NULL,
    Description   VARCHAR(2000) NULL,
    ThumbnailPath VARCHAR(300)  NOT NULL,
    SortOrder     INT           NOT NULL DEFAULT 0,
    IsPublished   TINYINT(1)    NOT NULL DEFAULT 1,
    CreatedAt     DATETIME(6)   NOT NULL,
    UpdatedAt     DATETIME(6)   NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -------------------------------------------------------------
-- 4) 시공사례 이미지 (시공 전/후/갤러리)
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS WORK_IMAGE (
    Id          INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    WorkCaseId  INT          NOT NULL,
    ImagePath   VARCHAR(300) NOT NULL,
    ImageType   VARCHAR(20)  NOT NULL COMMENT 'Before | After | Gallery',
    SortOrder   INT          NOT NULL DEFAULT 0,
    CreatedAt   DATETIME(6)  NOT NULL,
    CONSTRAINT FK_WORK_IMAGE_CASE FOREIGN KEY (WorkCaseId)
        REFERENCES WORK_CASE (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -------------------------------------------------------------
-- 5) 견적문의
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ESTIMATE_REQUEST (
    Id          INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name        VARCHAR(50)   NOT NULL,
    Phone       VARCHAR(30)   NOT NULL,
    Area        VARCHAR(100)  NULL,
    ServiceType VARCHAR(50)   NULL,
    Message     VARCHAR(2000) NULL,
    Status      VARCHAR(20)   NOT NULL DEFAULT 'Received'
                              COMMENT 'Received | InProgress | Completed',
    AdminMemo   VARCHAR(2000) NULL,
    CreatedAt   DATETIME(6)   NOT NULL,
    UpdatedAt   DATETIME(6)   NOT NULL,
    IpAddress   VARCHAR(50)   NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- -------------------------------------------------------------
-- 6) 견적문의 첨부파일
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ESTIMATE_FILE (
    Id                  INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    EstimateRequestId   INT          NOT NULL,
    FilePath            VARCHAR(300) NOT NULL,
    FileName            VARCHAR(255) NOT NULL,
    FileSize            BIGINT       NOT NULL DEFAULT 0,
    UploadedAt          DATETIME(6)  NOT NULL,
    CONSTRAINT FK_ESTIMATE_FILE_REQUEST FOREIGN KEY (EstimateRequestId)
        REFERENCES ESTIMATE_REQUEST (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =============================================================
-- 초기 관리자 계정 (admin / admin1234)
-- PasswordHash 는 앱 기동 시 DbInitializer 가 덮어씁니다.
-- 아래 INSERT 는 참고용이며, 실제 해시는 앱에서 자동 생성됩니다.
-- =============================================================
-- INSERT INTO ADMIN_USER (Username, PasswordHash, DisplayName, IsActive, CreatedAt)
-- VALUES ('admin', 'PLACEHOLDER', '관리자', 1, NOW());

SELECT '초기 DB 생성 완료' AS result;
