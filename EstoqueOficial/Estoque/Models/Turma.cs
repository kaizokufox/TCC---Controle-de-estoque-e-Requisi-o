using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Estoque.Controllers;
using System.Configuration;

namespace Estoque.Models
{
    public class Turma
    {
        public Int32    CodigoTurma { get; set; }
        public Int32    CodigoCurso { get; set; }
        public String   NomeTurma   { get; set; }
        public Int32    QtdAlunos   { get; set; }
        public String   Periodo     { get; set; }
        public DateTime DataInicio  { get; set; }
        public DateTime DataTermino { get; set; }

        // variavesi do controle //

        public String   Aux               { get; set; }
        public Int32    Soma              { get; set; }
        public String   DiasSemana        { get; set; }
        public String   NomeCurso         { get; set; }
        public Int32    CodigoDiasSemana  { get; set; }

        /******************************************************************** CONSTRUTORES TURMA ********************************************************************/
        public Turma() { }

        public Turma(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Turma WHERE CodigoTurma = @CodigoTurma;";
            Comando.Parameters.AddWithValue("@CodigoTurma", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.CodigoTurma = Convert.ToInt32(Leitor["CodigoTurma"].ToString());
            this.CodigoCurso = Convert.ToInt32(Leitor["FK_CodigoCurso"].ToString());
            this.NomeTurma   = Leitor["NomeTurma"].ToString();
            this.QtdAlunos   = Convert.ToInt32(Leitor["QtdAlunos"].ToString());
            this.Periodo     = Leitor["Periodo"].ToString();
            this.DataInicio  = Convert.ToDateTime(Leitor["DataInicio"].ToString());
            this.DataTermino = Convert.ToDateTime(Leitor["DataTermino"].ToString());

            Leitor.Close();
            Conexao.Close();

            Conexao.Open();

            Comando.CommandText = "SELECT FK_CodigoDiasSemana FROM Turma_Semana WHERE FK_CodigoTurma = @FK_CodigoTurma;";
            Comando.Parameters.AddWithValue("@FK_CodigoTurma", ID);

            Leitor = Comando.ExecuteReader();
            
            while (Leitor.Read())
            {
                this.CodigoDiasSemana = Convert.ToInt32(Leitor["FK_CodigoDiasSemana"].ToString());
            }

            Leitor.Close();
            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "SELECT DiasSemana FROM DiasSemana WHERE CodigoDiasSemana = @CodigoDiasSemana;";
            Comando.Parameters.AddWithValue("@CodigoDiasSemana", this.CodigoDiasSemana);

            Leitor = Comando.ExecuteReader();

            while (Leitor.Read())
            {
                this.DiasSemana = Leitor["DiasSemana"].ToString();
                
            }
            Conexao.Close();

        }

        /******************************************************************** CADASTRAR TURMA ********************************************************************/
        public Boolean CadastrarTurma(Int32 ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            /***** INSERINDO NORMALMENTE OS DADOS NA TABELA TURMA *****/
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Turma (FK_CodigoCurso,NomeTurma,QtdAlunos,DataInicio,DataTermino,Periodo) VALUES (@FK_CodigoCurso,@NomeTurma,@QtdAlunos,@DataInicio,@DataTermino,@Periodo);";

            Comando.Parameters.AddWithValue("@FK_CodigoCurso",     ID);
            Comando.Parameters.AddWithValue("@NomeTurma",       this.NomeTurma);
            Comando.Parameters.AddWithValue("@QtdAlunos",       this.QtdAlunos);
            Comando.Parameters.AddWithValue("@DataInicio",      this.DataInicio);
            Comando.Parameters.AddWithValue("@DataTermino",     this.DataTermino);
            Comando.Parameters.AddWithValue("@Periodo",         this.Periodo);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            /*** TABELA Turma = COLUNA -> CodigoTurma***/
            Conexao.Open();

            Comando.CommandText = "SELECT CodigoTurma FROM Turma WHERE FK_CodigoCurso = @FK_CodigoCurso2;";
            Comando.Parameters.AddWithValue("@FK_CodigoCurso2", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            

            while (Leitor.Read())
            {
                this.CodigoTurma = Convert.ToInt32(Leitor["CodigoTurma"].ToString());
            }

            Conexao.Close();

            /***** INSERINDO NORMALMENTE OS DADOS NA TABELA DiasSemana *****/
            Conexao.Open();

            Comando.CommandText = "INSERT INTO DiasSemana (DiasSemana,FK_CodigoTurma) VALUES (@DiasSemana,@FK_CodigoTurma);";
            Comando.Parameters.AddWithValue("@DiasSemana", this.Soma);
            Comando.Parameters.AddWithValue("@FK_CodigoTurma", this.CodigoTurma);

            Int32 Resultado2 = Comando.ExecuteNonQuery();

            Leitor.Close();
            Conexao.Close();

            /***** PEGANDO AS COLUNAS PARA INSERIR NA TABELA TURMA_SEMANA *****/

            /*** TABELA DiasSemana = COLUNA -> CodigoDiasSemanas***/
            Conexao.Open();

            Comando.CommandText = "SELECT CodigoDiasSemana FROM DiasSemana WHERE DiasSemana = @Semana;";
            Comando.Parameters.AddWithValue("@Semana", this.Soma);

            SqlDataReader Leitor2 = Comando.ExecuteReader();

            while (Leitor2.Read())
            {
                this.CodigoDiasSemana = Convert.ToInt32(Leitor2["CodigoDiasSemana"].ToString());
            }

            Leitor2.Close();
            Conexao.Close();

            
            /***** INSERINDO NA TABELA TURMA_SEMANA *****/
            Conexao.Open();

            Comando.CommandText = "INSERT INTO Turma_Semana (FK_CodigoDiasSemana, FK_CodigoTurma) VALUES (@FK_CodigoDiasSemana, @FK_CodigoTurma2);";
            Comando.Parameters.AddWithValue("@FK_CodigoDiasSemana",    this.CodigoDiasSemana);
            Comando.Parameters.AddWithValue("@FK_CodigoTurma2",  CodigoTurma);

            Int32 Resultado3 = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0 && Resultado3 > 0) ? true : false;
        }

        /******************************************************************** ALTERAR TURMA ********************************************************************/
        public Boolean AlterarTurma()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Turma SET FK_CodigoCurso = @FK_CodigoCurso, NomeTurma = @NomeTurma, QtdAlunos = @QtdAlunos, Periodo = @Periodo, DataInicio = @DataInicio, DataTermino = @DataTermino WHERE CodigoTurma = @CodigoTurma;";

            Comando.Parameters.AddWithValue("@CodigoTurma",     this.CodigoTurma);
            Comando.Parameters.AddWithValue("@FK_CodigoCurso",  this.CodigoCurso);
            Comando.Parameters.AddWithValue("@NomeTurma",       this.NomeTurma);
            Comando.Parameters.AddWithValue("@QtdAlunos",       this.QtdAlunos);
            Comando.Parameters.AddWithValue("@Periodo",         this.Periodo);
            Comando.Parameters.AddWithValue("@DataInicio",      this.DataInicio);
            Comando.Parameters.AddWithValue("@DataTermino",     this.DataTermino);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            Conexao.Close();
            /***** INSERINDO NORMALMENTE OS DADOS NA TABELA DiasSemana *****/
            Conexao.Open();

            Comando.CommandText = "UPDATE DiasSemana SET DiasSemana = @DiasSemana WHERE FK_CodigoTurma = @FK_CodigoTurma;";
            Comando.Parameters.AddWithValue("@DiasSemana", this.Soma);
            Comando.Parameters.AddWithValue("@FK_CodigoTurma", this.CodigoTurma);

            Int32 Resultado2 = Comando.ExecuteNonQuery();

            Conexao.Close();
            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** REMOVER TURMAS ********************************************************************/
        public Boolean RemoverTurma(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Conexao.Open();

            Comando.CommandText = "SELECT * FROM Turma_Semana WHERE FK_CodigoTurma = @FK_CodigoTurma";
            Comando.Parameters.AddWithValue("@FK_CodigoTurma", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            while (Leitor.Read())
            {
                this.CodigoDiasSemana = Convert.ToInt32(Leitor["FK_CodigoDiasSemana"].ToString());
            }

            Conexao.Close();

            Conexao.Open();

            Comando.CommandText = "DELETE FROM Turma_Semana WHERE FK_CodigoTurma = @FK_CodigoTurma2";
            Comando.Parameters.AddWithValue("@FK_CodigoTurma2", ID);

            Int32 Resultado2 = Comando.ExecuteNonQuery();
            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "DELETE FROM DiasSemana WHERE CodigoDiasSemana = @CodigoDiasSemana";
            Comando.Parameters.AddWithValue("@CodigoDiasSemana", this.CodigoDiasSemana);

            Int32 Resultado4 = Comando.ExecuteNonQuery();
            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "DELETE FROM Turma WHERE CodigoTurma = @CodigoTurma";
            Comando.Parameters.AddWithValue("@CodigoTurma", ID);

            Int32 Resultado3 = Comando.ExecuteNonQuery();
            Conexao.Close();
                        
            return (Resultado2 > 0 && Resultado3 > 0 && Resultado4 > 0) ? true : false;
        }

        /******************************************************************** LISTAR TURMA  ********************************************************************/
        public List<Turma> ListarTurmas()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT T.*, C.NomeCurso FROM Turma T JOIN Curso C on C.CodigoCurso = T.FK_CodigoCurso"; ;

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Turma> Turmas = new List<Turma>();

            while (Leitor.Read())
            {
                Turma T = new Turma();
                T.CodigoTurma   = Convert.ToInt32(Leitor["CodigoTurma"].ToString());
                T.NomeTurma     = Leitor["NomeTurma"].ToString();
                T.QtdAlunos     = Convert.ToInt32(Leitor["QtdAlunos"].ToString());
                T.NomeCurso     = Leitor["NomeCurso"].ToString();
                T.Periodo       = Leitor["Periodo"].ToString();
                T.DataInicio    = Convert.ToDateTime(Leitor["DataInicio"].ToString());
                T.DataTermino   = Convert.ToDateTime(Leitor["DataTermino"].ToString());

                Turmas.Add(T);
            }
            Conexao.Close();
            return Turmas;
        }

        /******************************************************************** LISTAR DIAS SEMANA  ********************************************************************/
        public Int32 ListarDiasSemana(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT FK_CodigoDiasSemana FROM Turma_Semana WHERE FK_CodigoTurma = @FK_CodigoTurma ";
            Comando.Parameters.AddWithValue("@FK_CodigoTurma", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            this.DiasSemana = Leitor["FK_CodigoDiasSemana"].ToString();
            Leitor.Close();

            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "SELECT DiasSemana FROM DiasSemana WHERE CodigoDiasSemana = @FK_CodigoTurma2 ";
            Comando.Parameters.AddWithValue("@FK_CodigoTurma2", this.DiasSemana);

            Leitor = Comando.ExecuteReader();

            Leitor.Read();
            this.Soma = Convert.ToInt32(Leitor["DiasSemana"].ToString());

            Conexao.Close();
            return this.Soma;
        }
    }
}