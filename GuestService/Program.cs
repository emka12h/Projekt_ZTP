using GuestService.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. KONFIGURACJA KESTRELA ---
// Wymuszamy, aby aplikacja wewnątrz kontenera słuchała na porcie HTTP.
// Dzięki temu Docker Compose (mapowanie 5050:5050) będzie działać bezbłędnie.
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5050); 
});

// --- 2. KONFIGURACJA BAZY DANYCH ---
builder.Services.AddDbContext<GuestDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            // Retry Policy: Baza danych w Dockerze wstaje wolniej niż kod.
            // Ten zapis sprawia, że aplikacja nie "wywali się", tylko poczeka na bazę.
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
        }));

// --- 3. KONTROLERY I JSON ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ważne: Zmienia Enumy na tekst (np. "Confirmed" zamiast 1) w odpowiedziach API.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 4. AUTOMATYCZNE MIGRACJE ---
// Ten blok sprawdza przy każdym starcie, czy baza danych ma aktualne tabele.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<GuestDbContext>();
        // Migrate() będzie teraz naszym jedynym narzędziem
        dbContext.Database.Migrate();
        Console.WriteLine(">>> Migracje GuestService zastosowane pomyślnie! <<<");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Błąd podczas stosowania migracji.");
    }
}

// --- 5. MIDDLEWARE ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GuestService API v1");
    c.RoutePrefix = "swagger";
});

// W Dockerze zazwyczaj nie używamy HTTPS Redirection dla wewnętrznych mikroserwisów.
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();