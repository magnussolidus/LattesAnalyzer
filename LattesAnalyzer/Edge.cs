using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LattesAnalyzer
{
    [Serializable]
    public class edge
    {
        [XmlIgnore]
        bool directed { get; set; }
        [XmlAttribute]
        public int source { get; set; }
        [XmlAttribute]
        public int target { get; set; }

        public edge() { }

        public edge(int sourceNode, int targetNode)
        {
            directed = false;
            source = sourceNode;
            target = targetNode;
        }

    }
}
