using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Application.Common
{
    public class ExcelValidatorContainer
    {
       public int HeadRowNo { get; set; }

        public int DataStartRowNo { get; set; }

        public bool DynamicTable { get; set; }

        public SortedDictionary<int,IValidators> FormatValidators { get; set; }

        public List<InvokerInfo> ExtValidators { get; set; }

        public SortedDictionary<int,string> ColsName { get; set; }

        public SortedDictionary<int,string> ColsDesc { get; set; }

        public SortedDictionary<int,string> ColsType { get; set; }


    }
}
