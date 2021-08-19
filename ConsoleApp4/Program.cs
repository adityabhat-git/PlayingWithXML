using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            string XmlPath = AppDomain.CurrentDomain.BaseDirectory + @"XSLT/Request.xml";
            bool isAvailable = true;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlPath);
            XDocument xDoc = XDocument.Parse(xmlDoc.InnerXml);
            List<XElement> toRemove = new List<XElement>(); // List for Removing Elements from PrevPackages
            List<XElement> xElement = new List<XElement>(); // List for Adding Elements to Packages
            //Bring in all the Packages under PrevPackages Node - in this case it would give 2 elements
            var PreviousPackages = xDoc.Element("RootElement").Element("PrevPackages").Elements().ToList();
            //Bring in all the Packages under Packages Node - this is case it would give 4 elements
            var Packages = xDoc.Element("RootElement").Element("Packages").Elements().ToList();
            //loop in thru all the PrevPackages and check if any of the element having an element H_PackageType matches within the Packages List
            //if it matches , then pick up all the elements of the PrevPackage element and store it in another variable
            //These stored elements would be the ones which we need to add to the Packages
            foreach (var element in PreviousPackages)
            {
                isAvailable = Packages.Any(x => x.Value.Equals(element.Element("H_PackageType").Value));
                if (isAvailable)
                {
                    xElement.AddRange(element.Elements());
                    toRemove.Add(element);
                }
            }

            //Add the stored Elements to Packages Node
            foreach (var element in xElement)
            {
                xDoc.Element("RootElement").Element("Packages").Add(element);
            }

            //Remove the elements from PrevPackages
            foreach (var element in toRemove)
            {
                xDoc.Element("RootElement").Element("PrevPackages").Elements().ToList().ForEach(el =>
                {
                    if (el.Equals(element))
                    {
                        el.Remove();
                    }
                });
            }

            //Save the xml again on the same path
            xDoc.Save(XmlPath);

            Console.WriteLine("Hello World!");
        }
    }
}
