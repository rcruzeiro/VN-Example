using System.Threading;
using System.Threading.Tasks;

namespace VN.Example.Infrastructure.Provider.MessageBus
{
    public interface IMessageService
    {
        Task PublishAsync<T>(string topicName, T data, CancellationToken cancellationToken = default)
            where T : class;

        Task<T> ConsumeAsync<T>(string topicName, CancellationToken cancellationToken = default)
            where T : class;
    }
}
