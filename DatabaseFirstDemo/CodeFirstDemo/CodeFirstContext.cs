using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstDemo
{
    public class CodeFirstContext : DbContext
    {
        static CodeFirstContext()
        {
            // Dit wordt zo de default initializer.
            Database.SetInitializer(new NullDatabaseInitializer<CodeFirstContext>());
        }

        public DbSet<Demo> Demos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
