using System;
using System.Collections.Generic;

namespace PackageAnalyzer
{
    public class PackageBuilder
    {
        // location relative to specified rootFolder.
        const string PACKAGE_DESCRIPTIONS = "//packages//descriptions";
        const string PACKAGE_CACHE = "//packages//cache";
        const string PACKAGE_SOURCE = "//src";
        const string PACKAGE_OUTPUT = "//output";

        

        public PackageBuilder()
        {
        }


        public List<string> Build(string startNode, string rootFolder)
        {
            List<string> builtPackages = new List<string>();

            GraphBuilder gb = new GraphBuilder($"{rootFolder}{PACKAGE_DESCRIPTIONS}");

            if (gb.Graph.IsCyclic)
            {
                // need logging. throw cycle exception
                throw new Exceptions.PackageBuilderCycleException("Cycle detected in package graph.");
            }

            return builtPackages;
        }
    }
}
