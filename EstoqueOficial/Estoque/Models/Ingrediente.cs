using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Estoque.Controllers;
using System.Configuration;

namespace Estoque.Models
{
    public class Ingrediente
    {
        public Int32 CodigoIngrediente { get; set; }
        public Int32 FK_CodigoLote { get; set; }
        public String NomeIngrediente { get; set; }
        public String UnidadeMedida { get; set; }
        public Int32 QtdIngrediente { get; set; }
        public Int32 NivelRisco { get; set; }
        public Int32 FK_NIFUsuario { get; set; }

        public DateTime DataRetirada { get; set; }

        public String Porcentagem { get; set; }

        public Int32 teste { get; set; }

        public List<Ingrediente> Ingredientes { get; set; } = new List<Ingrediente>();
        public List<Int32> Alerta { get; set; } = new List<Int32>();
        public String NomeLote { get; set; }

        /******************************************************************** CONSTRUTORES INGREDIENTE ********************************************************************/
        public Ingrediente() { }
        public Ingrediente(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM Ingrediente WHERE CodigoIngrediente = @CodigoIngrediente;";
            Comando.Parameters.AddWithValue("@CodigoIngrediente", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.CodigoIngrediente = Convert.ToInt32(Leitor["CodigoIngrediente"].ToString());
            this.FK_CodigoLote = Convert.ToInt32(Leitor["FK_CodigoLote"].ToString());
            this.NomeIngrediente = Leitor["NomeIngrediente"].ToString();
            this.UnidadeMedida = Leitor["UnidadeMedida"].ToString();
            this.QtdIngrediente = Convert.ToInt32(Leitor["QtdIngrediente"].ToString());
            this.NivelRisco = Convert.ToInt32(Leitor["NivelRisco"].ToString());

            Conexao.Close();
        }

        /******************************************************************** CADASTRAR INGREDIENTE ********************************************************************/
        public Boolean CadastrarIngrediente(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Ingrediente (FK_CodigoLote,NomeIngrediente,UnidadeMedida,QtdIngrediente,NivelRisco) VALUES (@FK_CodigoLote,@NomeIngrediente,@UnidadeMedida,@QtdIngrediente, @NivelRisco);";

            Comando.Parameters.AddWithValue("@FK_CodigoLote", ID);
            Comando.Parameters.AddWithValue("@NomeIngrediente", this.NomeIngrediente);
            Comando.Parameters.AddWithValue("@UnidadeMedida", this.UnidadeMedida);
            Comando.Parameters.AddWithValue("@QtdIngrediente", this.QtdIngrediente);
            Comando.Parameters.AddWithValue("@NivelRisco", this.NivelRisco);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** ALTERAR INGREDIENTE ********************************************************************/
        public Boolean AlterarIngrediente(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Ingrediente SET NomeIngrediente = @NomeIngrediente, UnidadeMedida = @UnidadeMedida, QtdIngrediente = @QtdIngrediente, NivelRisco = @NivelRisco WHERE CodigoIngrediente = @CodigoIngrediente ;";

            Comando.Parameters.AddWithValue("@CodigoIngrediente", ID);
            Comando.Parameters.AddWithValue("@NomeIngrediente", this.NomeIngrediente);
            Comando.Parameters.AddWithValue("@UnidadeMedida", this.UnidadeMedida);
            Comando.Parameters.AddWithValue("@QtdIngrediente", this.QtdIngrediente);
            Comando.Parameters.AddWithValue("@NivelRisco", this.NivelRisco);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** LISTAR INGREDIENTE ********************************************************************/
        public List<Ingrediente> ListarIngredientes()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT I.*, L.CodigoLote, L.NomeLote FROM Ingrediente I JOIN Lote L ON CodigoLote = FK_CodigoLote;";

            SqlDataReader Leitor = Comando.ExecuteReader();



            while (Leitor.Read())
            {
                Ingrediente I = new Ingrediente();
                I.FK_CodigoLote = Convert.ToInt32(Leitor["FK_CodigoLote"].ToString());
                I.NomeLote = Leitor["NomeLote"].ToString();
                I.CodigoIngrediente = Convert.ToInt32(Leitor["CodigoIngrediente"].ToString());
                I.NomeIngrediente = Leitor["NomeIngrediente"].ToString();
                I.QtdIngrediente = Convert.ToInt32(Leitor["QtdIngrediente"].ToString());
                I.UnidadeMedida = Leitor["UnidadeMedida"].ToString();
                I.NivelRisco = Convert.ToInt32(Leitor["NivelRisco"].ToString());
                I.FK_CodigoLote = Convert.ToInt32(Leitor["FK_CodigoLote"].ToString());


                Ingredientes.Add(I);
            }
            Conexao.Close();

            teste = Ingredientes.Count();
            return Ingredientes;
        }

        /******************************************************************** LISTAR INGREDIENTE - ALTERAR INGREDIENTE F ********************************************************************/
        public List<Ingrediente> IngredienteF()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT CodigoIngrediente, NomeIngrediente FROM Ingrediente;";

            SqlDataReader Leitor = Comando.ExecuteReader();

            while (Leitor.Read())
            {
                Ingrediente I = new Ingrediente();
                I.CodigoIngrediente = Convert.ToInt32(Leitor["CodigoIngrediente"].ToString());
                I.NomeIngrediente = Leitor["NomeIngrediente"].ToString();

                Ingredientes.Add(I);
            }
            Conexao.Close();

            return Ingredientes;
        }

        /******************************************************************** EXCLUIR INGREDIENTE ********************************************************************/
        public Boolean ExcluirIngrediente(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "DELETE FROM Ingrediente WHERE CodigoIngrediente = @CodigoIngrediente;";

            Comando.Parameters.AddWithValue("@CodigoIngrediente", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** SELECIONA INGREDIENTE ********************************************************************/
        public Boolean SelecionaIngrediente(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT QtdIngrediente FROM ingrediente WHERE CodigoIngrediente = @ID;";
            Comando.Parameters.AddWithValue("@ID", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            while (Leitor.Read())
            {
                QtdIngrediente = Convert.ToInt32(Leitor["QtdIngrediente"].ToString());
            }


            Conexao.Close();

            return (QtdIngrediente > 0) ? true : false;
        }

        /******************************************************************** RETIRAR INGREDIENTE ********************************************************************/
        public Boolean RetirarIngrediente(String ID, Int32 Qtd, Object NIF, Int32 QuantidadeRequisitada)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE ingrediente SET QtdIngrediente = @QtdIngrediente WHERE CodigoIngrediente = @ID;";
            Comando.Parameters.AddWithValue("QtdIngrediente", Qtd);
            Comando.Parameters.AddWithValue("@ID", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = " INSERT INTO IngredienteRetirado (FK_CodigoIngrediente, FK_NIFUsuario, QtdRetirado, DataRetirado) VALUES (@FK_CodigoIngrediente, @FK_NIFUsuario, @QtdRetirado, @DataRetirado);";
            Comando.Parameters.AddWithValue("@FK_CodigoIngrediente", ID);
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", NIF);
            Comando.Parameters.AddWithValue("@QtdRetirado", QuantidadeRequisitada);
            Comando.Parameters.AddWithValue("@DataRetirado", DataRetirada);

            Int32 Resultado2 = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** LISTAR INGREDIENTE RETIRADO ********************************************************************/
        public List<Ingrediente> ListaIngredientesRetirados()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT I.NomeIngrediente,I.FK_CodigoLote,L.NomeLote,IR.*, I.UnidadeMedida FROM IngredienteRetirado IR JOIN Ingrediente I ON CodigoIngrediente = FK_CodigoIngrediente JOIN Lote L ON CodigoLote = FK_CodigoLote;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Ingrediente> Retirados = new List<Ingrediente>();

            while (Leitor.Read())
            {
                Ingrediente I = new Ingrediente();
                I.CodigoIngrediente = Convert.ToInt32(Leitor["FK_CodigoIngrediente"].ToString());
                I.NomeIngrediente = Leitor["NomeIngrediente"].ToString();
                I.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                I.QtdIngrediente = Convert.ToInt32(Leitor["QtdRetirado"].ToString());
                I.DataRetirada = Convert.ToDateTime(Leitor["DataRetirado"].ToString());
                I.UnidadeMedida = Leitor["UnidadeMedida"].ToString();
                I.FK_CodigoLote = Convert.ToInt32(Leitor["FK_CodigoLote"].ToString());
                I.NomeLote = Leitor["NomeLote"].ToString();
                Retirados.Add(I);
            }
            return Retirados;
        }

        /******************************************************************** PESQUISAR INGREDIENTE ********************************************************************/
                                                                     /******* SEM TEMPO PARA IMPLEMENTAR *******/
        //public List<Ingrediente> PesquisarIngredinte(String Nome)
        //{
        //    SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
        //    //SqlConnection Conexao = new SqlConnection("Server = DESKTOP-VQGMLTC\\SQLSERVER; Database = DLEstoque; Trusted_Connection = True;");
        //    //SqlConnection Conexao = new SqlConnection("Server=Ricardo-PC; Database=DRLTCC; User Id=sa; Password=Senai1234;");

        //    Conexao.Open();
        //    SqlCommand Comando = new SqlCommand();
        //    Comando.Connection = Conexao;

        //    Comando.CommandText = "SELECT * FROM ingrediente WHERE NomeIngrediente LIKE '%@Nome%';";
        //    Comando.Parameters.AddWithValue("@Nome", Nome);

        //    SqlDataReader Leitor = Comando.ExecuteReader();

        //    List<Ingrediente> IngredientePesquisados = new List<Ingrediente>();
        //    Ingrediente I = new Ingrediente();

        //    while (Leitor.Read())
        //    {
        //        I.CodigoIngrediente = Convert.ToInt32(Leitor["CodigoIngrediente"].ToString());
        //        I.NomeIngrediente = Leitor["NomeIngrediente"].ToString();
        //        I.QtdIngrediente = Convert.ToInt32(Leitor["QtdIngrediente"].ToString());

        //        IngredientePesquisados.Add(I);
        //    }

        //    Conexao.Close();

        //    return IngredientePesquisados;
        //}

    }
}
