using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace PackageAnalyzer
{
    public class GraphBuilder
    {
        private Graph graph;
        public GraphBuilder()
        {
            graph = new Graph();
        }

        // path - path to folder containing description files
        public GraphBuilder(string path) : this()
        { 
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo fi in di.EnumerateFiles())
            {
                Insert(fi);
            }
        }

        public Graph Graph
        {
            get { return graph; }
        }

        public void Insert(FileInfo fileInfo)
        {
            XDocument xmlDoc= XDocument.Load(fileInfo.FullName);
            XElement root = xmlDoc.Root;
            XNamespace ns = root.GetDefaultNamespace();
            string packageID;
            IEnumerable<string> depPackageIDs = new List<string>();

            try
            {
                // get the ID of package
                XElement IDelement = (from c in xmlDoc.Descendants(ns + "package") select c).First();
                packageID = IDelement.Attribute("id").Value;

            }
            catch (Exception e)
            {
                throw new GraphBuilderException("Malformed package description file.", e);
            }

            // get dependency section
            IEnumerable<XElement> Depelements = from c in xmlDoc.Descendants(ns + "dependency") select c;
            if(Depelements.Count() == 1)
            {
                try
                {
                    // get dependent packages
                    IEnumerable<XAttribute> DepIDelement = from c in Depelements.First().Descendants(ns + "package").Attributes() select c;
                    depPackageIDs = DepIDelement.Select(t => t.Value);
                }
                catch (Exception e)
                {
                    throw new GraphBuilderException("Malformed dependency tag in description file.", e);
                }
            }

            if(depPackageIDs.Count() != 0)
            {
                foreach (string dependentPackage in depPackageIDs)
                {
                    graph.AddEdge(packageID, dependentPackage);
                }
            }
            else
            {
                graph.AddEdge(packageID);
            }
        }

    }
}
