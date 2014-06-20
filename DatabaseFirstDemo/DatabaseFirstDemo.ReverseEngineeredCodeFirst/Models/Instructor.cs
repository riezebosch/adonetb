using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseFirstDemo.ReverseEngineeredCodeFirst.Models
{
    public class Instructor : Person
    {
        public Nullable<System.DateTime> HireDate { get; set; }
    }
}
