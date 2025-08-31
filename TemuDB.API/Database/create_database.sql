-- TemuDB Datenbank erstellen
CREATE DATABASE IF NOT EXISTS temudb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE temudb;

-- Users Tabelle
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash LONGTEXT NOT NULL,
    DisplayName VARCHAR(100) NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT FALSE,
    IsAdmin BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ActivatedAt DATETIME NULL,
    INDEX IX_Users_Username (Username)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- TemuLinks Tabelle
CREATE TABLE IF NOT EXISTS TemuLinks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    Link VARCHAR(2000) NOT NULL,
    IsPublic BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    INDEX IX_TemuLinks_Username (Username)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Admin-Benutzer erstellen (optional)
-- Passwort: admin123 (BCrypt Hash)
INSERT INTO Users (Username, PasswordHash, DisplayName, IsActive, IsAdmin, CreatedAt, ActivatedAt) 
VALUES ('admin', '$2a$11$DBipnwKl75RljVUcSwxsTOZIXycK10hPYc232m0Qr3pjFPbGr7NVW', 'Administrator', TRUE, TRUE, NOW(), NOW())
ON DUPLICATE KEY UPDATE Username = Username;
