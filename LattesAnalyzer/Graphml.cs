using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LattesAnalyzer
{
    [Serializable]
    public class Graphml
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

        public void calCentralityIndexForEachNode()
        {
            int totalNodes = nodes.Count;

            foreach(Node calc in nodes)
            {
                calc.centralityIndex = this.edges.Count / (totalNodes - 1);
            }
        }

        public void export(string name)
        {
            try
            {
                if(!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"LattesAnalyzer\")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"LattesAnalyzer\"));
                }

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"LattesAnalyzer\" + name + ".graphml");
               
                using (Stream outputStream = File.Create(path))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(Graphml));
                    formatter.Serialize(outputStream, this);

                }             

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
