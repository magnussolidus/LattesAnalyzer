using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LattesAnalyzer
{
    [Serializable]
    public class graphml
    {
        [XmlAttribute]
        bool directed;
        [XmlArray]
        public List<node> nodes;
        [XmlArray]
        public List<edge> edges;

        public graphml()
        {
            directed = false;
            nodes = new List<node>();
            edges = new List<edge>();
        }

        public void AddNode(node n)
        {
            nodes.Add(n);
        }

        public void AddEdge(node sourceNode, node targetNode)
        {
            edges.Add(new edge(sourceNode.id, targetNode.id));
        }

        public void calCentralityIndexForEachNode(bool directed)
        {
            if(!directed)
            {
                int totalNodes = nodes.Count;
                int tempNodeEdges;
                float tempIndex = 0.0f;

                foreach (node calc in nodes)
                {
                    tempNodeEdges = 0;
                    foreach(edge e in edges)
                    {
                        if(e.source == calc.id || e.target == calc.id)
                        {
                            tempNodeEdges++; 
                        }
                    }
                    tempIndex = (float) tempNodeEdges / (float) (totalNodes - 1);
                    calc.centralityIndex = tempIndex;
                }
            }
        }

        public void export(string name)
        {
            try
            {
                /*if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"LattesAnalyzer\")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"LattesAnalyzer\"));
                }*/

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); // adiciona o namespace correto
                ns.Add("GraphML", "http://graphml.graphdrawing.org/xmlns/1.0rc");

                
                using (Stream outputStream = File.Create(name))
                {

                    var knownTypes = new Type[] { typeof(Autor) };
                    XmlSerializer formatter = new XmlSerializer(typeof(graphml), knownTypes);
                    XName XNgraph = XName.Get("graph", "http://graphml.graphdrawing.org/xmlns/1.0rc");
                    formatter.Serialize(outputStream, this, ns);
                    outputStream.Dispose();
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
