using CORPORATE_DISBURSEMENT_ADMIN.Extensions;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using DATA;
using DATA.Models;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        bool url_browsing_enabled = builder.Configuration.GetValue<bool>("ApplicationConfiguration:URLBrowsingEnabled");
        if (url_browsing_enabled)
        {
            builder.WebHost.UseUrls(builder.Configuration.GetValue<string>("ApplicationConfiguration:ApplicationURL") ?? string.Empty);
        }
        
        

        builder.Services.AddMvc();
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;

        });
        string? logpath = builder.Configuration["FilePaths:LogFileRootDirectory"];
        builder.Host.UseSerilog((hostContext, services, configuration) =>
        {
            configuration.Enrich.FromLogContext()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
           .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Error)
           .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
           .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
           .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
           .WriteTo.File(logpath + ".log", rollingInterval: RollingInterval.Hour, retainedFileCountLimit: null);
        });
        builder.Services.AddHttpClient();
        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseMySql(
         builder.Configuration.GetConnectionString("DefaultConnection"),
         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
     ));

        builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        builder.Services.AddActionModuleServices();
        builder.Services.AddUserLoginAndPermissionModuleServices();
        builder.Services.AddRazorPages();
       
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseMiddleware<LogEnrichMiddleware>();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        app.UseAuthorization();
        app.UseSerilogRequestLogging();
        app.MapRazorPages();
        app.UseHttpLogging();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=Login}");
        app.Run();
    }
}
