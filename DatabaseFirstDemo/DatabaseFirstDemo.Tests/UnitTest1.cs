using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data;
using System.Data.Entity;
using System.Transactions;

namespace DatabaseFirstDemo.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GivenListOfInstructors_ShouldContain_Instructor()
        {
            using (var context = new SchoolEntities())
            {
                Assert.IsTrue(
                    context.People.OfType<Instructor>().Any(p => p.FirstName == "Kim" && p.LastName == "Abercrombie"));
            }
        }

        [TestMethod]
        public void GivenListOfInstructors_ShouldNotContain_Student()
        {
            using (var context = new SchoolEntities())
            {
                Assert.IsFalse(
                    context.People.OfType<Instructor>().Any(p => p.FirstName == "Gytis" && p.LastName == "Barzdukas"));
            }
        }

        [TestMethod]
        public void GivenInstructor_ShouldContain_Location()
        {
            using (var context = new SchoolEntities())
            {
                Assert.IsTrue(context.People.OfType<Instructor>().Any(p => p.PersonID == 1 && p.Location == "17 Smith"));
            }
        }


        [TestMethod]
        public void GivenStudent_ShouldHave_StudentGrades()
        {
            using (var context = new SchoolEntities())
            {
                Assert.IsTrue(context.People.OfType<Student>().Any(s => s.FirstName == "Gytis" && s.Grades.Any(g => g.Course.Name == "Composition" && g.Grade == 4)));
            }
        }

        [TestMethod]
        public void GivenInstructor_ShouldHave_Course()
        {
            using (var context = new SchoolEntities())
            {
                Assert.IsTrue(context.People.OfType<Instructor>().Any(s => s.PersonID == 1 && s.Course.Any(g => g.Name == "Chemistry")));
            }
        }

        [TestMethod]
        public void GivenInstructor_ShouldNotHave_Course()
        {
            using (var context = new SchoolEntities())
            {
                Assert.IsFalse(context.People.OfType<Instructor>().Any(s => s.PersonID == 5 && s.Course.Any(g => g.Name == "Chemistry")));
            }
        }

        [TestMethod]
        public void GivenInstructor_ShouldNotBe_DynamicProxy()
        {
            using (var context = new SchoolEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var p = context.People.OfType<Instructor>().First();
                Assert.AreEqual(typeof(Instructor), p.GetType());
            }
        }

        [TestMethod]
        public void GivenProxyCreationDisabled_Should_LazyLoadingNotWorking()
        {
            using (var context = new SchoolEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = true;

                // First instructor with courses
                var p = context.People.OfType<Instructor>().Where(i => i.Course.Any()).First();

                // Courses not fetched because no proxy used
                Assert.AreEqual(0, p.Course.Count);
            }
        }

        [TestMethod]
        public void NotExistingInstructor_ShouldBe_Added()
        {
            // TransactionScope om onze test database niet te vervuilen. Handig!
            using (var ts = new TransactionScope())
            using (var context = new SchoolEntities())
            {
                var person = context
                    .People
                    .OfType<Student>()
                    .FirstOrDefault(p => p.FirstName == "Pietje" && p.LastName == "Puk");

                if (person == null)
                {
                    person = new Student
                    {
                        FirstName = "Pietje",
                        LastName = "Puk",
                        EnrollmentDate = DateTime.Today
                    };

                    context.People.Add(person);
                }
                else
                {
                    person.LastName = "Update";
                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    Console.WriteLine(string.Join(Environment.NewLine, ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors.Select(f => f.ErrorMessage))));
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void DetachedEntities_ShouldBe_AddedBeforeSaveChanges()
        {
            Person pietje = null;

            // Die scope werkt dus ook over contexten heen!
            using (new TransactionScope())
            {
                using (var context = new SchoolEntities())
                {
                    // AsNoTracking zorgt ervoor dat de ChangeTracker buitenspel wordt gezet.
                    pietje = context.People.AsNoTracking().First(p => p.PersonID == 33);
                }

                pietje.LastName = "AS NO TRACKING";

                using (var context = new SchoolEntities())
                {
                    // Slecht idee, twee pietjes in de DB
                    //context.People.Add(pietje);

                    context.Entry(pietje).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void DeleteFromContext()
        {
            using (var context = new SchoolEntities())
            {
                context.People.RemoveRange(context.People.Where(p => p.FirstName == "Pietje"));
                context.SaveChanges();
            }
        }
    }
}
