var builder = WebApplication.CreateBuilder(args);

//Zezwolenie na CORS (Cross-Origin Resource Sharing) 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// 1. Dodajemy usługi YARP do kontenera DI i mówimy mu, 
// żeby szukał konfiguracji w pliku appsettings.json pod kluczem "ReverseProxy"
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");
// 2. Dodajemy middleware YARP do potoku aplikacji (to on przechwytuje zapytania)
app.MapReverseProxy();

app.Run();