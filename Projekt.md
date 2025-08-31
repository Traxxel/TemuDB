Erstelle eine Anwendung mit zwei Komponenten.
Komponente 1 soll eine .net 8 C# WebAPI sein, welche Daten in einer lokalen json-Datei speichert anstelle einer Datenbank.
Komponente 2 soll eine .net 8 Blazor-Anwendung sein als Frontend. Nimm die einfachste Blazor-Option, die möglich ist

# Allgemeines

Die Webseite soll "Temu-Linksammlung" heissen.
Benutze in ersten Schritt nur Microsoft-Blazor-Komponenten für das Frontend (Komponente 2). Später sollen diese ggf. durch DevExpress- oder andere moderne Framework-Komponenten ausgetauscht werden können.
Erstelle jede Komponente in einem Unterverzeichnis.
Erstelle eine .sln-Datei im Hauptverzeichnis mit dem Namen TemuDB

# Komponente 1

Zugriffe auf die API sind nur nach einer Anmeldung möglich. Stelle sicher, dass nur in Komponente 2 angemeldete Benutzer die API benutzen können. Ein anonymer Zugriff muss unterbunden sein.
Später soll die json-Datei durch eine Datenbank ersetzt werden. Beachte dieses beim json-Format.

# Komponente 2

Das Speichern von Daten unter ##Funktion soll über die WebAPI "Komponente 1" erfolgen.

## Benutzerverwaltung

Jeder Benutzer muss sich mit Benutzernamen und Passwort anmelden.
Zuerst gibt es nur einen Benutzer mit dem Benutzernamen adminstefanmeyer und dem Passwort JJvmr111
Dieser Benutzer soll ein Administrator sein.

Weitere Benutzer sollen sich registrieren können mit ihrem Benutzernamen und einem Passwort. Auch müssen sie einen Namen angeben, an dem sie der Admin erkennen kann (z.B. Vorname oder Spitzname). Der Benutzer soll beim registrieren darauf hingewiesen werden.
Der Administrator soll eine Verwaltungsseite erhalten, wo er die Benutzer verwalten kann:
Neue Benutzer müssen erst freigeschaltet bzw. aktiviert werden, bevor sie sich anmelden können.
Es soll eine Möglichkeit zum Abmelden geben.

## Funktion

### Übersichtsseite

Benutzer sollen in der App Links der Shoppingseite Temu verwalten können, da dort keine Wunschlisten geführt werden können.
Auf der Hauptseite eines Benutzers nach der Anmeldung sollen in einer Liste alle seine gesammelten Einträge in einer schönen durchsuchbaren und sortierbaren Liste angezeigt werden.
Es soll ein Button "Neu erfassen" vorhanden sein.

### Neu erfassen

Durch Druck auf den Button "Neu erfassen" soll eine neue Seite geöffnet werden. Diese beinhaltet ein großes Textfeld, in dem ein Link per ctrl-v eingefügt werden kann.
Alternativ soll dort ein Link auf einer Browser-Adresszeile per drag and drop abgelegt werden können.
Zusätzlich soll in einem einzeiligen Textfeld eine Kurzbeschreibung des Artikels durch den Anwender erfasst werden können (Pflichtfeld).
Ein Button "Speichern" soll den Artikel per WebAPI speichern. Diese Felder sollen gespeichert werden:

- Benutzername
- Artikel-Kurzbeschreibung
- Link
- Timestamp der Speicherung
