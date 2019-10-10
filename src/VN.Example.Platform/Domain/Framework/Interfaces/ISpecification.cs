using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VN.Example.Platform.Domain
{
    public interface ISpecification<T>
        where T : class, IAggregation

    {
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }

        List<string> IncludeStrings { get; }
    }
}
