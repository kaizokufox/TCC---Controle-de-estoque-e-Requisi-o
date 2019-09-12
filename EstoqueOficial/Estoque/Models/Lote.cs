
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Estoque.Models
{
    public class Lote
    {
        public Int32 CodigoLote { get; set; }
        public String NomeLote { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime DataValidade { get; set; }
        public Int32 Quantidadeitens { get; set; }
        public Int32 Contador { get; set; }
        public Int32 inserido { get; set; }

        public Lote() { }

        /******************************************************************** CONSTRUTORES LOTE ********************************************************************/
        public Lote(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM Lote WHERE CodigoLote = @CodigoLote;";
            Comando.Parameters.AddWithValue("@CodigoLote", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.CodigoLote = Convert.ToInt32(Leitor["CodigoLote"].ToString());
            this.NomeLote = Leitor["NomeLote"].ToString();
            this.DataEntrada = Convert.ToDateTime(Leitor["DataEntrada"].ToString());
            this.DataValidade = Convert.ToDateTime(Leitor["DataValidade"].ToString());
            this.Quantidadeitens = Convert.ToInt32(Leitor["QtdItens"].ToString());

            Conexao.Close();
        }

        /******************************************************************** SELECIONA LOTE ********************************************************************/
        public void SelecionaLote(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT CodigoLote from Lote WHERE NomeLote = @NomeLote;";
            Comando.Parameters.AddWithValue("@NomeLote", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.CodigoLote = Convert.ToInt32(Leitor["CodigoLote"].ToString());

            Leitor.Close();
            Conexao.Close();
        }

        /******************************************************************** VERIFICA LOTE ********************************************************************/
        public void VerificaLote(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT QtdItens from Lote WHERE CodigoLote = @CodigoLote;";
            Comando.Parameters.AddWithValue("@CodigoLote", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.Quantidadeitens = Convert.ToInt32(Leitor["Qtditens"].ToString());

            Leitor.Close();
            Conexao.Close();

            Conexao.Open();

            Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT count  (*) as Contador FROM ingrediente WHERE FK_CodigoLote = @FK_CodigoLote;";
            Comando.Parameters.AddWithValue("@FK_CodigoLote", ID);

            Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.Contador = Convert.ToInt32(Leitor["Contador"].ToString());
            Conexao.Close();
        }

        /******************************************************************** CADASTRAR LOTE ********************************************************************/
        public Boolean CadastrarLote(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Lote (NomeLote,DataEntrada,DataValidade,QtdItens,FK_NIFUsuario) VALUES (@NomeLote,@DataEntrada,@DataValidade,@QtdItens,@FK_NIFUsuario);";

            Comando.Parameters.AddWithValue("@NomeLote", this.NomeLote);
            Comando.Parameters.AddWithValue("@DataEntrada", this.DataEntrada);
            Comando.Parameters.AddWithValue("@DataValidade", this.DataValidade);
            Comando.Parameters.AddWithValue("@QtdItens", this.Quantidadeitens);
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** ALTERAR LOTE ********************************************************************/
        public Boolean AlterarLote()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Lote SET NomeLote = @NomeLote,DataEntrada = @DataEntrada,DataValidade = @DataValidade,QtdItens = @QtdItens WHERE CodigoLote = @CodigoLote;";

            Comando.Parameters.AddWithValue("@CodigoLote", this.CodigoLote);
            Comando.Parameters.AddWithValue("@NomeLote", this.NomeLote);
            Comando.Parameters.AddWithValue("@DataEntrada", this.DataEntrada);
            Comando.Parameters.AddWithValue("@DataValidade", this.DataValidade);
            Comando.Parameters.AddWithValue("@QtdItens", this.Quantidadeitens);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** LISTAR LOTE ********************************************************************/
        public List<Lote> ListarLotes()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM Lote;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Lote> Lotes = new List<Lote>();

            while (Leitor.Read())
            {
                Lote L = new Lote();
                L.CodigoLote = Convert.ToInt32(Leitor["CodigoLote"].ToString());
                L.NomeLote = Leitor["NomeLote"].ToString();
                L.DataValidade = Convert.ToDateTime(Leitor["DataValidade"].ToString());
                L.Quantidadeitens = Convert.ToInt32(Leitor["Qtditens"].ToString());
                L.DataEntrada = Convert.ToDateTime(Leitor["DataEntrada"].ToString());
                L.inserido = PegaValores(L.CodigoLote);
                Lotes.Add(L);
            }
            Conexao.Close();
            return Lotes;
        }

        /******************************************************************** PEGA A QUANTIDADE DE INGREDIENTE DO LOTE ********************************************************************/
        public Int32 PegaValores(Int32 Lote)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT count  (*) as Contador FROM ingrediente WHERE FK_CodigoLote = @FK_CodigoLote;";
            Comando.Parameters.AddWithValue("@FK_CodigoLote", Lote);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            if (Convert.ToInt32(Leitor["Contador"].ToString()) > 0)
            {
                this.Contador = Convert.ToInt32(Leitor["Contador"].ToString());
            }
            else
            {
                this.Contador = 0;
            }
            Conexao.Close();
            return this.Contador;
        }

        /******************************************************************** DELETAR LOTE ********************************************************************/
        public Boolean DeletarLote(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "DELETE FROM Lote WHERE CodigoLote = @CodigoLote";
            Comando.Parameters.AddWithValue("@CodigoLote", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** DETALHES DO LOTE ********************************************************************/
        public List<Ingrediente> DetalhesLote(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM Ingrediente WHERE FK_CodigoLote = @FK_CodigoLote;";
            Comando.Parameters.AddWithValue("@FK_CodigoLote", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Ingrediente> DetalhesLote = new List<Ingrediente>();

            while (Leitor.Read())
            {
                Ingrediente I = new Ingrediente();
                I.CodigoIngrediente = Convert.ToInt32(Leitor["CodigoIngrediente"].ToString());
                I.NomeIngrediente = Leitor["NomeIngrediente"].ToString();
                I.UnidadeMedida = Leitor["UnidadeMedida"].ToString();
                I.QtdIngrediente = Convert.ToInt32(Leitor["QtdIngrediente"].ToString());

                DetalhesLote.Add(I);
            }
            Conexao.Close();
            return DetalhesLote;
        }
    }
}