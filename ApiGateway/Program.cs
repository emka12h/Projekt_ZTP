var builder = WebApplication.CreateBuilder(args);

// 1. Dodajemy usługi YARP do kontenera DI i mówimy mu, 
// żeby szukał konfiguracji w pliku appsettings.json pod kluczem "ReverseProxy"
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// 2. Dodajemy middleware YARP do potoku aplikacji (to on przechwytuje zapytania)
app.MapReverseProxy();

app.Run();