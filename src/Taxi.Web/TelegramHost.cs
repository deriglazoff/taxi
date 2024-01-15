namespace BotSample
{

    public class TelegramHost : IHostedService
    {

        private static HandlerBot _handlerBot;

        public TelegramHost(HandlerBot handlerBot)
        {
            _handlerBot = handlerBot;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _handlerBot.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _handlerBot.Stop();
            return Task.CompletedTask;
        }
    }
}