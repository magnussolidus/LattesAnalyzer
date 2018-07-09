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

        protected programState curStatus;

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
            hideMenu();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            switch (result)
            {
                case DialogResult.Abort:
                case DialogResult.Cancel:
                case DialogResult.No:
                    setIddleStatus();
                    break;
                case DialogResult.OK:
                case DialogResult.Yes:
                    curStatus = programState.checkingDirectory;
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
                // método para verificar os arquivos
                //this.readFiles();

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

        private void TestDocument(string filename, Type objType)
        {
            // https://www.youtube.com/watch?v=sfDPdflXbiM

            Autor test = new Autor();
            Artigo helper = new Artigo();
            List<Artigo> artigos = new List<Artigo>();
            XDocument xdoc = XDocument.Load(filename);

            // id lattes do autor
            xdoc.Descendants("CURRICULO-VITAE").Select( p => new { 
                id = p.Attribute("NUMERO-IDENTIFICADOR").Value
            }).ToList().ForEach(p => {
               // Console.WriteLine("ID: " + p.id);
                test.setId(p.id.ToCharArray(0, 16));
                //test.showId();
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

            });

            // ************* Artigos ************** \\
            // título artigo
            xdoc.Descendants("ARTIGO-PUBLICADO").Select(p => new
            {
                te = p.Element("DADOS-BASICOS-DO-ARTIGO").Attribute("TITULO-DO-ARTIGO").Value,
                // Autores do Artigo
                ti = p.Descendants("AUTORES").Select(c => new
                {
                    name = c.Attribute("NOME-COMPLETO-DO-AUTOR").Value,
                    cit = c.Attribute("NOME-PARA-CITACAO").Value,
                    id = c.Attribute("NRO-ID-CNPQ").Value
                }).ToList()
                //at1 = p.Element("AUTORES").Attribute("NOME-COMPLETO-DO-AUTOR").Value,
                //cit = p.Element("AUTORES").Attribute("NOME-PARA-CITACAO").Value,
                //id = p.Element("AUTORES").Attribute("NRO-ID-CNPQ").Value
                //name = p.Attribute("TITULO-DO-ARTIGO").Value
            }).ToList().ForEach(p =>
            {
                Console.Write("Título: {0}\n", p.te);
                // obtem o título do artigo
                p.ti.ForEach(z =>
                {
                    Console.Write("Nome: {0}\nAutor: {1}\nID Lattes: {2}\n", z.name, z.cit, z.id);
                });
                //helper.setTitulo(p.name);

            });
            // autores artigo
            /*
            xdoc.Descendants("AUTORES").Select(p => new
            {
                name = p.Attribute("NOME-COMPLETO-DO-AUTOR").Value,
                cit = p.Attribute("NOME-PARA-CITACAO").Value,
                id = p.Attribute("NRO-ID-CNPQ").Value
            }).ToList().ForEach(p =>
            {
                //Console.Write("Nome: {0}\nAutor: {1}\nId Autor: {2}\n", p.name, p.cit, p.id);

            });

            // artigos
            /*
            xdoc.DescendantNodes();
            xdoc.Ancestors();
            xdoc.Descendants("ARTIGOS-PUBLICADOS").Select(p => new
            {
                desc = p.Descendants("DADOS-BASICOS-DO-ARTIGO").Select(t => new {
                    title = t.Attribute("TITULO-DO-ARTIGO").Value
                }).ToList().ForEach(t => {
                    Console.Write("Título: {0}\n", t.title);
                })
            }).ToList().ForEach(p =>
            {
                xdoc.DescendantNodes().ToList().ForEach(d =>
                    {
                        Console.Write("Desc: {0}", d.ToString());
                    });
                xdoc.Ancestors();
                Console.Write("Título: {0}\n",
                    p.title);
            });
            */
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

                    TestDocument(folderBrowserDialog1.SelectedPath +"\\"+ path, typeof(Autor)); // testando 
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
