using Microsoft.EntityFrameworkCore;
using TemuDB.API.Data;
using TemuDB.API.Models;
using MySqlConnector;

namespace TemuDB.API.Services
{
    public class DatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Starte Datenbank-Initialisierung...");

                // Prüfe ob Datenbank existiert und erstelle sie falls nötig
                await EnsureDatabaseExistsAsync();

                // Stelle sicher, dass alle Tabellen existieren
                await _context.Database.EnsureCreatedAsync();

                // Prüfe ob Admin-Benutzer existiert
                var adminExists = await _context.Users.AnyAsync(u => u.IsAdmin);

                if (!adminExists)
                {
                    _logger.LogInformation("Kein Admin-Benutzer gefunden. Erstelle Standard-Admin...");

                    var adminUser = new User
                    {
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                        DisplayName = "Administrator",
                        IsActive = true,
                        IsAdmin = true,
                        CreatedAt = DateTime.UtcNow,
                        ActivatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(adminUser);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("✅ Admin-Benutzer erfolgreich erstellt!");
                    _logger.LogInformation("   Username: admin");
                    _logger.LogInformation("   Passwort: admin");
                    _logger.LogInformation("   Status: Aktiv & Admin-Rechte");
                }
                else
                {
                    _logger.LogInformation("✅ Admin-Benutzer bereits vorhanden.");
                }

                // Zeige Anzahl der Benutzer an
                var userCount = await _context.Users.CountAsync();
                _logger.LogInformation($"Datenbank enthält {userCount} Benutzer.");

                // Zeige Anzahl der Temu-Links an
                var linkCount = await _context.TemuLinks.CountAsync();
                _logger.LogInformation($"Datenbank enthält {linkCount} Temu-Links.");

                _logger.LogInformation("Datenbank-Initialisierung abgeschlossen.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler bei der Datenbank-Initialisierung: {Message}", ex.Message);
                throw;
            }
        }

        private async Task EnsureDatabaseExistsAsync()
        {
            try
            {
                // Extrahiere Connection String ohne Datenbankname
                var connectionString = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection String ist nicht konfiguriert.");
                }
                var builder = new MySqlConnectionStringBuilder(connectionString);
                var databaseName = builder.Database;

                // Entferne Datenbankname für Server-Verbindung
                builder.Database = "";
                var serverConnectionString = builder.ToString();

                using var connection = new MySqlConnection(serverConnectionString);
                await connection.OpenAsync();

                // Prüfe ob Datenbank existiert
                using var command = new MySqlCommand(
                    $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'",
                    connection);

                var result = await command.ExecuteScalarAsync();

                if (result == null)
                {
                    _logger.LogInformation($"Datenbank '{databaseName}' existiert nicht. Erstelle sie...");

                    // Erstelle die Datenbank
                    using var createCommand = new MySqlCommand(
                        $"CREATE DATABASE `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci",
                        connection);
                    await createCommand.ExecuteNonQueryAsync();

                    _logger.LogInformation($"✅ Datenbank '{databaseName}' erfolgreich erstellt!");
                }
                else
                {
                    _logger.LogInformation($"✅ Datenbank '{databaseName}' bereits vorhanden.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Prüfen/Erstellen der Datenbank: {Message}", ex.Message);
                throw;
            }
        }
    }
}
