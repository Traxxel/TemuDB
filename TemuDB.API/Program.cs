using TemuDB.API.Data;
using TemuDB.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddSingleton<JsonDataContext>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TemuLinkService>();

var app = builder.Build();

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
