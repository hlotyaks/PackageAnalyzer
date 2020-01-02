using System;
using System.Collections.Generic;

namespace PackageAnalyzer
{
    public class Graph
    {
        private Dictionary<string, List<string>> nodes;

        public Graph()
        {
            nodes = new Dictionary<string, List<string>>();
        }

        public int NodeCount
        {
            get { return nodes.Keys.Count; }
        }

        public List<string> Edges(string node)
        {
            return nodes[node];
        }

        public void AddEdge(string V)
        {
            if (nodes.ContainsKey(V))
            {
                return;
            }

            nodes.Add(V, new List<string>());
        }

        public void AddEdge(string V, string E)
        {
            if (nodes.ContainsKey(V))
            {
                nodes[V].Add(E);
            }
            else
            {  
                nodes.Add(V, new List<string>());
                nodes[V].Add(E);
            }

        }

        private bool IsCyclicInternal(string V, Dictionary<string, bool> visited, Dictionary<string, bool> recursive)
        {
            if (!visited[V])
            {
                visited[V] = true;
                recursive[V] = true;

                foreach (string E in nodes[V])
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
                foreach (string V in nodes.Keys)
                {
                    visited.Add(V,false);
                    recursive.Add(V,false);
                }

                foreach (string V in nodes.Keys)
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
