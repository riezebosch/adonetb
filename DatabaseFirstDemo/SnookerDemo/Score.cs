﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnookerDemo
{
    class Score
    {
        public int Id { get; set; }
        public IList<Bal> Ballen { get; set; }

        public int PersoonID { get; set; }
        public Persoon Persoon { get; set; }

        public int WedstrijdID { get; set; }

        public Wedstrijd Wedstrijd { get; set; }
    }
}
