using GuestService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfiguracja Bazy Danych z Retry Policy (odporność na start Dockera)
builder.Services.AddDbContext<GuestDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
        }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Automatyczne migracje przy starcie
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GuestDbContext>();
    dbContext.Database.Migrate();
}

// 3. Konfiguracja Swaggera - WYMUSZONA (bez IFa)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Te dwie linijki upewniają się, że Swagger znajdzie plik definicji w Dockerze
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GuestService API v1");
    c.RoutePrefix = "swagger";
});

// Ważne: Wyłączamy UseHttpsRedirection na czas testów w Dockerze, 
// bo kontenery często nie mają certyfikatów SSL, co powoduje błędy 404/Connection Refused.
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();