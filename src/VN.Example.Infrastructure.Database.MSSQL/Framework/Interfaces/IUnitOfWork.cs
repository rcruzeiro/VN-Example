using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VN.Example.Platform.Domain;

namespace VN.Example.Infrastructure.Database.MSSQL
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }

        IEnumerable<T> Get<T>(Func<T, bool> predicate = null)
            where T : class, IAggregation;

        IEnumerable<T> Get<T>(ISpecification<T> specification)
            where T : class, IAggregation;

        T GetOne<T>(Func<T, bool> predicate = null)
            where T : class, IAggregation;

        T GetOne<T>(ISpecification<T> specification)
            where T : class, IAggregation;

        T Find<T>(params object[] keyValues)
            where T : class, IAggregation;

        T Add<T>(T entity)
            where T : class, IAggregation;

        void Add<T>(IEnumerable<T> entities)
            where T : class, IAggregation;

        T Update<T>(T entity)
            where T : class, IAggregation;

        void Update<T>(IEnumerable<T> entities)
            where T : class, IAggregation;

        void Remove<T>(T entity)
            where T : class, IAggregation;

        void Remove<T>(IEnumerable<T> entities)
            where T : class, IAggregation;

        int SaveChanges();

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
