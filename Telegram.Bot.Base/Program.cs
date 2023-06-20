using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot.Base.Services;
using Telegram.Bot.Polling;
using Bit76.Database.Factories;


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
        services.AddSingleton<IBotStart, BotStart>();
        services.AddScoped<IUpdateHandler, UpdateHandler>();
        services.AddSingleton<ICommand, Command>();
        services.AddScoped<IDatabaseFactory, PostgresDatabaseFactory>();
    })
    .UseSerilog()
    .Build();

// Creating instance of start point a program
var app = ActivatorUtilities.CreateInstance<BotStart>(host.Services);

// Execute start method
await app.StartBotAsync();

// Plug appsettings.json in our builder
static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
}