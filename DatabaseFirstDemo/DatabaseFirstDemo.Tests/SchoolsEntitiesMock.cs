using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFirstDemo.Tests
{
    class SchoolsEntitiesMock : SchoolEntities
    {
        public override System.Data.Entity.DbSet<Person> People
        {
            get;
            set;
        }
    }

    class DbSetMock<T> : DbSet<T>
    {

    }
}
