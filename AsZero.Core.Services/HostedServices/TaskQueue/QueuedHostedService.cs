using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsZero.Core.Services.HostedServices
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly IServiceProvider _sp;
        private readonly IBackgroundTaskQueue _taskQueue;

        public QueuedHostedService(
            IBackgroundTaskQueue taskQueue,
            IServiceProvider sp,
            ILogger<QueuedHostedService> logger
        ){
            _taskQueue = taskQueue;
            _logger = logger;
            this._sp = sp;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation( $"Queued Hosted Service is running");
            await BackgroundProcessing(stoppingToken);

            async Task BackgroundProcessing(CancellationToken ct)
            {
                while (!ct.IsCancellationRequested)
                {
                    var workItem = await _taskQueue.DequeueAsync(ct);
                    try
                    {
                        await workItem(this._sp, ct).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                    }
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }


}
