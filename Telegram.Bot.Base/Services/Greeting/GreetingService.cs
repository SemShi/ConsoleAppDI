using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Telegram.Bot.Base.Services;

public class GreetingService : IGreetingService
{
    private readonly ILogger<GreetingService> _logger;
    private readonly IConfiguration _cfg;

    public GreetingService(ILogger<GreetingService> logger, IConfiguration cfg)
    {
        _logger = logger;
        _cfg = cfg;
    }
    
    public void Run()
    {
        _logger.LogInformation("GreetingService is running!");
    }
}