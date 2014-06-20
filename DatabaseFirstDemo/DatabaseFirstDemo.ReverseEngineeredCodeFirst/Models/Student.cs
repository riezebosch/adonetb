using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseFirstDemo.ReverseEngineeredCodeFirst.Models
{
    public class Student : Person
    {
        public Nullable<System.DateTime> EnrollmentDate { get; set; }
    }
}
