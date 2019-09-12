using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Estoque.Models
{
    public class Empresa
    {
        //Váriaveis da Empresa
        public Int32 CodigoEmpresa { get; set; }
        public String NomeEmpresa { get; set; }
        public string Fone { get; set; }
        public String Contato { get; set; }

        /******************************************************************** CADASTRAR EMPRESA ********************************************************************/
        public Boolean CadastrarEmpresa()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Empresa (NomeEmpresa, Fone, Contato) VALUES (@NomeEmpresa, @Fone, @Contato);";
            Comando.Parameters.AddWithValue("@NomeEmpresa", this.NomeEmpresa);
            Comando.Parameters.AddWithValue("@Fone", this.Fone);
            Comando.Parameters.AddWithValue("@Contato", this.Contato);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** LISTAR EMPRESAS ********************************************************************/
        public List<Empresa> ListarEmpresas()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM Empresa;";

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Empresa> ListaEmpresa = new List<Empresa>();

            while (Leitor.Read())
            {
                Empresa E = new Empresa();

                E.CodigoEmpresa = Convert.ToInt32(Leitor["CodigoEmpresa"].ToString());
                E.NomeEmpresa = Leitor["NomeEmpresa"].ToString();
                E.Contato = Leitor["Contato"].ToString();
                E.Fone = Leitor["Fone"].ToString();
                ListaEmpresa.Add(E);
            }
            return ListaEmpresa;
        }

        /******************************************************************** LISTAR EMPRESA ESPECIFICA ********************************************************************/
        public List<Empresa> ListarEmpresa(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM Empresa WHERE CodigoEmpresa = @CodigoEmpresa;";
            Comando.Parameters.AddWithValue("@CodigoEmpresa", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Empresa> ListaEmpresa = new List<Empresa>();

            while (Leitor.Read())
            {
                Empresa E = new Empresa();

                E.CodigoEmpresa = Convert.ToInt32(Leitor["CodigoEmpresa"].ToString());
                E.NomeEmpresa = Leitor["NomeEmpresa"].ToString();
                E.Contato = Leitor["Contato"].ToString();
                E.Fone = Leitor["Fone"].ToString();
                ListaEmpresa.Add(E);
            }
            return ListaEmpresa;
        }

        /******************************************************************** ALTERAR EMPRESA ********************************************************************/
        public Boolean AlterarEmpresa(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Empresa SET NomeEmpresa = @NomeEmpresa, Fone = @Fone, Contato = @Contato WHERE CodigoEmpresa = @CodigoEmpresa;";
            Comando.Parameters.AddWithValue("@NomeEmpresa", this.NomeEmpresa);
            Comando.Parameters.AddWithValue("@Fone", this.Fone);
            Comando.Parameters.AddWithValue("@Contato", this.Contato);
            Comando.Parameters.AddWithValue("@CodigoEmpresa", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }
    }
}