using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.Entity;
using System.Transactions;

namespace CodeFirstDemo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GooiDeDatabaseWegAanHetBegin_MaakHemDaarnaAutomatischAan()
        {
            // Setting the default initializer because the other test may otherwise influence this one.
            Database.SetInitializer(new CreateDatabaseIfNotExists<CodeFirstContext>());
            
            using (var context = new CodeFirstContext())
            {
                context.Database.Delete();
            }

            using (var context = new CodeFirstContext())
            {
                context.Demos.Add(new Demo
                {
                    Cursus = "ADONETB",
                    Omschrijving = "asdf"
                });

                context.SaveChanges();
            }

            using (var context = new CodeFirstContext())
            {
                context.Demos.Single();
            }
        }


        [TestMethod]
        public void NuMetInitializer()
        {
            using (var context = new CodeFirstContext())
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<CodeFirstContext>());

                context.Demos.Add(new Demo
                {
                    Cursus = "ADONETB",
                    Omschrijving = "asdf"
                });

                context.SaveChanges();
            }

            using (var context = new CodeFirstContext())
            {
                context.Demos.Single();
            }
        }

        [TestMethod]
        public void NuMetInitieleVulling()
        {
            using (var context = new CodeFirstContext())
            {
                Database.SetInitializer(new MyCustomDropCreateAlwaysInitializer());
                context.Database.Initialize(true);
            }

            using (new TransactionScope())
            {
                using (var context = new CodeFirstContext())
                {
                    context.Demos.Add(new Demo
                    {
                        Cursus = "ADONETB",
                        Omschrijving = "asdf"
                    });

                    context.SaveChanges();
                }

                using (var context = new CodeFirstContext())
                {
                    Assert.AreEqual(2, context.Demos.Count());
                } 
            }
        }

        class MyCustomDropCreateAlwaysInitializer :
            DropCreateDatabaseAlways<CodeFirstContext>
        {
            protected override void Seed(CodeFirstContext context)
            {
                base.Seed(context);

                context.Demos.Add(new Demo
                    {
                        Cursus = "UNITTST",
                        Omschrijving = "Unit testing using Visual Studio 2013"
                    });
            }
        }
    }
}
