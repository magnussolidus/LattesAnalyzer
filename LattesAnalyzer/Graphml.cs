using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattesAnalyzer
{
    [Serializable]
    class Graphml
    {

        bool directedEdges;
        public List<Node> nodes;
        public List<Edge> edges;

        public Graphml()
        {
            directedEdges = false;
            nodes = new List<Node>();
            edges = new List<Edge>();
        }

        public void AddNode(Node n)
        {
            nodes.Add(n);
        }

        public void AddEdge(Node sourceNode, Node targetNode)
        {
            edges.Add(new Edge(sourceNode, targetNode));
        }

    }
}
