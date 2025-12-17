using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQProductNameUpdateHostedService : IHostedService
    {
        private readonly IRabbitMQProductNameUpdateConsumer _rabbitMQProductNameUpdateConsumer;
        public RabbitMQProductNameUpdateHostedService(IRabbitMQProductNameUpdateConsumer rabbitMQProductNameUpdateConsumer)
        {
            _rabbitMQProductNameUpdateConsumer = rabbitMQProductNameUpdateConsumer;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _rabbitMQProductNameUpdateConsumer.Consume();
            await Task.CompletedTask;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMQProductNameUpdateConsumer.Dispose();
            await Task.CompletedTask;
        }
    }
}
