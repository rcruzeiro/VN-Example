using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Couchbase;
using VN.Example.Platform.Domain;
using VN.Example.Platform.Domain.BehaviorAggregation;

namespace VN.Example.Infrastructure.Database.Couchbase.Repositories
{
    public class BehaviorRepository : BaseRepository, IBehaviorRepository
    {
        public BehaviorRepository(CouchbaseOptions options)
            : base(options)
        { }

        public Task<IEnumerable<Behavior>> GetBehaviors(ISpecification<Behavior> spec, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task CreateBehavior(Behavior behavior, CancellationToken cancellationToken = default)
        {
            var document = new Document<dynamic>
            {
                Id = Guid.NewGuid().ToString(),
                Content = new
                {
                    behavior.Id,
                    behavior.IP,
                    behavior.PageName,
                    behavior.UserAgent,
                    behavior.PageParameters,
                    behavior.CreatedAt
                }
            };

            var upsert = await Bucket.UpsertAsync(document);

            upsert.EnsureSuccess();
        }
    }
}
