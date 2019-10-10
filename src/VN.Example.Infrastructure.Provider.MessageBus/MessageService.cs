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
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageService(RabbitMQConfiguration rabbitMQConfiguration)
        {
            if (rabbitMQConfiguration == null) throw new ArgumentNullException(nameof(rabbitMQConfiguration));

            var factory = new ConnectionFactory
            {
                HostName = rabbitMQConfiguration.HostName,
                Port = rabbitMQConfiguration.Port,
                UserName = rabbitMQConfiguration.Username,
                Password = rabbitMQConfiguration.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync<T>(string topicName, T data, CancellationToken cancellationToken = default)
            where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();

            _channel.QueueDeclare(topicName,
                                 false,
                                 false,
                                 false,
                                 null);
            string message = JsonConvert.SerializeObject(data);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(string.Empty,
                                 topicName,
                                 null,
                                 body);

            return Task.CompletedTask;
        }

        public Task ConsumeAsync(string topicName, EventHandler<BasicDeliverEventArgs> handler, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _channel.QueueDeclare(topicName,
                                 false,
                                 false,
                                 false,
                                 null);
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += handler;

            _channel.BasicConsume(topicName,
                                 true,
                                 consumer);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_channel != null) _channel.Dispose();
                if (_connection != null) _connection.Dispose();
            }
        }
    }
}
