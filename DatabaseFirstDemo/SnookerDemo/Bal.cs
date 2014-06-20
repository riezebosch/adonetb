using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnookerDemo
{
    class Bal
    {
        public int Id { get; set; }
        public int Punten { get; set; }

        public IList<Score> Scores { get; set; }
    }
}
