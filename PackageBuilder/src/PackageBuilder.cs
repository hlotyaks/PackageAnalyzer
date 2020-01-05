using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;

namespace PackageAnalyzer
{
    public class PackageBuilder
    {
        // location relative to specified rootFolder.
        const string PACKAGE_DESCRIPTIONS = @"\packages\descriptions\";
        const string PACKAGE_CACHE = @"\packages\cache\";
        const string PACKAGE_SOURCE = @"\src\";
        const string PACKAGE_OUTPUT = @"\output\";
        const string PACKAGE_EXTENSION = ".zip";
        string _cacheRoot;
        string _sourceRoot;
        string _outputRoot;
        string _packageRoot;

        List<string> _cacheContents;
        public PackageBuilder(string rootFolder)
        {
            _cacheRoot = $"{rootFolder}{PACKAGE_CACHE}";
            _sourceRoot = $"{rootFolder}{PACKAGE_SOURCE}";
            _outputRoot = $"{rootFolder}{PACKAGE_OUTPUT}";
            _packageRoot = $"{rootFolder}{PACKAGE_DESCRIPTIONS}";

            // create output dir if not exist.
            if(!Directory.Exists(_outputRoot))
            {
                Directory.CreateDirectory(_outputRoot);
            }

            // initialize cache contents
            DirectoryInfo di = new DirectoryInfo(_cacheRoot);
            _cacheContents = di.GetFiles().Select(f => f.Name.Substring(0,f.Name.Length - PACKAGE_EXTENSION.Length)).ToList();
        }

        public Dictionary<string, bool> Build(string startNode, string sourceFolder, Dictionary<string, string> hash, Graph g)
        {
            List<string> packages = new List<string>();
            packages.Add(startNode);
            
            Dictionary<string, bool> packageBuildResults = new Dictionary<string, bool>();

            BuildPackages(packages, hash, g, packageBuildResults);

            return packageBuildResults;
        }

        private void BuildPackages(List<string> packages, Dictionary<string, string> hash, Graph g, Dictionary<string, bool> buildResults)
        {
            if (packages.Count == 0)
            {
                return;
            }

            List<string> children = new List<string>();

            // if the parent built then this needs to build
            // if the hash version doesn't exist in cache the this needs to build
            // if the hash version exists in cache then copy to output
            
            bool forceBuild;

            foreach (string package in packages)
            {
                bool packagebuilt;
                buildResults.TryGetValue(package, out packagebuilt);
                if (packagebuilt)
                {
                    continue;
                }

                forceBuild = false;

                // if the packages dependency was built then this package will beuilt too.
                foreach (string k in buildResults.Keys)
                {
                    if (g.Paths(package).Contains(k) && buildResults[k])
                    {
                        forceBuild = true;
                    }
                }

                buildResults.Add(package, BuildPackage(package, hash, forceBuild));

                children.AddRange(g.Children(package));
            }

            BuildPackages(children, hash, g, buildResults); 
        }

        private bool BuildPackage(string package, Dictionary<string, string> hash, bool force)
        {
            // in this context all building means is getting a zip version of the package with hash extention to the output folder.
            // this can happen 1 of 2 ways.  1) copy the existing one from cache or 2) zip the src add a new one to cache and output

            string packageHashName = $"{hash[package]}";
            if (!PackageExists(packageHashName) || force)
            {
                Compile(package, packageHashName);
                CopyOutputToCache(packageHashName);
                return true; // true because package did 'build'
            }
            else
            {
                CopyCacheToOuput(packageHashName);
                return false; // false because package didn't 'build'
            }

        }

        private bool PackageExists(string packageHashName)
        {
            return _cacheContents.Contains(packageHashName);
        }

        private void CopyCacheToOuput(string packageHashName)
        {
            File.Copy($"{_cacheRoot}{packageHashName}.zip", $"{_outputRoot}{packageHashName}.zip", true);
        }

        private void CopyOutputToCache(string packageHashName)
        {
            File.Copy($"{_outputRoot}{packageHashName}.zip", $"{_cacheRoot}{packageHashName}.zip", true);
        }

        private void Compile(string package, string packageHashName)
        {
            string source = $"{_sourceRoot}{package}";
            string dest = $"{_outputRoot}{packageHashName}.zip";

            ZipFile.CreateFromDirectory($"{_sourceRoot}{package}", $"{_outputRoot}{packageHashName}.zip");
        }
    }
}
