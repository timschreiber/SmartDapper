using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDapper.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> All();
        T Find(object key);
        void Add(T obj);
        void Update(T obj);
        void Delete(object key);
    }
}
