using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace DatabaseFirstDemo.ReverseEngineeredCodeFirst.Models.Mapping
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            this.Property(t => t.EnrollmentDate).HasColumnName("EnrollmentDate");
            Map(m => m.Requires(p => p.EnrollmentDate).HasValue());
        }
    }
}
