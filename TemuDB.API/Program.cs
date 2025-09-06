using Microsoft.EntityFrameworkCore;
using TemuDB.API.Data;
using TemuDB.API.Services;
using DotNetEnv;

// .env-Datei laden
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework mit MySQL konfigurieren
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// CORS konfigurieren
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:7000", "http://localhost:5000", "http://localhost:5082", "http://localhost:5083")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Services registrieren
builder.Services.AddScoped<AuthServiceEF>();
builder.Services.AddScoped<TemuLinkServiceEF>();
builder.Services.AddScoped<DatabaseInitializer>();

var app = builder.Build();

// Datenbank initialisieren
try
{
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await initializer.InitializeAsync();
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Fehler bei der Datenbank-Initialisierung: {Message}", ex.Message);
    logger.LogWarning("Die Anwendung startet ohne Datenbankverbindung. Stellen Sie sicher, dass MySQL läuft.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
