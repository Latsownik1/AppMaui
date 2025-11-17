using Microsoft.Extensions.Logging;
using Produkty;
using Produkty.Data;

namespace Produkty;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // 🔹 Ścieżka do lokalnej bazy danych (plik będzie w AppDataDirectory)
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "produkty.db3");

        // 🔹 Rejestrujemy bazę w DI (Dependency Injection)
        builder.Services.AddSingleton(new ProductDatabase(dbPath));

        return builder.Build();
    }
}