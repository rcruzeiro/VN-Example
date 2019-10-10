using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VN.Example.Platform.Domain;
using VN.Example.Platform.Domain.BehaviorAggregation;

namespace VN.Example.Infrastructure.Database.MSSQL.Repositories
{
    public class BehaviorRepository : IBehaviorRepository
    {
        private readonly IUnitOfWorkAsync _unitOfWork;

        public BehaviorRepository(IUnitOfWorkAsync unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Behavior>> GetBehaviors(ISpecification<Behavior> spec, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.GetAsync(spec, cancellationToken);
        }

        public async Task CreateBehavior(Behavior behavior, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.AddAsync(behavior, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
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
                if (_unitOfWork != null) _unitOfWork.Dispose();
            }
        }
    }
}
