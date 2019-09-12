using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;


namespace Estoque.Models
{
    public class Curso
    {
        public Int32 CodigoCurso { get; set; }
        public Int32 FK_NIF { get; set; }
        public String NomeCurso { get; set; }
        public Int32 Duracao { get; set; }

        public static Int32 Aux { get; set; }

        /******************************************************************** CONSTRUTORES CURSO ********************************************************************/
        public Curso() { }

        public Curso(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Curso WHERE CodigoCurso = @CodigoCurso;";
            Comando.Parameters.AddWithValue("@CodigoCurso", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.CodigoCurso = Convert.ToInt32(Leitor["CodigoCurso"].ToString());
            this.FK_NIF = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
            this.NomeCurso = Leitor["NomeCurso"].ToString();
            this.Duracao = Convert.ToInt32(Leitor["Duracao"].ToString());

            Conexao.Close();
        }

        /******************************************************************** CADASTRAR CURSO ********************************************************************/
        public Boolean Cadastrar(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Curso (FK_NIFUsuario,NomeCurso,Duracao) VALUES (@FK_NIFUsuario,@NomeCurso,@Duracao);";

            Comando.Parameters.AddWithValue("@FK_NIFUsuario", ID);
            Comando.Parameters.AddWithValue("@NomeCurso", this.NomeCurso);
            Comando.Parameters.AddWithValue("@Duracao", this.Duracao);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** ALTERAR CURSO ********************************************************************/
        public Boolean AlterarCurso()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Curso SET NomeCurso = @NomeCurso, Duracao = @Duracao WHERE CodigoCurso = @CodigoCurso;";

            Comando.Parameters.AddWithValue("@CodigoCurso", this.CodigoCurso);
            Comando.Parameters.AddWithValue("@NomeCurso", this.NomeCurso);
            Comando.Parameters.AddWithValue("@Duracao", this.Duracao);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** REMOVER CURSOS ********************************************************************/
        public Boolean RemoverCurso(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "DELETE FROM Curso WHERE CodigoCurso = @CodigoCurso";
            Comando.Parameters.AddWithValue("@CodigoCurso", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** LISTAR CURSO ********************************************************************/
        public List<Curso> ListarCursos(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Curso";
            //Comando.Parameters.AddWithValue("@FK_NIFUsuario", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Curso> Cursos = new List<Curso>();

            while (Leitor.Read())
            {
                Curso C = new Curso();
                C.CodigoCurso = Convert.ToInt32(Leitor["CodigoCurso"].ToString());
                C.FK_NIF = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                C.NomeCurso = Leitor["NomeCurso"].ToString();
                C.Duracao = Convert.ToInt32(Leitor["Duracao"].ToString());

                Aux = C.CodigoCurso;
                Cursos.Add(C);
            }
            Conexao.Close();
            return Cursos;
        }

        /******************************************************************** VERIFICA CURSO ********************************************************************/
        public Boolean VerificaCurso(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT FK_CodigoCurso FROM Turma WHERE FK_CodigoCurso = @FK_CodigoCurso";
            Comando.Parameters.AddWithValue("@FK_CodigoCurso", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            int Codigo = 0;

            while (Leitor.Read())
            {
                try
                {
                    Codigo = Convert.ToInt32(Leitor["FK_CodigoCurso"].ToString());
                }
                catch
                {
                    Codigo = 0;
                }
            }

            Conexao.Close();
            return (Codigo > 0) ? true : false;
        }
    }
}