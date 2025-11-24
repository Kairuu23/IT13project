using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using project.Data;
using project.Pages;
using project.Services;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace project
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddMauiBlazorWebView();

#if WINDOWS
            Microsoft.Maui.Handlers.ViewHandler.ViewMapper.AppendToMapping("CameraPermission", (handler, view) =>
            {
                if (handler.PlatformView is Microsoft.UI.Xaml.Controls.WebView2 webView2)
                {
                    webView2.CoreWebView2Initialized += (sender, args) =>
                    {
                        try
                        {
                            if (webView2.CoreWebView2 != null)
                            {
                                webView2.CoreWebView2.Settings.IsWebMessageEnabled = true;
                                webView2.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
                                webView2.CoreWebView2.Settings.IsStatusBarEnabled = false;


                                webView2.CoreWebView2.PermissionRequested += (s, e) =>
                                {
                                    if (e.PermissionKind ==
                                        Microsoft.Web.WebView2.Core.CoreWebView2PermissionKind.Camera ||
                                        e.PermissionKind ==
                                        Microsoft.Web.WebView2.Core.CoreWebView2PermissionKind.Microphone)
                                    {
                                        e.State =
                                            Microsoft.Web.WebView2.Core.CoreWebView2PermissionState.Allow;
                                    }
                                };
                            }
                        }
                        catch { }
                    };

                    try { var _ = webView2.CoreWebView2; } catch { }
                }
            });
#endif

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // =============================
            //         SERVICE DI
            // =============================
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<IMemberService, MemberService>();

            builder.Services.AddSingleton<INativeNavigationService, NativeNavigationService>();
            builder.Services.AddTransient<AttendanceScannerPage>();
            

            // =============================
            //     DATABASE CONNECTION
            // =============================
            const string localConnectionString =
                "Data Source=KAIRULP\\SQLEXPRESS;Initial Catalog=GymCRM_DB;User ID=sa;Password=kairu2356;Encrypt=True;Trust Server Certificate=True";

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(localConnectionString));

            builder.Services.AddScoped<DatabaseInitializer>();

            var app = builder.Build();

            // =============================
            //   DATABASE INITIALIZATION
            // =============================
            try
            {
                using var scope = app.Services.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                initializer.InitializeAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Database initialization failed: {ex.Message}");
                Console.WriteLine($"❌ Database initialization failed: {ex.Message}");
            }

            return app;
        }
    }
}
