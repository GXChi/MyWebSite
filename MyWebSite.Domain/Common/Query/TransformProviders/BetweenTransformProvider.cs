using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Common.Query.TransformProviders
{
    class BetweenTransformProvider : ITransformProvider
    {
        public bool Match(ConditionItem item, Type type)
        {
            return item.Method == QueryMethod.Between;
        }

        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            IList<ConditionItem> returnValue = new List<ConditionItem>();
            if (item.Value is string)
                item.Value = new[] { item.Value, null };
            var arr = (item.Value as string[]);
            if (arr == null)
                throw new ArgumentException("ConditionItem is not between type");
            if (arr.Length != 2)
                throw new ArgumentException("ConditionItem is not between type cause the length is not 2");
            if (arr[0] != null)
            {
                returnValue.Add(new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, arr[0]));
            }
            if (arr[1] != null)
            {
                returnValue.Add(new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, arr[1]));
            }
            return returnValue;
        }
    }
}
