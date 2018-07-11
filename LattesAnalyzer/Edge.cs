using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattesAnalyzer
{
    class Edge
    {
        bool directed { get; set; }
        public Node source { get; set; }
        public Node target { get; set; }

        public Edge(Node sourceNode, Node targetNode)
        {
            directed = false;
            source = sourceNode;
            target = targetNode;
        }

    }
}
