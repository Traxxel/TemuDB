# MySQL Setup Guide für TemuDB

## Problem

Die API kann nicht starten, weil MySQL nicht läuft oder nicht konfiguriert ist.

## Lösung

### 1. MySQL Server installieren

**Option A: XAMPP (Empfohlen für Entwicklung)**

1. Download von: https://www.apachefriends.org/
2. Installieren und XAMPP Control Panel starten
3. MySQL starten (Button "Start" klicken)

**Option B: MySQL Installer**

1. Download von: https://dev.mysql.com/downloads/installer/
2. Installieren und MySQL als Service konfigurieren

### 2. Datenbank erstellen

**Mit MySQL Workbench oder phpMyAdmin:**

```sql
CREATE DATABASE temudb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

**Oder mit Kommandozeile:**

```bash
mysql -u root -p
CREATE DATABASE temudb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. Connection String anpassen

In `TemuDB.API/appsettings.json` das Passwort anpassen:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=temudb;User=root;Password=DEIN_PASSWORT;"
  }
}
```

**Häufige Passwörter:**

- XAMPP: leer (kein Passwort)
- MySQL Installer: das Passwort, das Sie bei der Installation gesetzt haben

### 4. API starten

```bash
cd TemuDB.API
dotnet run
```

### 5. Erwartete Ausgabe

Wenn alles funktioniert, sollten Sie folgende Logs sehen:

```
info: TemuDB.API.Services.DatabaseInitializer[0]
      Starte Datenbank-Initialisierung...
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
```

## Troubleshooting

### Fehler: "Unable to connect to any of the specified MySQL hosts"

**Lösungen:**

1. MySQL Server läuft nicht → Starten Sie MySQL
2. Falsches Passwort → Überprüfen Sie den Connection String
3. Firewall blockiert Port 3306 → Firewall-Regel hinzufügen

### Fehler: "Access denied for user"

**Lösungen:**

1. Falsches Passwort → Passwort in appsettings.json korrigieren
2. Benutzer hat keine Rechte → Benutzerrechte prüfen

### XAMPP MySQL startet nicht

**Lösungen:**

1. Port 3306 ist belegt → Anderen MySQL-Service beenden
2. XAMPP als Administrator ausführen
3. XAMPP neu installieren

## Test der Verbindung

Nach dem Start können Sie die API testen:

```bash
# Admin-Login testen
curl -X POST "http://localhost:5290/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin"
  }'
```

## Admin-Zugang

Nach erfolgreicher Initialisierung können Sie sich mit folgenden Daten anmelden:

- **Username**: admin
- **Passwort**: admin
- **Status**: Aktiv mit Admin-Rechten
