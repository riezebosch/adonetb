﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseFirstDemo
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SchoolEntities : DbContext
    {
        public SchoolEntities()
            : base("name=SchoolEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<OnsiteCourse> OnsiteCourses { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<StudentGrade> StudentGrades { get; set; }
        public virtual DbSet<OnlineCourse> OnlineCourses { get; set; }
    
        public virtual int DeletePerson(Nullable<int> personID)
        {
            var personIDParameter = personID.HasValue ?
                new ObjectParameter("PersonID", personID) :
                new ObjectParameter("PersonID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeletePerson", personIDParameter);
        }
    }
}
