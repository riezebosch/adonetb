using CodeFirstDemo.Fluent.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstDemo.Fluent.DAL
{
    public class CodeFirstContext : DbContext
    {

        public DbSet<Demo> Demos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add<Demo>(new DemoConfiguration());
            
            // Dit is verplaatst naar de DemoConfiguration class.
            //modelBuilder
            //    .Entity<Demo>()
            //    .HasKey(m => m.DemoIdentifier);

            //modelBuilder
            //    .Entity<Demo>()
            //    .Property(m => m.Cursus)
            //    .IsRequired();

            //modelBuilder
            //   .Entity<Demo>()
            //   .Property(m => m.Cursus)
            //   .HasMaxLength(50);

            //modelBuilder
            //    .Entity<Demo>()
            //    .ToTable("tbl_Demo");
        }
    }
}
