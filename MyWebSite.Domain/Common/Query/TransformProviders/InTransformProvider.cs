﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Common.Query.TransformProviders
{
    public class InTransformProvider : ITransformProvider
    {
        public bool Match(ConditionItem item, Type type)
        {
            return item.Method == QueryMethod.ExIn;
        }

        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            var arr = (item.Value as Array);
            if (arr == null)
            {
                var arrStr = item.Value.ToString();
                if (!string.IsNullOrEmpty(arrStr))
                {
                    arr = arrStr.Split(',');
                }
            }
            return new[] { new ConditionItem(item.Field, QueryMethod.In, arr) };
        }
    }
}
