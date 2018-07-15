using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebSite.Domain.Common.Query
{
    public interface IQueryConditionTransformProvider
    {
        IQueryable<T> Search<T>(IQueryable<T> table, IEnumerable<ConditionItem> items);
    }
}
