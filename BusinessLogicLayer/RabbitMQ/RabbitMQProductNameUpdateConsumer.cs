using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQProductNameUpdateConsumer : IRabbitMQProductNameUpdateConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMQProductNameUpdateConsumer> _logger;
        public RabbitMQProductNameUpdateConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            string hostName = "localhost";
            string userName = "guest";
            string password = "guest";
            string port = "5672";
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = Convert.ToInt32(port)
        };
        _connection =connectionFactory.CreateConnection();

            _channel = _connection.CreateModel(); 
        }
        public void Consume()
        {
            string routingKey = "product.update.name";
            string queueName = "orders.product.update.name.queue";

            string exchangeName = "Products.exchange";
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null); 

            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, args) =>
            {
                byte[] body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                if (message != null)
                {
                    ProductNameUpdateMessage? productNameUpdateMessage = JsonSerializer.Deserialize<ProductNameUpdateMessage>(message);

                    _logger.LogInformation($"Product name updated: {productNameUpdateMessage.ProductID}, New name: {productNameUpdateMessage.NewName}");
                }
            };

            _channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);
        }
        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
 