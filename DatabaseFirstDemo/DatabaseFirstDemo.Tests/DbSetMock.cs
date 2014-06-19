using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFirstDemo.Tests
{
    class DbSetMock<T> : DbSet<T>, IQueryable<T>
        where T: class
    {
        IQueryable _data;
        public DbSetMock(IList<T> data)
        {
            _data = data.AsQueryable();
        }
        public Type ElementType { get { return _data.ElementType; } }
        public Expression Expression { get { return _data.Expression; } }
        public IQueryProvider Provider { get { return _data.Provider; } }

    }
}
