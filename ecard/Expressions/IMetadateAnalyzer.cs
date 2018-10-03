using System;
using System.Collections.Generic;

namespace Oxite.Expressions
{
    public interface IMetadateAnalyzer
    {
        IList<ColumnMetadate> GetColumns(Type type);
    }
}