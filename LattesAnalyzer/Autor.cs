using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattesAnalyzer
{
    public class Autor
    {
        string nome, nacionalidade; 
        int nArtigos;
        char[] identificador;
        List<String> citNome;
        
        public Autor()
        {
            nome = "";
            nacionalidade = "Desconhecida";
            nArtigos = 0;
            identificador = new char[16];
            citNome = new List<String>();
        }

        public Autor(string name, List<string> citationName)
        {
            nome = name;
            nacionalidade = "Desconhecida";
            nArtigos = 0;
            identificador = new char[16];
            citNome = citationName;
        }

        public Autor(string name, List<string> citationName, char[] id)
        {
            nome = name;
            nacionalidade = "Desconhecida";
            nArtigos = 0;
            identificador = id;
            citNome = citationName;
        }

        public void setNome(string value)
        {
            this.nome = value;
        }

        public string getNome()
        {
            return this.nome;
        }

        public void setNacionalidade(string value)
        {
            this.nacionalidade = value;
        }

        public string getNacionalidade()
        {
            return this.nacionalidade;
        }

        public void setArtigos(int value)
        {
            this.nArtigos = value;
        }

        public int getArtigos()
        {
            return this.nArtigos;
        }

        public char[] getId()
        {
            return this.identificador;
        }

        public void setId(char[] value)
        {
            if(value.Length == 16)
                this.identificador = value;
        }

        public void setNomeCitacao(List<string> value)
        {
            this.citNome = value;
        }

        public List<string> getNomeCitacao()
        {
            return this.citNome;
        }

        public void showId()
        {
            Console.WriteLine(this.identificador);
        }

        public bool comparaNomeCitacao(string source)
        {
            foreach(string cit in this.citNome)
            {
                if(source.CompareTo(cit) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool comparaAutor(Autor a, Autor b)
        {
            if(a.identificador == b.identificador)
            {
                return true;
            }
            else if (a.nome == b.nome)
            {
                return true;
            }
            else
            {
                foreach(string nameA in a.citNome)
                {
                    foreach(string nameB in b.citNome)
                    {
                        if(nameA.CompareTo(nameB) == 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }



    }
}
