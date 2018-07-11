using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LattesAnalyzer
{
    [Serializable]
    public class Edge
    {
        bool directed { get; set; }
        [XmlAttribute]
        public Node source { get; set; }
        [XmlAttribute]
        public Node target { get; set; }

        public Edge() { }

        public Edge(Node sourceNode, Node targetNode)
        {
            directed = false;
            source = sourceNode;
            target = targetNode;
        }

    }
}
