using SmartDapper.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace SmartDapper.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        private readonly TableMap<T> _tableMap;

        public Repository(IDbTransaction transaction)
        {
            _tableMap = TableMap<T>.Create();
        }

        protected IDbTransaction Transaction { get; private set; }

        protected IDbConnection Connection
        {
            get { return Transaction.Connection; }
        }

        private string _sqlSelect
        {
            get
            {
                return string.Format("SELECT {0} FROM {1}", string.Join(", ", _tableMap.PropertyMaps
                    .Select(x => x.ColumnName == x.PropertyName
                        ? x.ColumnName
                        : string.Format("{0} AS {1}", x.ColumnName, x.PropertyName)
                    )
                ), _tableMap.TableName);
            }
        }

        private string _sqlKeyConstraint
        {
            get
            {
                return string.Join(" AND ", _tableMap.PropertyMaps
                    .Where(x => x.KeyOrder.HasValue)
                    .OrderBy(x => x.KeyOrder.Value)
                    .Select(x => string.Format("{0} = @{1}", x.ColumnName, x.PropertyName))
                );
            }
        }

        public IEnumerable<T> All()
        {
            return Connection.Query<T>(
                sql: _sqlSelect,
                transaction: Transaction
            );
        }

        public T Find(object key)
        {
            return Connection.Query<T>(
                sql: string.Format("{0} {1}", _sqlSelect, _sqlKeyConstraint),
                param: key,
                transaction: Transaction
            ).FirstOrDefault();
        }

        public void Add(T obj)
        {
            //public string getSqlInsert()
            //{
            //    var columnsPart = string.Join(", ", _tableMap.PropertyMaps.Select(x => x.ColumnName));
            //    var parametersPart = string.Join(", ", _tableMap.PropertyMaps.Where(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.None).Select(x => string.Format("@{0}", x.PropertyName)));
            //    var result = string.Format("INSERT INTO {0}({1}) VALUES({2})", _tableMap.TableName, columnsPart, parametersPart);
            //    if (_tableMap.PropertyMaps.Any(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity))
            //        result = string.Format("{0}; SELECT SCOPE_IDENTITY()");
            //    return result;
            //}
            throw new NotImplementedException();
        }

        public void Update(T obj)
        {
            //public string getSqlUpdate()
            //{
            //    var setsPart = string.Join(" AND ", _tableMap.PropertyMaps.Where(x => x.DatabaseGeneratedOption == DatabaseGeneratedOption.None).Select(x => string.Format("{0} = @{1}", x.ColumnName, x.PropertyName)));
            //    var constraintsPart = string.Join(" AND ", _tableMap.PropertyMaps.Where(x => x.KeyOrder.HasValue).OrderBy(x => x.KeyOrder.Value).Select(x => string.Format("{0} = @{1}", x.ColumnName, x.PropertyName)));
            //    return string.Format("UPDATE {0} SET {1} WHERE {2}", _tableMap.TableName, setsPart, constraintsPart);
            //}
            throw new NotImplementedException();
        }

        public void Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(object key)
        {
            Connection.Execute(
                sql: string.Format("DELETE FROM {0} WHERE {1}", _tableMap.TableName, _sqlKeyConstraint),
                param: key,
                transaction: Transaction
            );
        }
    }
}
