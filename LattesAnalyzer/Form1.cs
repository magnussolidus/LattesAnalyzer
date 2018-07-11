using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LattesAnalyzer
{
    public partial class Form1 : Form
    {
        public enum programState
        {
            iddle,
            checkingDirectory,
            checkingFiles,
            outputReady,
            error,
            finished
        }

        protected programState curStatus; // variável de controle sobre o estado do programa
        private List<Autor> autors = new List<Autor>(); // lista de usuários a serem processados
        private List<Artigo> articles = new List<Artigo>(); // lista de artigos a serem processados

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusLabel.Text += "Escolher diretório";
            initialHide();
            curStatus = programState.iddle;
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // display info about the program
        }

        private void abrirDiretórioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // opens up a modal window to select a directory
            DialogResult result = folderBrowserDialog1.ShowDialog();
            switch (result)
            {
                case DialogResult.Abort:
                case DialogResult.Cancel:
                case DialogResult.No:
                    //setIddleStatus();
                    break;
                case DialogResult.OK:
                case DialogResult.Yes:
                    curStatus = programState.checkingDirectory;
                    hideMenu();
                    checkDirectory();
                    break;
            }

        }

        private void initialHide()
        {
            filesLabel.Hide();
            cancel.Hide();
            reset.Hide();
        }

        private void hideMenu()
        {
            abrirDiretórioToolStripMenuItem.Available = false;
        }
        private void showMenu()
        {
            abrirDiretórioToolStripMenuItem.Available = true;
        }

        // verifica a quantidade de arquivos no caminho recebido e conta quantos são xml
        private uint countFiles(string path)
        {
            uint total = 0;
            try
            {
                var txtFiles = Directory.EnumerateFiles(path);
                foreach (string currentFile in txtFiles)
                {
                    string filename = currentFile.Substring(path.Length + 1);
                    if(filename.LastIndexOf(".xml") != -1)
                    {
                        total++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }   
            return total;
        }

        public void checkDirectory()
        {
            uint nFiles = 0;

            if (curStatus != programState.checkingDirectory)
            {
                return;
            }
            else
            {
                statusLabel.Text = "Situação: Verificando Diretório \'" + folderBrowserDialog1.SelectedPath + "\'";
                // método que verifica a quantidade de arquivos compatíveis no diretório
                nFiles = countFiles(folderBrowserDialog1.SelectedPath);

                if (nFiles > 0)
                {
                    curStatus = programState.checkingFiles;
                    checkFiles(nFiles);
                }
                else
                {
                    setErrorStatus("Nenhum arquivo compatível encontrado!");
                }
            }
        }

        public void checkFiles(uint ammount)
        {
            if(curStatus != programState.checkingFiles)
            {
                return;
            }
            else 
            { 
                statusLabel.Text = "Situação: Aguardando sua decisão";
                filesLabel.Text = "Foram encontrados " + ammount + " arquivos.";
                reset.Text = "Iniciar Análise";
                cancel.Text = "Cancelar Análise";
                filesLabel.Show();
                cancel.Show();
                reset.Show();
            }
        }

        private List<String> getValidPaths(string path)
        {
            List<String> result = new List<String>();
            try
            {
                var txtFiles = Directory.EnumerateFiles(path);
                foreach (string currentFile in txtFiles)
                {
                    string filename = currentFile.Substring(path.Length + 1);
                    if(filename.LastIndexOf(".xml") != -1)
                    {
                        result.Add(filename);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        private void gatherFromFile(string filename)
        {
            // video demonstrando um caso de uso -> https://www.youtube.com/watch?v=sfDPdflXbiM

            Autor test = new Autor();
            List<string> tempCitName = new List<string>();

            XDocument xdoc = XDocument.Load(filename);

            // id lattes do autor (sempre tem no currículo)
            xdoc.Descendants("CURRICULO-VITAE").Select( p => new { 
                id = p.Attribute("NUMERO-IDENTIFICADOR").Value
            }).ToList().ForEach(p => {
                //Console.WriteLine("ID: " + p.id);
                test.setId(p.id.ToCharArray(0, 16));
            });

            // nome do autor e nome em citação, nacionalidade é bonus
            xdoc.Descendants("DADOS-GERAIS").Select(p => new
            {
                name = p.Attribute("NOME-COMPLETO").Value,
                citName = p.Attribute("NOME-EM-CITACOES-BIBLIOGRAFICAS").Value,
                nat = p.Attribute("PAIS-DE-NACIONALIDADE").Value
            }).ToList().ForEach(p =>
            {
                //Console.Write("Nome: {0}\nCitação: {1}\nNatural de: {2}\n", p.name, p.citName, p.nat);
                test.setNome(p.name);
                test.setNomeCitacao(p.citName.Split(';').ToList());
                test.setNacionalidade(p.nat);

            });
            // Adiciona o usuário a lista de usuários a serem processados
            autors.Add(test);

            // ************* Artigos ************** \\
            // título artigo
            xdoc.Descendants("ARTIGO-PUBLICADO").Select(p => new
            {
                ta = p.Element("DADOS-BASICOS-DO-ARTIGO").Attribute("TITULO-DO-ARTIGO").Value,
                te = p.Element("DADOS-BASICOS-DO-ARTIGO").Attribute("ANO-DO-ARTIGO").Value,
                // Autores do Artigo
                ti = p.Descendants("AUTORES").Select(c => new
                {
                    name = c.Attribute("NOME-COMPLETO-DO-AUTOR").Value,
                    cit = c.Attribute("NOME-PARA-CITACAO").Value,
                    id = c.Attribute("NRO-ID-CNPQ").Value
                }).ToList()
            }).ToList().ForEach(p =>
            {
                // obtem o título e o ano do artigo
                //Console.Write("Título: {0}\nAno:{1}\n", p.ta, p.te);
                Artigo helper = new Artigo(p.ta, int.Parse(p.te));
                //helper.setTitulo(p.ta);
                //helper.setAno(int.Parse(p.te));
                p.ti.ForEach(z =>
                {
                    //Console.Write("Nome: {0}\nAutor: {1}\nID Lattes: {2}\n", z.name, z.cit, z.id);
                    tempCitName = z.cit.Split(';').ToList();
                    helper.addAutor(new Autor(z.name, tempCitName, z.id.ToCharArray()));
                    // obtem as informações de cada autor referente àquele artigo
                });

                articles.Add(helper); // adiciona o artigo à pilha de manuseio
                //helper.cleanAutors(); // limpa a lista de autores da variável auxiliar

            });
        }

        // método que lê os arquivos e gera a rede
        public void readFiles()
        {
            if(curStatus != programState.checkingFiles)
            {
                return;
            }
            else
            {
                // verifica e recebe o caminho dos arquivos que devem ser analisados
                List<String> paths = getValidPaths(folderBrowserDialog1.SelectedPath);
                
                // loop de leitura do arquivo
                foreach(string path in paths)
                {
                    gatherFromFile(folderBrowserDialog1.SelectedPath +"\\"+ path);
                }

                // iniciar o tratamento dos dados e criar a rede. 
                analyzeData();
                // TO DO
                // Aparentemente os artigos que se repetem já não são adicionados a pilha
                // Calcular o índice de centralidade para cada nó (se o normalizado ficar difícil usar sem normalização)
            }
        }

        public void analyzeData()
        {
            if(curStatus != programState.checkingFiles)
            {
                return;
            }

            int nodeId = 1;

            List<Node> nodes = new List<Node>();
            List<Edge> edges = new List<Edge>();

            foreach(Autor aut in autors)
            {
                //TODO: Verificar o que preencher em DATA
                nodes.Add(new Node(nodeId++, aut));

            }

            foreach (Artigo art in articles)
            {
                var distinctCombinations = art.getAutores()
                                              .SelectMany(x => art.getAutores(), (x, y) => Tuple.Create(x, y))  //Cria tuplas
                                              .Where(tuple => tuple.Item1.getNome() != tuple.Item2.getNome())   //Remove duplicatas de autor com ele mesmo
                                              .Distinct(new UnorderedTupleComparer<Autor>()).ToList()                   //Remove duplicatas de (No1,No2) e (No2,No1)
                                              ;

                List<Tuple<Autor,Autor>> list = distinctCombinations;

                foreach (Tuple<Autor,Autor> pair in distinctCombinations)
                {

                    Edge e = new Edge(nodes.Find(n => n.Data == pair.Item1), nodes.Find(n => n.Data == pair.Item2));


                }

            }

        }
        

        public void setIddleStatus()
        {
            statusLabel.Text = "Situação: Escolher Diretório";
            initialHide();
            this.showMenu();
            curStatus = programState.iddle;
        }

        public void setErrorStatus(string msg)
        {
            if(curStatus != programState.error)
            {
                statusLabel.Text = "Situação: " + msg;
                filesLabel.Show();
                reset.Text = "Reiniciar";
                reset.Show();
            }
            else
            {
                return;
            }
        }

        // método que controla a execução do programa
        private void programLoop(uint nFiles)
        {

            switch(curStatus)
            {
                case programState.iddle: 
                    statusLabel.Text = "Situação: Escolher Diretório";
                    filesLabel.Hide();
                    cancel.Hide();
                    break;

                case programState.checkingDirectory:
                    statusLabel.Text = "Situação: Verificando Diretório \'" + folderBrowserDialog1.SelectedPath + "\'";
                    // método que verifica a quantidade de arquivos compatíveis no diretório
                    nFiles = countFiles(folderBrowserDialog1.SelectedPath);
                    
                    if(nFiles > 0)
                    {
                        curStatus = programState.checkingFiles;
                        programLoop(nFiles);
                    }
                    else
                    {
                        curStatus = programState.error;
                        programLoop(nFiles);
                    }
                    break;

                case programState.checkingFiles:
                    statusLabel.Text = "Situação: Analisando arquivos, por favor aguarde.";
                    filesLabel.Text = "Foram encontrados " + nFiles + " arquivos.";
                    cancel.Text = "Cancelar Análise";
                    filesLabel.Show();
                    cancel.Show();
                    break;

                case programState.outputReady:
                    statusLabel.Text = "Situação: Análise concluída!";
                    filesLabel.Text = "Foram analisados " + nFiles + " arquivos.";
                    // exibir tabela com o resultado do índice individual
                    break;

                case programState.error:
                    switch(nFiles)
                    {
                        case 0:
                            statusLabel.Text = "Situação: Não foram encontrados arquivos compatíveis no local especificado.";
                            filesLabel.Text = "Nenhum arquivo compatível";
                            filesLabel.Show();
                            break;
                        default:
                            statusLabel.Text = "Situação: Ocorreu um erro inesperado!";
                            filesLabel.Hide();
                            break;
                    }
                    break;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            setIddleStatus();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            if(curStatus == programState.checkingFiles)
            {
                this.readFiles();
            }
            else if(curStatus == programState.error || curStatus == programState.checkingDirectory)
            {
                setIddleStatus();
            }
        }

    }
}
