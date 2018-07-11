using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LattesAnalyzer
{
    [Serializable]
    public class node
    {
        public Autor data { get; set; }
        [XmlAttribute]
        public int id { get; set; }
        [XmlElement]
        public float centralityIndex { get; set; }

        public node() { }

        public node(int id, Autor Data)
        {
            this.id = id;
            this.data = Data;
            this.centralityIndex = 0.0f;
        }

    }
}
