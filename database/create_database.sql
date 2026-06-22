-- PostgreSQL script
-- Chạy file này trong pgAdmin Query Tool.
-- Nếu database drug_warning_db chưa có, tạo trước bằng lệnh:
-- CREATE DATABASE drug_warning_db;

DROP TABLE IF EXISTS "MedicineWarnings" CASCADE;
DROP TABLE IF EXISTS "Medicines" CASCADE;
DROP TABLE IF EXISTS "Diseases" CASCADE;
DROP TABLE IF EXISTS "Admins" CASCADE;

CREATE TABLE "Admins" (
    "Id" SERIAL PRIMARY KEY,
    "Username" VARCHAR(100) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(256) NOT NULL,
    "FullName" VARCHAR(150) NULL
);

CREATE TABLE "Medicines" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(200) NOT NULL UNIQUE,
    "Description" VARCHAR(1000) NULL,
    "Usage" VARCHAR(1000) NULL,
    "SideEffects" VARCHAR(1000) NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Diseases" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(200) NOT NULL UNIQUE,
    "Description" VARCHAR(1000) NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "MedicineWarnings" (
    "Id" SERIAL PRIMARY KEY,
    "MedicineId" INT NOT NULL,
    "DiseaseId" INT NOT NULL,
    "RiskLevel" VARCHAR(30) NOT NULL,
    "WarningContent" VARCHAR(2000) NOT NULL,
    "Recommendation" VARCHAR(2000) NULL,

    CONSTRAINT "FK_MedicineWarnings_Medicines"
        FOREIGN KEY ("MedicineId") REFERENCES "Medicines"("Id") ON DELETE CASCADE,

    CONSTRAINT "FK_MedicineWarnings_Diseases"
        FOREIGN KEY ("DiseaseId") REFERENCES "Diseases"("Id") ON DELETE CASCADE,

    CONSTRAINT "UQ_Medicine_Disease" UNIQUE ("MedicineId", "DiseaseId")
);
