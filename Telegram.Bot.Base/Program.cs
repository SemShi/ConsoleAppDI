using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot.Base.Services;


var builder = new ConfigurationBuilder();
BuildConfig(builder);

// Serilog setup configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Application starting..");

// Register your DI services
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<IGreetingService, GreetingService>();
    })
    .UseSerilog()
    .Build();

// Creating instance of start point a programm
var app = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);

// Execute start method
app.Run();

// Plug appsettings.json in our builder
static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
}