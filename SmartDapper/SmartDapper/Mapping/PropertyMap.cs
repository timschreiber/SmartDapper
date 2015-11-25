using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDapper.Mapping
{
    internal class PropertyMap
    {
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public string ColumnName { get; set; }
        public DatabaseGeneratedOption DatabaseGeneratedOption { get; set; }
        public int? KeyOrder { get; set; }

        public string SqlSelectPart
        {
            get { return ColumnName == PropertyName ? ColumnName : string.Format("{0} AS {1}", ColumnName, PropertyName); }
        }
    }
}
