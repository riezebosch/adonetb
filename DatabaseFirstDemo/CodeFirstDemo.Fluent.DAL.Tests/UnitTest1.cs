using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Transactions;
using CodeFirstDemo.Fluent.Model;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace CodeFirstDemo.Fluent.DAL.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [AssemblyInitialize]
        public static void TestMethod1(TestContext tc)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<CodeFirstContext>());

            using (var context = new CodeFirstContext())
            {
                context.Database.Initialize(true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void TestConcurrencyWithTimestampColumn()
        {
            using (new TransactionScope())
            {
                using (var context = new CodeFirstContext())
                {
                    context.Demos.Add(new Demo { Cursus = "ADONETB", Omschrijving = "Entity Framework with VS2013" });
                    context.SaveChanges();
                }

                using (var c1 = new CodeFirstContext())
                using (var c2 = new CodeFirstContext())
                {
                    var d1 = c1.Demos.First();
                    var d2 = c2.Demos.First();

                    d1.Cursus = "TEST1";
                    d2.Cursus = "TEST2";

                    c1.SaveChanges();
                    c2.SaveChanges();
                }
            }
        }
    }
}
