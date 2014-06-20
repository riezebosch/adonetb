using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnookerDemo
{
    class Wedstrijd
    {
        public int Id { get; set; }

        public string Omschrijving { get; set; }
        public IList<Persoon> Personen { get; set; }
        public IList<Score> Scores { get; set; }
    }
}
