using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace VN.Example.Infrastructure.Provider.MessageBus
{
    public interface IMessageService : IDisposable
    {
        Task PublishAsync<T>(string topicName, T data, CancellationToken cancellationToken = default)
            where T : class;

        Task ConsumeAsync(string topicName, EventHandler<BasicDeliverEventArgs> handler, CancellationToken cancellationToken = default);
    }
}
