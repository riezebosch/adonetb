using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstDemo
{
    public class CodeFirstContext : DbContext
    {
        public DbSet<Demo> Demos { get; set; }
    }
}
