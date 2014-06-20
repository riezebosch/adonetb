using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseFirstDemo.ReverseEngineeredCodeFirst.Models;
using System.Linq;

namespace DatabaseFirstDemo.ReverseEngineeredCodeFirst
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var context = new SchoolContext())
            {
                Assert.IsTrue(
                    context.People.OfType<Instructor>().Any(p => p.FirstName == "Kim" && p.LastName == "Abercrombie"));
            }
        }

        [TestMethod]
        public void GivenListOfInstructors_ShouldNotContain_Student()
        {
            using (var context = new SchoolContext())
            {
                Assert.IsTrue(
                    context.People.OfType<Student>().Any(p => p.FirstName == "Gytis" && p.LastName == "Barzdukas"));
            }
        }
    }
}
