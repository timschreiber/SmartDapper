using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DatabaseGeneratedAttribute : Attribute
    {
        public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption)
        {
            DatabaseGeneratedOption = databaseGeneratedOption;
        }

        internal DatabaseGeneratedOption DatabaseGeneratedOption { get; private set; }
    }
}
