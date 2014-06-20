using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace DatabaseFirstDemo.ReverseEngineeredCodeFirst.Models.Mapping
{
    public class InstructorMap : EntityTypeConfiguration<Instructor>
    {
        public InstructorMap()
        {
            this.Property(t => t.HireDate)
                .IsRequired();

            this.Property(t => t.HireDate).HasColumnName("HireDate");

            this.ToTable("Instructor");
        }
    }
}
