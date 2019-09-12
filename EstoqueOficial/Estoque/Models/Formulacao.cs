using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;


namespace Estoque.Models
{
    public class Formulacao
    {
        public Int32 CodigoFormulacao { get; set; }
        public String NomeFormulacao { get; set; }
        public Double Porcentagem { get; set; }
        public String NomeIngrediente { get; set; }
        public Int32 CodigoIngrediente { get; set; }


        public String Aux { get; set; }
        public Double Total { get; set; }
        public Double[] Resultado = new Double[0];

        /******************************************************************** CONSTRUTORES FORMULAÇÃO ********************************************************************/
        public Formulacao() { }
        public Formulacao(String ID)
        {

            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM  Formulacao WHERE CodigoFormulacao = @CodigoFormulacao;";
            Comando.Parameters.AddWithValue("@CodigoFormulacao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.CodigoFormulacao = Convert.ToInt32(Leitor["CodigoFormulacao"].ToString());
            this.NomeFormulacao = Leitor["NomeFormulacao"].ToString();

            Conexao.Close();
        }

        /******************************************************************** CADASTRAR FORMULAÇÃO ********************************************************************/
        public Boolean CadastrarFormulacao()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Formulacao (NomeFormulacao) VALUES (@NomeFormulacao);";
            Comando.Parameters.AddWithValue("@NomeFormulacao", this.NomeFormulacao);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** ALTERAR FORMULAÇÃO ********************************************************************/
        public Boolean AlterarFormulacao(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Formulacao SET NomeFormulacao = @NomeFormulacao WHERE CodigoFormulacao = @CodigoFormulacao;";

            Comando.Parameters.AddWithValue("@CodigoFormulacao", ID);
            Comando.Parameters.AddWithValue("@NomeFormulacao", this.NomeFormulacao);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** EXCLUIR FORMULAÇÕES ********************************************************************/
        public Boolean ExcluirFormulacao(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "DELETE FROM Formu_Ingrediente WHERE FK_CodigoFormulacao = @FK_CodigoFormulacao;";
            Comando.Parameters.AddWithValue("@FK_CodigoFormulacao", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();


            Conexao.Open();
            Comando.CommandText = "DELETE FROM Formulacao WHERE CodigoFormulacao = @CodigoFormulacao;";
            Comando.Parameters.AddWithValue("@CodigoFormulacao", ID);

            Int32 Resultado2 = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** LISTAR FORMULAÇÕES ********************************************************************/
        public List<Formulacao> ListarFormulacoes()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM  Formulacao;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Formulacao> Formulacoes = new List<Formulacao>();

            while (Leitor.Read())
            {
                Formulacao F = new Formulacao();
                F.CodigoFormulacao = Convert.ToInt32(Leitor["CodigoFormulacao"].ToString());
                F.NomeFormulacao = Leitor["NomeFormulacao"].ToString();

                Formulacoes.Add(F);
            }
            Conexao.Close();
            return Formulacoes;
        }

        /******************************************************************** FORMULAÇÕES DETALHES ********************************************************************/
        public List<Formulacao> DetalhesFormulacao(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT F.*,I.NomeIngrediente FROM  Formu_Ingrediente F JOIN Ingrediente I ON CodigoIngrediente = FK_CodigoIngrediente WHERE FK_CodigoFormulacao = @FK_CodigoFormulacao;";
            Comando.Parameters.AddWithValue("@FK_CodigoFormulacao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Formulacao> DetalhesFormulacoes = new List<Formulacao>();

            while (Leitor.Read())
            {
                Formulacao F = new Formulacao();
                F.NomeIngrediente = Leitor["NomeIngrediente"].ToString();
                F.CodigoFormulacao = Convert.ToInt32(Leitor["FK_CodigoFormulacao"].ToString());
                F.Porcentagem = Convert.ToDouble(Leitor["Porcentagem"].ToString());
                F.CodigoIngrediente = Convert.ToInt32(Leitor["FK_CodigoIngrediente"].ToString());

                DetalhesFormulacoes.Add(F);
            }
            Conexao.Close();
            return DetalhesFormulacoes;
        }

        /******************************************************************** ADD INGREDIENTE - FORMULACAO ********************************************************************/
        public Boolean CadastrarIngredienteF(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Formu_Ingrediente (FK_CodigoIngrediente, FK_CodigoFormulacao, Porcentagem) VALUES (@FK_CodigoIngrediente,@FK_CodigoFormulacao,@Porcentagem);";

            Comando.Parameters.AddWithValue("@FK_CodigoIngrediente", this.CodigoIngrediente);
            Comando.Parameters.AddWithValue("@FK_CodigoFormulacao", ID);
            Comando.Parameters.AddWithValue("@Porcentagem", this.Porcentagem);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** LISTAR INGREDIENTE - FORMULACAO ********************************************************************/
        public List<Formulacao> DetalhesIngrediente(String ID, Object CodigoFormulacao)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT P.Porcentagem, I.NomeIngrediente FROM Formu_Ingrediente P JOIN Ingrediente I on CodigoIngrediente = FK_CodigoIngrediente WHERE FK_CodigoIngrediente = @FK_CodigoIngrediente AND FK_CodigoFormulacao = @FK_CodigoFormulacao;";
            Comando.Parameters.AddWithValue("@FK_CodigoIngrediente", ID);
            Comando.Parameters.AddWithValue("@FK_CodigoFormulacao", CodigoFormulacao);

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Formulacao> DetalheIngrediente = new List<Formulacao>();

            while (Leitor.Read())
            {
                Formulacao F = new Formulacao();
                F.NomeIngrediente = Leitor["NomeIngrediente"].ToString();
                F.Porcentagem = Convert.ToDouble(Leitor["Porcentagem"].ToString());
                DetalheIngrediente.Add(F);
            }
            return DetalheIngrediente;
        }

        /******************************************************************** ALTERAR INGREDIENTE - FORMULACAO ********************************************************************/
        public Boolean AlterarIngredienteF(String ID, Object CodigoFormulacao)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Formu_Ingrediente SET FK_CodigoIngrediente = @FK_CodigoIngrediente, FK_CodigoFormulacao = @FK_CodigoFormulacao, Porcentagem = @Porcentagem WHERE FK_CodigoFormulacao = @FK_CodigoFormulacao2 AND FK_CodigoIngrediente = @FK_CodigoIngrediente2;";

            Comando.Parameters.AddWithValue("@FK_CodigoIngrediente", this.CodigoIngrediente);
            Comando.Parameters.AddWithValue("@FK_CodigoFormulacao", CodigoFormulacao);
            Comando.Parameters.AddWithValue("@Porcentagem", this.Porcentagem);
            Comando.Parameters.AddWithValue("@FK_CodigoFormulacao2", CodigoFormulacao);
            Comando.Parameters.AddWithValue("@FK_CodigoIngrediente2", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** EXCLUIR INGREDIENTE - FORMULACAO ********************************************************************/
        public Boolean ExcluirIngredienteF(String ID, Object FK_CodigoFormualcao)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "DELETE FROM Formu_Ingrediente WHERE FK_CodigoIngrediente = @FK_CodigoIngrediente AND FK_CodigoFormulacao = @FK_CodigoFormulacao;";
            Comando.Parameters.AddWithValue("@FK_CodigoIngrediente", ID);
            Comando.Parameters.AddWithValue("@FK_CodigoFormulacao", FK_CodigoFormualcao);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }
    }
}

