using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Base.Services;

public class BotStart : IBotStart
{
#region Fileds
    private static ITelegramBotClient bot;
#endregion

#region DI services
    private readonly ILogger<BotStart> _logger;
    private readonly IConfiguration _cfg;
    private readonly IUpdateHandler _updateHandler;
#endregion

    public BotStart(ILogger<BotStart> logger, IConfiguration cfg, IUpdateHandler updateHandler)
    {
        _logger = logger;
        _cfg = cfg;
        _updateHandler = updateHandler;
    }

    public async Task<bool> ConfigureBot()
    {
        bool taskResult = false;
        await Task.Run(() =>
        {
            taskResult = !string.IsNullOrWhiteSpace(_cfg.GetValue<string>("BotToken"));
            if (taskResult)
            {
                bot = new TelegramBotClient(_cfg.GetValue<string>("BotToken")!);
                _logger.LogInformation("Bot instance created!");
            }
            else
                _logger.LogError("BotToken in appsettings.json is empty!");
        });
        
        return taskResult;
    }

    public async Task StartBotAsync()
    {
        if(!await ConfigureBot()) return;
        
        _logger.LogInformation("Bot " + bot.GetMeAsync().Result.FirstName + " started.");
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        bot.StartReceiving(
            _updateHandler.HandleUpdateAsync,
            _updateHandler.HandlePollingErrorAsync,
            receiverOptions,
            cancellationToken
        );
        _logger.LogInformation("Waiting on messages...");
        Console.ReadLine();
    }
}