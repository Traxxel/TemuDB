# Environment Configuration Setup

## Übersicht
Die Anwendung verwendet jetzt `.env`-Dateien für die sichere Verwaltung von sensiblen Konfigurationsdaten wie Datenbankpasswörtern.

## Setup

### 1. .env-Datei erstellen
Kopiere die `.env.example`-Datei zu `.env`:
```bash
cp .env.example .env
```

### 2. .env-Datei konfigurieren
Bearbeite die `.env`-Datei mit deinen tatsächlichen Werten:
```env
# Database Configuration
DB_SERVER=localhost
DB_DATABASE=temudb
DB_USER=root
DB_PASSWORD=dein_sicheres_passwort_hier
```

### 3. Sicherheitshinweise
- **NIEMALS** die `.env`-Datei in Git committen
- Die `.env`-Datei ist bereits in der `.gitignore` ausgeschlossen
- Verwende starke Passwörter für die Produktion
- Teile die `.env`-Datei niemals öffentlich

## Dateien
- `.env` - Deine lokale Konfiguration (nicht in Git)
- `.env.example` - Vorlage für andere Entwickler (in Git)
- `appsettings.json` - Verwendet Umgebungsvariablen für die Connection String

## Funktionsweise
1. Die `Program.cs` lädt die `.env`-Datei beim Start
2. Die `appsettings.json` verwendet Platzhalter wie `${DB_PASSWORD}`
3. .NET ersetzt diese Platzhalter mit den Werten aus den Umgebungsvariablen

## Troubleshooting
Falls die Anwendung nicht startet:
1. Überprüfe, ob die `.env`-Datei existiert
2. Überprüfe, ob alle erforderlichen Variablen gesetzt sind
3. Überprüfe die MySQL-Verbindung mit den konfigurierten Werten
