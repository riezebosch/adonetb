using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace LinqDemo
{
    delegate int MyOwnDelegate(string input);
    delegate TOut MyOwnDelegate<TIn, TOut>(TIn input);
    delegate TOut MyOwnDelegate<T1, T2, TOut>(T1 t1, T2 t2);

    static class MyStringExtenions
    {
        public static string RemoveVowels(this string input)
        {
            char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
            var result = new StringBuilder();

            foreach (var c in input)
            {
                if (!vowels.Contains(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }

    [TestClass]
    public class UnitTest1
    {
        

        [TestMethod]
        public void OuderwetseVolledigUitgeschrevenDelegateDemo()
        {
            MyOwnDelegate d1 = new MyOwnDelegate(DoeIets);
            var d2 = new MyOwnDelegate(DoeIets);
            MyOwnDelegate d3 = DoeIets;

            // Dit mag niet, hier komt de compiler niet meer uit
            //var d4 = DoeIets;

            d3.Invoke("Input nogiets");
            d3("Input maar dan korter");

            // Naar andere methode wijzen obv zelfde signatuur
            d3 = DoeIets2;

            DoeIetsMetDelegate(d3);

            var d4 = new MyOwnDelegate<string, int>(DoeIets);
            var d5 = new Func<string, int>(DoeIets);

            //new [] { 1, 2, 3, 5,5645, 45,34, 4}.Where()
        }

        private int DoeIets(string input)
        {
            throw new NotImplementedException();
        }

        private int DoeIets2(string input)
        {
            return 3;
        }

        private static void DoeIetsMetDelegate(MyOwnDelegate d)
        {
            if (d("waarde") != null)
            {
                Console.WriteLine("Iets spannends obv output van delegate");
            }
        }

        [TestMethod]
        public void AnonymouMethods()
        {
            int i = 5;
            DoeIetsMetDelegate(delegate(string input) { return i++; });

            Console.WriteLine(i);
        }

        [TestMethod]
        public void EindelijkMetLambdas()
        {
            int i = 5;
            DoeIetsMetDelegate(input => i++);
            DoeIetsMetDelegate((string input) => i++);
            DoeIetsMetDelegate((string input) => { return i++; });

            Console.WriteLine(i);
        }

        [TestMethod]
        public void ExtensionMethods()
        {
            int[] items = { 1, 2, 43, 3434, 234 };
            var result = items
                .Where(i => i % 2 == 0)
                .Select(i => i * 3);

            var result2 = from i in items
                          where i % 2 == 0
                          select i * 3;


            var result3 = Enumerable.Select(Enumerable.Where(items, i => i % 2 == 0), i => i * 3);

            Console.WriteLine(string.Join(", ", result));
        }

        [TestMethod]
        public void ExtensionmethodDemo()
        {
            string input = "Data Access Technologies using ADO.NET";
            var result = MyStringExtenions.RemoveVowels(input);

            input.RemoveVowels();

            Console.WriteLine(result);
        }

        [TestMethod]
        public void ExpressionsDemo()
        {
            DoeIetsMetEenExpression(m => m % 2 == 0);
        }

        private void DoeIetsMetEenExpression(Expression<Func<int, bool>> f)
        {
            f.Compile()(4);
        }


        [TestMethod]
        public void ExpressionWhere()
        {
            var items = new List<Persoon>
            {
                new Persoon { Name = "Jan" } 
            };

            var result = items.Where(p => p.Name == "Jan");
        }


        class Persoon
        {
            public string Name { get; set; }
        }

        [TestMethod]
        public void DeferredExecution()
        {
            var items = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            var query = from i in items
                        where i % 2 == 0
                        select i;

            items.Add(4);
            Console.WriteLine(string.Join(", ", query));

            query = from i in query
                    select i * 3;

            query = items.Where(i => i % 2 == 0).Select(i => i).Select(i => i * 3);

            items.Add(4);
            Console.WriteLine(string.Join(", ", query));
        }
    }
}
