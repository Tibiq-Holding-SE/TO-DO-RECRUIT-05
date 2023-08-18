using TaskManagerLite.Interfaces;

namespace TaskManagerLite.Services.Background
{
    public class BackgroundWorker : BackgroundService
    {
        private int _minutesDelaySpan => 0;
        private int _secondsDelaySpan => 5;
        private readonly IServiceProvider _services;

        public BackgroundWorker(IServiceProvider serviceProvider)
        {
            _services = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await CorrectTimerAsync();
                await BeatAsync();
            }
        }

        private async Task CorrectTimerAsync()
        {
            await Task.Delay(new TimeSpan(0, _minutesDelaySpan, _secondsDelaySpan));
        }

        private async Task BeatAsync()
        {
            await using var scope = _services.CreateAsyncScope();

            var backgoundRepo = scope.ServiceProvider.GetRequiredService<IBackgroundRepository>();

            await backgoundRepo.MarkCloseToOverdueTasksAsync();
            await backgoundRepo.MarkOverdueTasksAsync();
            await backgoundRepo.DeleteOldTasksAsync();
        }
    }
}
