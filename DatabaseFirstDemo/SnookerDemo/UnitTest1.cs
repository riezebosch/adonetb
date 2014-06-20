using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace SnookerDemo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Database.SetInitializer(new SnookerTestSeedInitializer());
            using (var context = new SnookerContext())
            {
                var totaal = from w in context.Wedstrijden
                             from p in w.Personen
                             from s in p.Scores
                             //from b in s.Ballen
                             select new { w.Omschrijving, p.Naam, Punten = s.Ballen.Sum(b => b.Punten) };

                Console.WriteLine(totaal);

                foreach (var item in totaal)
                {
                    Console.WriteLine("{0} - {1}: {2}", item.Omschrijving, item.Naam, item.Punten);
                }

                var w1 = totaal.Where(w => w.Omschrijving == "Eerste wedstrijd");
                Assert.AreEqual(2, w1.Count());
            }
        }
    }
}
