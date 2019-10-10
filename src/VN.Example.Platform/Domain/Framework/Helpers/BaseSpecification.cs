using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VN.Example.Platform.Domain
{
    public class BaseSpecification<T> : ISpecification<T>
        where T : class, IAggregation

    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public List<string> IncludeStrings { get; } = new List<string>();

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        protected virtual void AddInclude(Expression<Func<T, object>> expression)
        {
            Includes.Add(expression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }
}
