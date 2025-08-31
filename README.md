# TemuDB - Temu-Linksammlung

Eine moderne Webanwendung zur Verwaltung von Temu-Links mit Benutzerauthentifizierung und Admin-Funktionen.

## 🚀 Features

- **Benutzerauthentifizierung**: Registrierung, Anmeldung und Abmeldung
- **Temu-Link Verwaltung**: Links hinzufügen, anzeigen, suchen und löschen
- **Admin-Bereich**: Benutzerfreischaltung und -verwaltung
- **Moderne UI**: Responsive Design mit Bootstrap 5
- **Sichere Datenspeicherung**: JSON-basierte lokale Datenspeicherung
- **Blazor Server**: Interaktive Webanwendung mit .NET 8

## 🛠️ Technologie-Stack

### Backend (TemuDB.API)

- **.NET 8** - Aktuellste .NET Version
- **ASP.NET Core Web API** - RESTful API
- **BCrypt.Net-Next** - Sichere Passwort-Hashing
- **JSON-basierte Datenspeicherung** - Einfache Wartung

### Frontend (TemuDB.Blazor)

- **Blazor Server** - Interaktive Webanwendung
- **Bootstrap 5** - Responsive UI-Framework
- **Bootstrap Icons** - Moderne Icons
- **JavaScript Interop** - Browser-Integration

## 📋 Voraussetzungen

- **.NET 8 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **Visual Studio 2022** oder **Visual Studio Code**
- **Git** für die Versionskontrolle

## 🚀 Installation und Setup

### 1. Repository klonen

```bash
git clone https://github.com/Traxxel/TemuDB.git
cd TemuDB
```

### 2. Beide Projekte bauen

```bash
dotnet build TemuDB.sln
```

### 3. API starten

```bash
cd TemuDB.API
dotnet run
```

Die API läuft dann auf: `http://localhost:5290`

### 4. Blazor-App starten

```bash
cd TemuDB.Blazor
dotnet run
```

Die Blazor-App läuft dann auf: `http://localhost:5083`

## 👤 Standard-Benutzer

Nach der ersten Ausführung wird automatisch ein Admin-Benutzer erstellt:

- **Benutzername:** `admin`
- **Passwort:** `admin`
- **Rolle:** Administrator

## 🔧 Konfiguration

### Ports ändern

Die Standard-Ports können in den `launchSettings.json` Dateien angepasst werden:

- **API:** `TemuDB.API/Properties/launchSettings.json`
- **Blazor:** `TemuDB.Blazor/Properties/launchSettings.json`

### CORS-Einstellungen

Die CORS-Policy ist in `TemuDB.API/Program.cs` konfiguriert und erlaubt Zugriff von:

- `https://localhost:7000`
- `http://localhost:5000`
- `http://localhost:5083`

## 📁 Projektstruktur

```
TemuDB/
├── TemuDB.API/                 # Backend API
│   ├── Controllers/           # API-Controller
│   ├── Data/                 # Datenzugriff
│   ├── Models/               # Datenmodelle
│   ├── Services/             # Geschäftslogik
│   └── Program.cs            # API-Startup
├── TemuDB.Blazor/            # Frontend
│   ├── Components/           # Blazor-Komponenten
│   │   ├── Pages/           # Seiten
│   │   ├── Layout/          # Layout-Komponenten
│   │   └── Services/        # Frontend-Services
│   └── Program.cs           # Blazor-Startup
└── TemuDB.sln               # Solution-Datei
```

## 🔐 API-Endpunkte

### Authentifizierung

- `POST /api/auth/login` - Benutzer anmelden
- `POST /api/auth/register` - Benutzer registrieren
- `GET /api/auth/users/inactive` - Inaktive Benutzer (Admin)
- `GET /api/auth/users/all` - Alle Benutzer (Admin)
- `POST /api/auth/users/{id}/activate` - Benutzer freischalten (Admin)

### Temu-Links

- `GET /api/temulink/user/{username}` - Links eines Benutzers
- `POST /api/temulink` - Neuen Link erstellen
- `DELETE /api/temulink/{id}` - Link löschen

## 🎨 Benutzeroberfläche

### Hauptseite

- Übersicht aller Temu-Links
- Suchfunktion
- Sortierung nach verschiedenen Kriterien
- Link-Verwaltung (Löschen)

### Admin-Bereich

- Benutzerfreischaltung
- Benutzerübersicht
- Admin-Statistiken

### Link-Erfassung

- Drag & Drop Unterstützung
- Copy-Paste Funktionalität
- Validierung der Eingaben

## 🔒 Sicherheit

- **Passwort-Hashing** mit BCrypt
- **CORS-Konfiguration** für sichere Cross-Origin-Requests
- **Input-Validierung** auf Client- und Server-Seite
- **Admin-Berechtigungen** für sensible Operationen

## 🚀 Deployment

### Lokale Entwicklung

1. Beide Projekte starten
2. API auf Port 5290
3. Blazor auf Port 5083

### Produktionsumgebung

- **API:** Als Windows Service oder Docker Container
- **Blazor:** Als IIS-Anwendung oder Docker Container
- **Daten:** JSON-Dateien in gesichertem Verzeichnis

## 🐛 Bekannte Probleme

### Blazor Server Interaktivität

- Alle Seiten benötigen `@rendermode InteractiveServer`
- JavaScript Interop nur in `OnAfterRenderAsync` möglich

### Port-Konflikte

- Falls Ports bereits belegt sind, in `launchSettings.json` ändern
- CORS-Policy entsprechend anpassen

## 🤝 Beitragen

1. Repository forken
2. Feature-Branch erstellen (`git checkout -b feature/AmazingFeature`)
3. Änderungen committen (`git commit -m 'Add some AmazingFeature'`)
4. Branch pushen (`git push origin feature/AmazingFeature`)
5. Pull Request erstellen

## 📝 Lizenz

Dieses Projekt ist unter der MIT-Lizenz lizenziert.

## 👨‍💻 Autor

**Stefan Meyer**

- GitHub: [@Traxxel](https://github.com/Traxxel)

## 🙏 Danksagungen

- **Bootstrap** für das UI-Framework
- **Microsoft** für Blazor und .NET
- **Bootstrap Icons** für die Icons

---

**TemuDB** - Eine moderne Lösung zur Verwaltung von Temu-Links 🛍️
