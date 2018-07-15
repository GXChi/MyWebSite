using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Common.Query
{
    public interface ITransformProvider
    {
        bool Match(ConditionItem item, Type type);

        IEnumerable<ConditionItem> Transform(ConditionItem item, Type type);
    }
}
