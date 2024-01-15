using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotSample
{
    public class HandlerBot
    {
        private readonly ILogger _logger;

        private readonly ITelegramBotClient _botClient;

        private CancellationTokenSource _cts;

        private List<string> _commands;
        public HandlerBot(ILogger<HandlerBot> logger, ITelegramBotClient botClient)
        {
            _logger = logger;
            _botClient = botClient;

            _commands = new List<string>()
            {
                "Чего тебе надо?",
                "Я буду кричать!",
                "Ты кто такой?",
                "ААААА!"
            };

        }
        public void Stop()
        {
            _cts?.Cancel();
        }
        public void Start()
        {
            _cts = new CancellationTokenSource();
            var cancellationToken = _cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {

                if (update.Type == UpdateType.Message || update.Type == UpdateType.CallbackQuery)
                {
                    string response;
                    var message = update.Message;
                    message ??= update.CallbackQuery.Message;
                    if (message is not null && message.Text.ToLower() == "/start")
                    {
                        response = "Привет";
                    }
                    else
                    {
                        var max = _commands.Count();

                        var num = new Random().Next(0, max);
                        response = _commands.ElementAt(num);
                    }

                    IEnumerable<InlineKeyboardButton[]> buttons = new[] { new InlineKeyboardButton[] { new InlineKeyboardButton("Еще") { CallbackData = "d" } } };
                    var keyboard = new InlineKeyboardMarkup(buttons);

                    _logger.LogDebug("{chat} {req} {res}", message.Chat.Id, message?.Text, response);

                    await botClient.SendTextMessageAsync(message.Chat.Id,response);
                }

            }
            catch (Exception e)
            {

                _logger.LogError(e, "{@update}", update);
            }

        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "error");
            Thread.Sleep(1000);
            return Task.CompletedTask;
        }

    }
}