# TemuDB MySQL Migration

## Übersicht

Das TemuDB-Projekt wurde von JSON-Dateien auf MySQL mit Entity Framework Core migriert.

## Neue Architektur

### Entity Framework Core mit MySQL

- **Provider**: Pomelo.EntityFrameworkCore.MySql
- **Version**: .NET 8.0
- **Datenbank**: MySQL 8.0+

### Neue Services

- `AuthServiceEF`: Benutzerverwaltung mit EF Core
- `TemuLinkServiceEF`: Temu-Link-Verwaltung mit EF Core
- `ApplicationDbContext`: EF Core DbContext für MySQL

## Installation & Setup

### 1. MySQL Server installieren

```bash
# Option 1: MySQL Installer
# Download von: https://dev.mysql.com/downloads/installer/

# Option 2: XAMPP
# Download von: https://www.apachefriends.org/
```

### 2. Datenbank erstellen

```sql
CREATE DATABASE temudb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. Tabellen erstellen

Führen Sie das SQL-Skript aus: `TemuDB.API/Database/create_database.sql`

Oder verwenden Sie die Migration:

```bash
cd TemuDB.API
dotnet ef database update
```

### 4. Connection String konfigurieren

In `TemuDB.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=temudb;User=root;Password=yourpassword;"
  }
}
```

### 5. API starten

```bash
cd TemuDB.API
dotnet run
```

## Datenbankstruktur

### Users Tabelle

```sql
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash LONGTEXT NOT NULL,
    DisplayName VARCHAR(100) NOT NULL,
    IsActive BOOLEAN NOT NULL DEFAULT FALSE,
    IsAdmin BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ActivatedAt DATETIME NULL
);
```

### TemuLinks Tabelle

```sql
CREATE TABLE TemuLinks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    Link VARCHAR(2000) NOT NULL,
    IsPublic BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

## Migration von JSON zu MySQL

### Automatische Migration

Die bestehenden JSON-Daten werden automatisch in MySQL migriert, wenn die API zum ersten Mal gestartet wird.

### Manuelle Migration

```bash
# Admin-Benutzer erstellen
curl -X POST "http://localhost:5290/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin123",
    "displayName": "Administrator"
  }'

# Benutzer als Admin aktivieren
mysql -u root -p temudb
UPDATE Users SET IsAdmin = 1, IsActive = 1 WHERE Username = 'admin';
```

## API-Endpunkte

### Authentifizierung

- `POST /api/auth/login` - Benutzer anmelden
- `POST /api/auth/register` - Benutzer registrieren
- `GET /api/auth/users/inactive` - Inaktive Benutzer (Admin)
- `GET /api/auth/users/all` - Alle Benutzer (Admin)
- `POST /api/auth/users/{id}/activate` - Benutzer aktivieren (Admin)

### Temu-Links

- `GET /api/temulink/user/{username}` - Links eines Benutzers
- `POST /api/temulink` - Neuen Link erstellen
- `PUT /api/temulink/{id}` - Link bearbeiten
- `DELETE /api/temulink/{id}` - Link löschen
- `GET /api/temulink/all` - Alle Links (Admin)
- `GET /api/temulink/public` - Öffentliche Links

## Vorteile der MySQL-Migration

### Performance

- ✅ Bessere Abfrage-Performance
- ✅ Indizierung für schnelle Suche
- ✅ Optimierte Joins

### Skalierbarkeit

- ✅ Unterstützung für große Datenmengen
- ✅ Transaktionale Integrität
- ✅ Backup & Recovery

### Funktionalität

- ✅ Vollständige EF Core-Features
- ✅ LINQ-Unterstützung
- ✅ Migrationen
- ✅ Code-First Development

## Troubleshooting

### Fehler: "Unable to connect to any of the specified MySQL hosts"

- MySQL Server läuft nicht
- Falsche Verbindungsdaten in appsettings.json
- Firewall blockiert Port 3306

### Fehler: "Access denied for user"

- Falsches Passwort
- Benutzer hat keine Rechte auf die Datenbank
- Datenbank existiert nicht

### Migration funktioniert nicht

- Verwende das SQL-Skript für manuelle Erstellung
- Stelle sicher, dass MySQL läuft
- Überprüfe die Connection String

## Entwicklung

### Neue Migration erstellen

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Datenbank zurücksetzen

```bash
dotnet ef database drop
dotnet ef database update
```

### Migrationen anzeigen

```bash
dotnet ef migrations list
```
