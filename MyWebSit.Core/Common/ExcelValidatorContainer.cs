using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Core.Common
{
    public class ExcelValidatorContainer
    {
        public int HeadRowNo { get; set; }

        public int DataStartRowNo { get; set; }

        public bool DynamicTable { get; set; }

        public int DynamicStartColNo { get; set; }

        public SortedDictionary<int, IValidators> FormatValidators { get; set; } = new SortedDictionary<int, IValidators>();

        public List<InvokerInfo> ExtValidators { get; set; } = new List<InvokerInfo>();

        public SortedDictionary<int, string> ColsName { get; set; } = new SortedDictionary<int, string>();

        public SortedDictionary<int, string> ColsDesc { get; set; } = new SortedDictionary<int, string>();

        public SortedDictionary<int, string> ColsType { get; set; } = new SortedDictionary<int, string>();

    }
}
