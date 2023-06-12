namespace Telegram.Bot.Base.Services;

public interface IBotStart
{
    Task<bool> ConfigureBot();
    Task StartBotAsync();
}