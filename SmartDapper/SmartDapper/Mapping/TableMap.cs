using SmartDapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SmartDapper.Mapping
{
    internal class TableMap<T>
    {
        private TableMap()
        {
        }

        public Type ObjectType { get; set; }
        public string TableName { get; set; }
        public IList<PropertyMap> PropertyMaps { get; set; }

        #region Static Factory
        public static TableMap<T> Create()
        {
            var cacheKey = string.Format("SmartDapper/{0}", typeof(T).FullName);
            var result = MemoryCache.Default[cacheKey] as TableMap<T>;
            if(result == null)
            {
                result = new TableMap<T>
                {
                    ObjectType = typeof(T),
                    TableName = getTableName(),
                    PropertyMaps = getPropertyMaps()
                };
                inferKeys(result);
                MemoryCache.Default.Set(cacheKey, result, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromDays(7) });
            }
            return result;
        }
        #endregion

        #region Private Static Methods
        private static string getTableName()
        {
            var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
            return tableAttribute != null && !string.IsNullOrWhiteSpace(tableAttribute.TableName)
                ? tableAttribute.TableName
                : typeof(T).Name;
        }

        private static IList<PropertyMap> getPropertyMaps()
        {
            var result = new List<PropertyMap>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);
            for (var i = 0; i < props.Length; i++)
            {
                if (props[i].GetCustomAttribute<NotMappedAttribute>() == null)
                {
                    var propertyMap = new PropertyMap
                    {
                        PropertyName = props[i].Name,
                        PropertyType = props[i].PropertyType,
                        ColumnName = getColumnName(props[i]),
                        DatabaseGeneratedOption = getDatabaseGeneratedOption(props[i]),
                        KeyOrder = getKeyOrder(props[i])
                    };
                    result.Add(propertyMap);
                }
            }
            return result;
        }

        private static string getColumnName(PropertyInfo prop)
        {
            var columnAttribute = prop.GetCustomAttribute<ColumnAttribute>();
            return columnAttribute != null && !string.IsNullOrWhiteSpace(columnAttribute.ColumnName)
                ? columnAttribute.ColumnName
                : prop.Name;
        }

        private static DatabaseGeneratedOption getDatabaseGeneratedOption(PropertyInfo prop)
        {
            var databaseGeneratedAttribute = prop.GetCustomAttribute<DatabaseGeneratedAttribute>();
            return databaseGeneratedAttribute != null
                ? databaseGeneratedAttribute.DatabaseGeneratedOption
                : DatabaseGeneratedOption.None;
        }

        private static int? getKeyOrder(PropertyInfo prop)
        {
            var keyAttribute = prop.GetCustomAttribute<KeyAttribute>();
            return keyAttribute != null
                ? keyAttribute.ColumnOrder
                : default(int?);
        }

        private static void inferKeys(TableMap<T> tableMap)
        {
            // If there is already one or more Keys specified, then just return.
            if (tableMap.PropertyMaps.Any(x => x.KeyOrder.HasValue))
            {
                return;
            }
            else
            {
                // Otherwise, find a PropertyMap that corresponds to a SQL Identity. That's the key.
                var prop = tableMap.PropertyMaps.FirstOrDefault(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity);
                if (prop != null)
                {
                    prop.KeyOrder = 1;
                    return;
                }
                else
                {
                    // Otherwise, find a PropertyMap with ColumnName == "ID". That's the key.
                    prop = tableMap.PropertyMaps.FirstOrDefault(x => string.Compare(x.ColumnName, "id", true) == 0);
                    if (prop != null)
                    {
                        prop.KeyOrder = 1;
                        return;
                    }
                    else
                    {
                        // Otherwise, find a PropertyMap with ColumnName == ClassName + "ID". That's the key.
                        prop = tableMap.PropertyMaps.FirstOrDefault(x => string.Compare(x.ColumnName, "", true) == 0);
                        if (prop != null)
                        {
                            prop.KeyOrder = 1;
                            return;
                        }
                    }
                }
            }
            // If we got here, then the key is missing. Throw an InvalidOperationException.
            throw new InvalidOperationException("Missing key - None specified, and none could be inferred from object properties.");
        }
        #endregion
    }
}
