using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VN.Example.Platform.Domain;

namespace VN.Example.Infrastructure.Database.MSSQL
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<IEnumerable<T>> GetAsync<T>(Func<T, bool> predicate = null, CancellationToken cancellationToken = default)
            where T : class, IAggregation;

        Task<IEnumerable<T>> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default)
            where T : class, IAggregation;

        Task<T> GetOneAsync<T>(Func<T, bool> predicate = null, CancellationToken cancellationToken = default)
            where T : class, IAggregation;

        Task<T> GetOneAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default)
            where T : class, IAggregation;

        Task<T> FindAsync<T>(object[] keyValues, CancellationToken cancellationToken = default)
            where T : class, IAggregation;

        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class, IAggregation;

        Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class, IAggregation;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
