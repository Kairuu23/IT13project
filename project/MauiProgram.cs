using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


using project.Data; 

namespace project
{
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

            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Server=KAIRULP\\SQLEXPRESS;Database=GymCRM_DB;User Id=sa;Password=kairu2356;TrustServerCertificate=True;"));

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
