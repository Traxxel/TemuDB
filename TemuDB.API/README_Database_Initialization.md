# Datenbank-Initialisierung für TemuDB

## Übersicht

Die TemuDB API verfügt jetzt über eine automatische Datenbank-Initialisierung, die beim Start folgende Aufgaben erfüllt:

1. **Prüft, ob MySQL läuft**
2. **Erstellt die Datenbank 'temudb' falls sie nicht existiert**
3. **Erstellt alle Tabellen falls sie nicht existieren**
4. **Erstellt einen Admin-Benutzer falls keiner vorhanden ist**

## Automatische Initialisierung

### Was passiert beim Start:

```
info: TemuDB.API.Services.DatabaseInitializer[0]
      Starte Datenbank-Initialisierung...
info: TemuDB.API.Services.DatabaseInitializer[0]
      Datenbank 'temudb' existiert nicht. Erstelle sie...
info: TemuDB.API.Services.DatabaseInitializer[0]
      ✅ Datenbank 'temudb' erfolgreich erstellt!
info: TemuDB.API.Services.DatabaseInitializer[0]
      Kein Admin-Benutzer gefunden. Erstelle Standard-Admin...
info: TemuDB.API.Services.DatabaseInitializer[0]
      ✅ Admin-Benutzer erfolgreich erstellt!
info: TemuDB.API.Services.DatabaseInitializer[0]
         Username: admin
info: TemuDB.API.Services.DatabaseInitializer[0]
         Passwort: admin
info: TemuDB.API.Services.DatabaseInitializer[0]
         Status: Aktiv & Admin-Rechte
info: TemuDB.API.Services.DatabaseInitializer[0]
      Datenbank enthält 1 Benutzer.
info: TemuDB.API.Services.DatabaseInitializer[0]
      Datenbank enthält 0 Temu-Links.
info: TemuDB.API.Services.DatabaseInitializer[0]
      Datenbank-Initialisierung abgeschlossen.
```

## Voraussetzungen

### 1. MySQL Server installieren

**Option A: XAMPP (Empfohlen für Entwicklung)**

```bash
# Download von: https://www.apachefriends.org/
# Installieren und XAMPP Control Panel starten
# MySQL starten (Button "Start" klicken)
```

**Option B: MySQL Installer**

```bash
# Download von: https://dev.mysql.com/downloads/installer/
# Installieren und MySQL als Service konfigurieren
```

### 2. Connection String konfigurieren

In `TemuDB.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=temudb;User=root;Password=DEIN_PASSWORT;"
  }
}
```

**Häufige Passwörter:**

- **XAMPP**: `Password=;` (leer)
- **MySQL Installer**: Das Passwort, das Sie bei der Installation gesetzt haben

## Erste Ausführung

### 1. MySQL starten

Stellen Sie sicher, dass MySQL läuft.

### 2. API starten

```bash
cd TemuDB.API
dotnet run
```

### 3. Automatische Erstellung

Die API erstellt automatisch:

- ✅ Datenbank `temudb` (falls nicht vorhanden)
- ✅ Tabellen `Users` und `TemuLinks` (falls nicht vorhanden)
- ✅ Admin-Benutzer (falls nicht vorhanden)

## Admin-Zugang

Nach der ersten Ausführung können Sie sich mit folgenden Daten anmelden:

- **Username**: `admin`
- **Passwort**: `admin`
- **Status**: Aktiv mit Admin-Rechten

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

## Fehlerbehandlung

### Fehler: "Unable to connect to any of the specified MySQL hosts"

**Lösungen:**

1. MySQL Server läuft nicht → Starten Sie MySQL
2. Falsche Server-Adresse → Überprüfen Sie den Connection String
3. Firewall blockiert Port 3306 → Firewall-Regel hinzufügen

### Fehler: "Access denied for user"

**Lösungen:**

1. Falsches Passwort → Passwort in appsettings.json korrigieren
2. Benutzer hat keine Rechte → Benutzerrechte prüfen

### API startet ohne Datenbankverbindung

Die API zeigt eine Warnung an, wenn MySQL nicht erreichbar ist:

```
warn: Program[0]
      Die Anwendung startet ohne Datenbankverbindung. Stellen Sie sicher, dass MySQL läuft.
```

## Test der Initialisierung

Nach erfolgreicher Initialisierung können Sie die API testen:

```bash
# Admin-Login testen
curl -X POST "http://localhost:5290/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin"
  }'
```

**Erwartete Antwort:**

```json
{
  "success": true,
  "user": {
    "id": 1,
    "username": "admin",
    "displayName": "Administrator",
    "isAdmin": true
  }
}
```

## Vorteile der automatischen Initialisierung

- ✅ **Plug & Play**: Keine manuelle Datenbankeinrichtung erforderlich
- ✅ **Sicherheit**: Standard-Admin wird automatisch erstellt
- ✅ **Robustheit**: Funktioniert auch bei fehlender Datenbank
- ✅ **Transparenz**: Detaillierte Logs zeigen den Initialisierungsprozess
- ✅ **Wiederverwendbarkeit**: Funktioniert bei jeder neuen Installation
