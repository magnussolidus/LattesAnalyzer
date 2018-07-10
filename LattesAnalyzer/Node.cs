using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattesAnalyzer
{
    class Node
    {
        object Data { get; set; }
        int id { get; set; }

        public Node(int id, object Data)
        {
            this.id = id;
            this.Data = Data;
        }

    }
}
