using CodeFirstDemo.Fluent.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace CodeFirstDemo.Fluent.DAL
{
    class DemoConfiguration : EntityTypeConfiguration<Demo>
    {
        public DemoConfiguration()
        {
            this.HasKey(m => m.DemoIdentifier);

            this.Property(m => m.Cursus)
                .IsRequired();

            this.Property(m => m.Cursus)
               .HasMaxLength(50);

            this.Property(m => m.Timestamp)
                .IsRowVersion();

            this.ToTable("tbl_Demo");
        }
    }
}
