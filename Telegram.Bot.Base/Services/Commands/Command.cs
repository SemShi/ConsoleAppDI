using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Base.Services;

public class Command : ICommand
{
    public async Task<Message> Start(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Добро пожаловать на борт, добрый путник!",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }

    public async Task<Message> Default(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Привет-привет!",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
}