using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Base.Services;

public class UpdateHandler : IUpdateHandler
{
#region DI services
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IConfiguration _cfg;

    private readonly ICommand _command;
#endregion
    
    public UpdateHandler(ILogger<UpdateHandler> logger, IConfiguration cfg, ICommand command)
    {
        _logger = logger;
        _cfg = cfg;
        _command = command;
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("New message: {Update}", Newtonsoft.Json.JsonConvert.SerializeObject(update));
        
        var handler = update switch
        {
            { Message: { } message }                       => BotOnMessageReceived(botClient, message, cancellationToken),
            { EditedMessage: { } message }                 => BotOnMessageReceived(botClient, message, cancellationToken),
            { CallbackQuery: { } callbackQuery }           => BotOnCallbackQueryReceived(botClient, callbackQuery, cancellationToken),
            { InlineQuery: { } inlineQuery }               => BotOnInlineQueryReceived(botClient, inlineQuery, cancellationToken),
            { ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(botClient, chosenInlineResult, cancellationToken),
            _                                              => UnknownUpdateHandlerAsync(botClient, update, cancellationToken)
        };
        await handler;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError("Exception: {Exception}", Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }

    private async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        
        if (message.Text is not { } messageText)
            return;
        
        var action = messageText.ToLower().Split(' ')[0] switch
        {
             "/start"           => _command.Start(botClient, message, cancellationToken),
             _                  => _command.Default(botClient, message, cancellationToken)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

    }
    private async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken){}
    private async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery, CancellationToken cancellationToken){}
    private async Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken){}
    private Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken){return Task.CompletedTask;}
    
}