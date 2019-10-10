using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VN.Example.Platform.Domain;

namespace VN.Example.Infrastructure.Database.MSSQL
{
    public abstract class BaseContext : DbContext, IUnitOfWorkAsync
    {
        IDbContextTransaction transaction;

        protected string ConnectionString { get; }

        DbContext IUnitOfWork.Context => this;

        protected BaseContext()
        { }

        protected BaseContext(DbContextOptions options)
            : base(options)
        { }

        protected BaseContext(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected BaseContext(IDataSource source)
            : this(source.GetConnectionString())
        { }

        IEnumerable<T> IUnitOfWork.Get<T>(Func<T, bool> predicate)
        {
            return Set<T>()
                .NullSafeWhere(predicate)
                .ToList();
        }

        async Task<IEnumerable<T>> IUnitOfWorkAsync.GetAsync<T>(Func<T, bool> predicate, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                Set<T>().NullSafeWhere(predicate).ToList());
        }

        IEnumerable<T> IUnitOfWork.Get<T>(ISpecification<T> specification)
        {
            var query = GetSpecIQueryable(specification);

            return query
                .NullSafeWhere(specification.Criteria)
                .ToList();
        }

        async Task<IEnumerable<T>> IUnitOfWorkAsync.GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            var query = GetSpecIQueryable(specification);

            return await query
                .NullSafeWhere(specification.Criteria)
                .ToListAsync(cancellationToken);
        }

        T IUnitOfWork.GetOne<T>(Func<T, bool> predicate)
        {
            return Set<T>()
                .NullSafeWhere(predicate)
                .SingleOrDefault();
        }

        async Task<T> IUnitOfWorkAsync.GetOneAsync<T>(Func<T, bool> predicate, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                Set<T>().NullSafeWhere(predicate).SingleOrDefault());
        }

        T IUnitOfWork.GetOne<T>(ISpecification<T> specification)
        {
            var query = GetSpecIQueryable(specification);

            return query
                .NullSafeWhere(specification.Criteria)
                .SingleOrDefault();
        }

        async Task<T> IUnitOfWorkAsync.GetOneAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            var query = GetSpecIQueryable(specification);

            return await query
                .NullSafeWhere(specification.Criteria)
                .SingleOrDefaultAsync(cancellationToken);
        }

        T IUnitOfWork.Find<T>(params object[] keyValues)
        {
            return Find<T>(keyValues);
        }

        async Task<T> IUnitOfWorkAsync.FindAsync<T>(object[] keyValues, CancellationToken cancellationToken)
        {
            return await FindAsync<T>(keyValues, cancellationToken);
        }

        T IUnitOfWork.Add<T>(T entity)
        {
            return Add(entity).Entity;
        }

        void IUnitOfWork.Add<T>(IEnumerable<T> entities)
        {
            AddRange(entities);
        }

        async Task IUnitOfWorkAsync.AddAsync<T>(T entity, CancellationToken cancellationToken)
        {
            await AddAsync(entity, cancellationToken);
        }

        async Task IUnitOfWorkAsync.AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await AddRangeAsync(entities, cancellationToken);
        }

        T IUnitOfWork.Update<T>(T entity)
        {
            return Update(entity).Entity;
        }

        void IUnitOfWork.Update<T>(IEnumerable<T> entities)
        {
            UpdateRange(entities);
        }

        void IUnitOfWork.Remove<T>(T entity)
        {
            Remove(entity);
        }

        void IUnitOfWork.Remove<T>(IEnumerable<T> entities)
        {
            RemoveRange(entities);
        }

        int IUnitOfWork.SaveChanges()
        {
            return SaveChanges();
        }

        async Task<int> IUnitOfWorkAsync.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await SaveChangesAsync(cancellationToken);
        }

        void IUnitOfWork.BeginTransaction()
        {
            transaction = Database.BeginTransaction();
        }

        void IUnitOfWork.Commit()
        {
            if (transaction != null)
                transaction.Commit();
        }

        void IUnitOfWork.Rollback()
        {
            if (transaction != null)
                transaction.Rollback();
        }

        public override void Dispose()
        {
            if (transaction != null)
                transaction.Dispose();

            base.Dispose();
        }

        private IQueryable<T> GetSpecIQueryable<T>(ISpecification<T> spec)
            where T : class, IAggregation
        {
            var includes = spec.Includes
                .Aggregate(Set<T>().AsQueryable(),
                (current, include) => current.Include(include));

            var stringIncludes = spec.IncludeStrings
                .Aggregate(includes,
                (current, include) => current.Include(include));

            return stringIncludes;
        }
    }

    static class NullSafeExtensions
    {
        internal static IEnumerable<T> NullSafeWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
        {
            var result = predicate == null ? source : source.Where(predicate);

            return result;
        }

        internal static IQueryable<T> NullSafeWhere<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate = null)
        {
            var result = predicate == null ? source : source.Where(predicate).AsQueryable();

            return result;
        }
    }
}
