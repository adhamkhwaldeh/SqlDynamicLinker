using System;
using System.Collections.Generic;
using System.Text;

namespace AlJawad.SqlDynamicLinker.DynamicFilter
{
    public class BaseIdentifierFilter<TKey> : BaseFilter
    {
        public TKey Id { get; set; }
    }
}
