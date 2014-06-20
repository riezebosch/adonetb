using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnookerDemo
{
    class Persoon
    {
        public int Id { get; set; }

        public string Naam { get; set; }

        public IList<Score> Scores { get; set; }

        public IList<Wedstrijd> Wedstrijden { get; set; }
    }
}
