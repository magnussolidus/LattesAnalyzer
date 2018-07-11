using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattesAnalyzer
{
    [Serializable]
    public class Node
    {
        public object Data { get; set; }
        public int id { get; set; }
        public float centralityIndex { get; set; }

        public Node(int id, object Data)
        {
            this.id = id;
            this.Data = Data;
            this.centralityIndex = 0.0f;
        }

    }
}
