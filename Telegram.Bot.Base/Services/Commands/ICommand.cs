using Telegram.Bot.Types;

namespace Telegram.Bot.Base.Services;

public interface ICommand
{
    Task<Message> Start(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
    Task<Message> Default(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}