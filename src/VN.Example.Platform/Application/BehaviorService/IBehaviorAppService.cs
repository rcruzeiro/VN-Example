using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VN.Example.Platform.Application.BehaviorService.DTOs;

namespace VN.Example.Platform.Application.BehaviorService
{
    public interface IBehaviorAppService
    {
        Task<IEnumerable<BehaviorDto>> GetBehaviorsByIPAsync(string ip, CancellationToken cancellationToken = default);
        Task<IEnumerable<BehaviorDto>> GetBehaviorsByPageNameAsync(string pageName, CancellationToken cancellationToken = default);
        Task<BehaviorDto> GetBehaviorAsync(string ip, string pageName, string userAgent, CancellationToken cancellationToken = default);

        Task CreateBehaviorAsync(CreateBehaviorDto dto, CancellationToken cancellationToken = default);
    }
}
