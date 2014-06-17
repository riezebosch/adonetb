using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DatabaseFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SchoolEntities())
            {
                
                
                var query = from p in context.People.ToList()
                            where IsValid(p.FirstName)
                            select p.FirstName + " " + p.LastName;

                //Console.WriteLine(query);

                //foreach (var p in query)
                //{
                //    Console.WriteLine(p);
                //}

                foreach (var person in context
                    .People
                    .Include(p => 
                        p.StudentGrades.Select(g => g.Course)))
                {
                    Console.WriteLine(person.FirstName);

                    foreach (var grade in person.StudentGrades)
                    {
                        Console.WriteLine("  {1}: {0}", grade.Grade, grade.Course.Name);
                    }
                }
            }
        }

        static bool IsValid(string name)
        {
            var whitelist = new List<string> { "Peggy", "Jos", "Anton" };
            return whitelist.Contains(name);
        }
    }
}
