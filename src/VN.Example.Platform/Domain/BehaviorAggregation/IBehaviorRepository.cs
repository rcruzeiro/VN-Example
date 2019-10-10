using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VN.Example.Platform.Domain.BehaviorAggregation
{
    public interface IBehaviorRepository : IDisposable
    {
        Task<IEnumerable<Behavior>> GetBehaviors(ISpecification<Behavior> spec, CancellationToken cancellationToken = default);

        Task CreateBehavior(Behavior behavior, CancellationToken cancellationToken = default);
    }
}
