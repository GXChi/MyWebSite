using MyWebSite.Domain.Common.Query.QueryConditionProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.Common.Query
{
    public static class QueryFactory
    {
        public static IQueryConditionTransformProvider Provider { get; set; }

        public static void Register(IQueryConditionTransformProvider provider)
        {
            Provider = provider;
        }
        static QueryFactory()
        {
            Provider = new QueryableQueryConditionProvider();
        }


    }
}
