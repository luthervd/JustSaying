using System.Threading;
using System.Threading.Tasks;
using JustSaying.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JustSaying.Sample.Restaurant.KitchenConsole
{
    /// <summary>
    /// A background service responsible for starting the bus which listens for
    /// messages on the configured queues
    /// </summary>
    public class Subscriber : BackgroundService
    {
        private readonly IMessagingBus _bus;
        private readonly ILogger<Subscriber> _logger;

        public Subscriber(IMessagingBus bus, ILogger<Subscriber> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kitchen subscriber running");

            _bus.StartAsync(stoppingToken);

            return Task.CompletedTask;
        }
    }
}
