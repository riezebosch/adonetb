using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace SnookerDemo
{
    class SnookerContext : DbContext
    {
        public virtual DbSet<Wedstrijd> Wedstrijden { get; set; }
    }
}
