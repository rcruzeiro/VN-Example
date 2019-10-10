using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace VN.Example.Infrastructure.Provider.MessageBus
{
    public class MessageService : IMessageService
    {
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;

        public MessageService(RabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration ?? throw new ArgumentNullException(nameof(rabbitMQConfiguration));
        }

        public Task PublishAsync<T>(string topicName, T data, CancellationToken cancellationToken = default)
            where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfiguration.HostName,
                Port = _rabbitMQConfiguration.Port,
                UserName = _rabbitMQConfiguration.Username,
                Password = _rabbitMQConfiguration.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(topicName,
                                     false,
                                     false,
                                     false,
                                     null);
                string message =
                    $"{DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")} - message: {JsonConvert.SerializeObject(data)}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(string.Empty,
                                     topicName,
                                     null,
                                     body);
            }

            return Task.CompletedTask;
        }

        public Task<T> ConsumeAsync<T>(string topicName, CancellationToken cancellationToken = default)
            where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            T result = default;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfiguration.HostName,
                Port = _rabbitMQConfiguration.Port,
                UserName = _rabbitMQConfiguration.Username,
                Password = _rabbitMQConfiguration.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(topicName,
                                     false,
                                     false,
                                     false,
                                     null);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (ch, e) =>
                {
                    var content = Encoding.UTF8.GetString(e.Body);
                    result = JsonConvert.DeserializeObject<T>(content);
                };

                channel.BasicConsume(topicName,
                                     true,
                                     consumer);
            }

            return Task.Run(() => result);
        }
    }
}
