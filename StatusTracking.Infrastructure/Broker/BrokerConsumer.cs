using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProcessService.Infrastructure.Broker
{
    public class BrokerConsumer
    {
        readonly IBrokerConnection _brokerConnection;

        public BrokerConsumer(IBrokerConnection brokerConnection)
        {
            _brokerConnection = brokerConnection;
        }

        public void BrokerStartConsumer<T>(string queueName, string exchange, string routingKey, Action<T> callback)
        {
            var channel = PrepareChannel(queueName, exchange, routingKey);
            var consumer = CreateConsumer(channel, queueName, routingKey, (message) =>
            {
                try
                {
                    T deserializedObject = JsonSerializer.Deserialize<T>(message);
                    if (deserializedObject != null)
                    {
                        callback(deserializedObject);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao desserializar a mensagem: {ex.Message}");
                }
            });

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        public void BrokerStartConsumer(string queueName, string exchange, string routingKey, Action<string> callback)
        {
            var channel = PrepareChannel(queueName, exchange, routingKey);
            var consumer = CreateConsumer(channel, queueName, routingKey, (message) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        callback(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
                }
            });

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private IModel PrepareChannel(string queueName, string exchange, string routingKey)
        {
            var channel = _brokerConnection.CreateChannel();

            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true, autoDelete: false);
            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchange, routingKey: routingKey);

            return channel;
        }

        private EventingBasicConsumer CreateConsumer(IModel channel, string queueName, string routingKey, Action<string> processMessage)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Queue name: {queueName}");
                Console.WriteLine($"Routing key: {routingKey}");

                processMessage(message);

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            return consumer;
        }
    }
}
