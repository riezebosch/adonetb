using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Data;
using System.Data.Entity;
using System.Transactions;
using System.Text;
using System.Data.Entity.Infrastructure;
using Moq;
using System.Collections.Generic;

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

        [TestMethod]
        public void LoadByKey_ShouldReturn_EntityWithKeyOrNullIfNotFound()
        {
            using (var context = new SchoolEntities())
            {
                var p = context.People.Find(1);
                Assert.IsNotNull(p);

                var ne = context.People.Find(12345);
                Assert.IsNull(ne);
            }
        }

        [TestMethod]
        public void LoadByKey_ShouldNot_InvokeQueryOnSubsequentRequest()
        {
            using (var context = new SchoolEntities())
            {
                var sb = new StringBuilder();
                context.Database.Log = s => sb.Append(s);

                var isaiah = context.People.First(p => p.FirstName == "Isaiah");
                Console.WriteLine(sb.ToString());
                Assert.AreNotEqual("", sb.ToString(), "Expecting DB lookup with First");

                sb.Clear();
                context.People.Find(isaiah.PersonID);
                Assert.AreEqual("", sb.ToString(), "Expecting entity loaded from cache with Find");

                context.People.First(p => p.PersonID == isaiah.PersonID);
                Console.WriteLine(sb.ToString());
                Assert.AreNotEqual("", sb.ToString(), "Expecting DB lookup with First");

            }
        }

        [TestMethod]
        public void UsingStoredProcedureOnContext()
        {
            using (new TransactionScope())
            using (var context = new SchoolEntities())
            {
                context.DeletePerson(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void ConcurrentUpdate_ShouldNot_AffectRecord()
        {
            using (new TransactionScope())
            {
                using (var context1 = new SchoolEntities())
                using (var context2 = new SchoolEntities())
                {
                    var p1 = context1.People.Find(1);
                    var p2 = context2.People.Find(1);

                    p1.FirstName = "UPDATE";
                    context1.SaveChanges();

                    p2.FirstName = "SOMETHING ELSE";
                    context2.SaveChanges();
                }

                using (var context = new SchoolEntities())
                {
                    Assert.AreEqual("UPDATE", context.People.Find(1).FirstName);
                }
            }
        }

        [TestMethod]
        public void ConcurrentUpdate_DatabaseWins()
        {
            using (new TransactionScope())
            {
                using (var context1 = new SchoolEntities())
                using (var context2 = new SchoolEntities())
                {
                    var p1 = context1.People.Find(1);
                    var p2 = context2.People.Find(1);

                    p1.FirstName = "UPDATE";
                    context1.SaveChanges();

                    try
                    {
                        p2.FirstName = "SOMETHING ELSE";
                        context2.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // Refreshes the tracked entities with new values from the database.
                        ex.Entries.Single().Reload();
                    }

                    Assert.AreEqual(p2.FirstName, "UPDATE");
                    Assert.AreEqual(EntityState.Unchanged, context2.Entry(p2).State);
                    
                    context2.SaveChanges();
                }

                using (var context = new SchoolEntities())
                {
                    Assert.AreEqual("UPDATE", context.People.Find(1).FirstName);
                }
            }
        }

        [TestMethod]
        public void ConcurrentUpdate_ClientWins()
        {
            using (new TransactionScope())
            {
                using (var context1 = new SchoolEntities())
                using (var context2 = new SchoolEntities())
                {
                    var p1 = context1.People.Find(1);
                    var p2 = context2.People.Find(1);

                    p1.FirstName = "UPDATE";
                    context1.SaveChanges();

                    try
                    {
                        p2.FirstName = "SOMETHING ELSE";
                        context2.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());

                        context2.SaveChanges();
                    }

                    Assert.AreEqual(p2.FirstName, "SOMETHING ELSE");
                    Assert.AreEqual(EntityState.Unchanged, context2.Entry(p2).State);

                    context2.SaveChanges();
                }

                using (var context = new SchoolEntities())
                {
                    Assert.AreEqual("SOMETHING ELSE", context.People.Find(1).FirstName);
                }
            }
        }

        [TestMethod]
        public void ConcurrentUpdate_ClietnChooses()
        {
            using (new TransactionScope())
            {
                using (var context1 = new SchoolEntities())
                using (var context2 = new SchoolEntities())
                {
                    var p1 = context1.People.Find(1);
                    var p2 = context2.People.Find(1);

                    p1.FirstName = 
                        p1.LastName = "UPDATE";
                    
                    context1.SaveChanges();

                    try
                    {
                        p2.FirstName = 
                            p2.LastName = "SOMETHING ELSE";
                        
                        context2.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();

                        var current = entry.CurrentValues;
                        var other = entry.GetDatabaseValues();
                        var resolved = other.Clone();

                        ClientChooses(current, other, resolved);

                        entry.OriginalValues.SetValues(other);
                        entry.CurrentValues.SetValues(resolved);

                        context2.SaveChanges();
                    }

                    Assert.AreEqual(p2.FirstName, "UPDATE");
                    Assert.AreEqual(p2.LastName, "SOMETHING ELSE");
                    Assert.AreEqual(EntityState.Unchanged, context2.Entry(p2).State);

                    context2.SaveChanges();
                }

                using (var context = new SchoolEntities())
                {
                    
                    var p = context.People.Find(1);
                    Assert.AreEqual("UPDATE", p.FirstName);
                    Assert.AreEqual("SOMETHING ELSE", p.LastName);
                }
            }
        }

        private void ClientChooses(DbPropertyValues current, DbPropertyValues other, DbPropertyValues resolved)
        {
            resolved["FirstName"] = other["FirstName"];
            resolved["LastName"] = current["LastName"];
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void ConcurrencyCheck_ShouldBe_OnAllFields()
        {
            using (new TransactionScope())
            using (var context1 = new SchoolEntities())
            using (var context2 = new SchoolEntities())
            {
                var p1 = context1.People.OfType<Instructor>().First();
                var p2 = context2.People.Find(p1.PersonID) as Instructor;

                p1.Location = "Smalle Zijde 35";
                context1.SaveChanges();

                p2.Location = "Kruisboog 42";
                context2.SaveChanges();
            }
        }

        [TestMethod]
        public void ContextUsingMock_ShouldNot_BeDependantOnADabtabase()
        {
            var grades = new List<StudentGrade>
            {
                new StudentGrade
                {
                    Grade = 4, 
                    Course = new Course { Name = "ADONETB" }, 
                    StudentID = 25
                }
            }.AsQueryable();

            var data = new List<Person>
            {
                new Student
                {
                    PersonID = 25,
                    FirstName = "Pietje",
                    LastName = "Puk",

                    // We moeten het wel zelf aan elkaar relateren, dat gebeurt (helaas) niet automatisch
                    Grades = grades.ToList()
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Person>>();
            mockSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Eenvoudiger setup mvb onze eigen extension method.
            var mockSetGrades = new Mock<DbSet<StudentGrade>>();
            mockSetGrades.SetupMock(grades);

            var mockContext = new Mock<SchoolEntities>();

            mockContext.Setup(m => m.People).Returns(mockSet.Object);
            mockContext.Setup(m => m.StudentGrades).Returns(mockSetGrades.Object);

            var query = from p in mockContext.Object.People.OfType<Student>().Include(s => s.Grades)
                        where p.FirstName == "Kim"
                        select p;

            Assert.IsFalse(query.Any());

            var pietje = mockContext.Object.People.FirstOrDefault(p => p.FirstName == "Pietje") as Student;
            Assert.IsNotNull(pietje);
            Assert.IsTrue(pietje.Grades.Any());
        }

        [TestMethod]
        public void MockingEntityFramework_ZonderMockingFramework()
        {
            var data = new List<Person>
            {
                new Student
                {
                    PersonID = 25,
                    FirstName = "Pietje",
                    LastName = "Puk",
                }
            };

            var context = new SchoolEntities();
            context.People = new DbSetMock<Person>(data);

            var query = from p in context.People
                        where p.FirstName == "Kim"
                        select p;

            Assert.IsFalse(query.Any());
            Assert.IsTrue(context.People.Any(p => p.FirstName == "Pietje"));
        }
    }
}
