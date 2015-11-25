using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        public KeyAttribute(int columnOrder = 0)
        {
            ColumnOrder = columnOrder;
        }

        internal int ColumnOrder { get; private set; }
    }
}
