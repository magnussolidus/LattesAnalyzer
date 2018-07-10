using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattesAnalyzer
{
    class Artigo
    {
        string titulo;
        int ano;
        List<Autor> autores;

        public Artigo()
        {
            titulo = "";
            ano = 3000;
            autores = new List<Autor>();
        }

        public Artigo(string title)
        {
            titulo = title;
            ano = 3000;
            autores = new List<Autor>();
        }

        public Artigo(string title, int year)
        {
            titulo = title;
            ano = year;
            autores = new List<Autor>();
        }

        public Artigo(string title, int year, List<Autor> authors)
        {
            this.titulo = title;
            this.ano = year;
            this.autores = authors;
        }

        public Artigo(Artigo source)
        {
            this.titulo = source.titulo;
            this.ano = source.ano;
            this.autores = source.autores;
        }

        public void setTitulo(string value)
        {
            this.titulo = value;
        }

        public string getTitulo()
        {
            return this.titulo;
        }

        public void setAno(int value)
        {
            this.ano = value;
        }

        public int getAno()
        {
            return this.ano;
        }

        public void setAutores(List<Autor> value)
        {
            this.autores = value;
        }

        public List<Autor> getAutores()
        {
            return this.autores;
        }

        public List<Autor> getAutoresAsNew()
        {
            List<Autor> temp = new List<Autor>();
            temp = this.autores;
            return temp;
        }

        public void addAutor(Autor novo)
        {
            this.autores.Add(novo);
        }

        public void cleanAutors()
        {
            this.autores.Clear();
        }

        public void Reset()
        {
            this.autores.Clear();
            this.ano = 0;
            this.titulo = "";
        }

    }
}
