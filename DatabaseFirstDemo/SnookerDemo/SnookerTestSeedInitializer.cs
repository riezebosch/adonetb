using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace SnookerDemo
{
    class SnookerTestSeedInitializer : DropCreateDatabaseAlways<SnookerContext>
    {
        protected override void Seed(SnookerContext context)
        {
            var ballen = Enumerable.Range(1, 9).Select(i => new Bal { Punten = i }).ToList();
            var s = new Score { Ballen = new List<Bal> { ballen[0], ballen[3] } };
            var p = new Persoon { Naam = "Pietje Puk", Scores = new List<Score> { s } };
            var w1 = new Wedstrijd { Omschrijving = "Eerste wedstrijd", Personen = new List<Persoon> { p } };
            s.Wedstrijd = w1;

            var w2 = new Wedstrijd
                {
                    Omschrijving = "Tweede westrijd",
                    Personen = new List<Persoon>
                    {
                        p,
                    }
                };

            var agent = new Persoon
            {
                Naam = "Agent Blauwdraad",
                Scores = new List<Score>
                {
                    new Score
                    {
                        Wedstrijd = w2,
                        Ballen = new List<Bal>
                        {
                            ballen[2], ballen[5], ballen[8]
                        }
                    }
                }
            };

            w2.Personen.Add(agent);
            p.Scores.Add(new Score { Ballen = new List<Bal> { ballen[2], ballen[4], ballen[6] }, Wedstrijd = w2 });

            context.Wedstrijden.AddRange(new[] { w1, w2 });
            context.SaveChanges();
        }
    }
}
