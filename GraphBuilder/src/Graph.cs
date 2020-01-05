using System;
using System.Collections.Generic;

namespace PackageAnalyzer
{
    public class Graph
    {
        private Dictionary<string, List<string>> directednodes;
        private Dictionary<string, List<string>> parentnodes;
        public Graph()
        {
            directednodes = new Dictionary<string, List<string>>();
            parentnodes = new Dictionary<string, List<string>>();
        }

        public List<string> Paths(string V)
        {
            return directednodes[V];
        }

        public int NodeCount
        {
            get { return directednodes.Keys.Count; }
        }

        public List<string> Children(string node)
        {
            return parentnodes[node];
        }

        public void AddEdge(string V)
        {
            if (!directednodes.ContainsKey(V))
            {
                directednodes.Add(V, new List<string>());
            }

            if (!parentnodes.ContainsKey(V))
            {
                parentnodes.Add(V, new List<string>());
            }
            
        }

        public void AddEdge(string V, string E)
        {
            // Add vertex and edge information
            // Example:
            //      A <= B
            //      B is the vertex and A is the edge between B and A
            //      A is also the parent of B
            if (directednodes.ContainsKey(V))
            {
                directednodes[V].Add(E);
            }
            else
            {  
                directednodes.Add(V, new List<string>());
                directednodes[V].Add(E);
            }

            // If this vertex has not been seen yet also store it as a possible parent
            if(!parentnodes.ContainsKey(V))
            {
                parentnodes.Add(V, new List<string>());
            }

            // Add the edge and vertex parent information
            if (parentnodes.ContainsKey(E))
            {
                parentnodes[E].Add(V);
            } 
            else
            {
                parentnodes.Add(E, new List<string>());
                parentnodes[E].Add(V);
            }
        }

        private bool IsCyclicInternal(string V, Dictionary<string, bool> visited, Dictionary<string, bool> recursive)
        {
            if (!visited[V])
            {
                visited[V] = true;
                recursive[V] = true;

                foreach (string E in directednodes[V])
                {
                    if (!visited[E] && IsCyclicInternal(E, visited, recursive))
                    {
                        return true;
                    }
                    else if (recursive[E])
                    {
                        return true;
                    }
                }
            }

            recursive[V] = false;

            return false;
        }

        public bool IsCyclic
        {
            get
            {

                Dictionary<string, bool> visited = new Dictionary<string, bool>();
                Dictionary<string, bool> recursive = new Dictionary<string, bool>();

                // initialize the search...
                foreach (string V in directednodes.Keys)
                {
                    visited.Add(V,false);
                    recursive.Add(V,false);
                }

                foreach (string V in directednodes.Keys)
                {
                    if (IsCyclicInternal(V, visited, recursive))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
