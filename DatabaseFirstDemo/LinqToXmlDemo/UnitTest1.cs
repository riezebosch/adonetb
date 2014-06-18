using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace LinqToXmlDemo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var doc = XDocument.Load("Books.xml");

            var recent = from book in doc.Element("catalog").Elements("book")
                         where (DateTime)book.Element("publish_date") > new DateTime(2000, 12, 31)
                         select book.Element("title").Value;

            foreach (var book in recent)
            {
                Console.WriteLine(book);
            }
        }

        [TestMethod]
        public void TelAlleBedragenInDeBooksXmlBijElkaarOp()
        {
            var doc = XDocument.Load("Books2.xml");
            XNamespace ns = "HIERSTAATMIJNNAMESPACE_DITDOCUMENT_ISUNIEK";

            var totaal = doc
                .Element(ns + "catalog")
                .Elements(ns + "book")
                .Sum(b => (decimal?)b.Element(ns + "price") ?? 0);
            Console.WriteLine(totaal);
        }

        [TestMethod]
        public void CreateXmlFromCode()
        {
            XNamespace ns = "urn:www-infosupport-com:adonetb:linq-to-xml";
            XNamespace other = "child-items";

            var doc = new XDocument(
                new XElement(ns + "Root", 
                    new XElement(ns + "Item1"),
                    new XElement(ns + "Item2", 
                        new XAttribute("some", 3)),
                    new XElement(other + "Item3", 
                        new XElement(other + "Item3.1"),
                        new XElement(other + "Item3.2"))
                ));

            Console.WriteLine(doc);
        }

        [TestMethod]
        public void AddAdditionalBook()
        {
            var doc = XDocument.Load("Books.xml");
            doc.Element("catalog").Add(
                new XElement("book", 
                    new XElement("author", "Pietje Puk"),
                    new XElement("title", "Pietje Puk wordt agent"),
                    new XElement("genre", "Children"),
                    new XElement("publish_date", DateTime.Today)));

            doc.Save("Books3.xml");
        }
    }
}
