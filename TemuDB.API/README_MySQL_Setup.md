# MySQL Setup für TemuDB

## Voraussetzungen

1. **MySQL Server installieren**

   - Download von: https://dev.mysql.com/downloads/mysql/
   - Oder XAMPP: https://www.apachefriends.org/

2. **MySQL Workbench (optional)**
   - Für die Verwaltung der Datenbank

## Einrichtung

### 1. MySQL Server starten

```bash
# Windows (XAMPP)
# Starte XAMPP Control Panel und aktiviere MySQL

# Windows (MySQL Installer)
# MySQL wird als Service automatisch gestartet
```

### 2. Datenbank erstellen

```sql
CREATE DATABASE temudb;
```

### 3. Benutzer erstellen (optional)

```sql
CREATE USER 'temudb_user'@'localhost' IDENTIFIED BY 'yourpassword';
GRANT ALL PRIVILEGES ON temudb.* TO 'temudb_user'@'localhost';
FLUSH PRIVILEGES;
```

### 4. Connection String anpassen

In `appsettings.json` die Verbindungsdaten anpassen:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=temudb;User=root;Password=yourpassword;"
  }
}
```

### 5. Migrationen ausführen

```bash
# Migration erstellen
dotnet ef migrations add InitialCreate

# Datenbank aktualisieren
dotnet ef database update
```

### 6. Admin-Benutzer erstellen

Nach der ersten Ausführung können Sie einen Admin-Benutzer über die API erstellen:

```bash
curl -X POST "http://localhost:5290/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin123",
    "displayName": "Administrator"
  }'
```

Dann den Benutzer in der Datenbank als Admin und aktiv markieren:

```sql
UPDATE Users SET IsAdmin = 1, IsActive = 1 WHERE Username = 'admin';
```

## Troubleshooting

### Fehler: "Unable to connect to any of the specified MySQL hosts"

- MySQL Server läuft nicht
- Falsche Verbindungsdaten
- Firewall blockiert Port 3306

### Fehler: "Access denied for user"

- Falsches Passwort
- Benutzer hat keine Rechte auf die Datenbank

### Migration funktioniert nicht

- Verwende die DesignTimeDbContextFactory
- Stelle sicher, dass MySQL läuft
- Überprüfe die Connection String
