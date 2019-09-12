using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;

namespace Estoque.Models
{

    public class Usuario
    {

        public Int32 NIF { get; set; }
        public String Nome { get; set; }
        public String Sobrenome { get; set; }
        public String CPF { get; set; }
        public String Cargo { get; set; }
        public String Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public String Email { get; set; }
        public String Senha { get; set; }

        public Usuario() { }

        public Usuario(String ID)
        {
            ///Byte[] Codigo = Convert.FromBase64String(ID);

            //SqlConnection Conexao = new SqlConnection("Server = DESKTOP-VQGMLTC\\SQLSERVER; Database = DLEstoque; Trusted_Connection = True;");
            SqlConnection Conexao = new SqlConnection("Server=ESN509VMSSQL; Database=DLEstoque; User Id=Aluno; Password=Senai1234;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Usuario WHERE NIF = @NIF;";
            Comando.Parameters.AddWithValue("@NIF", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            //Codigo = Encoding.UTF8.GetBytes(Leitor["Codigo"].ToString());




            this.NIF = Convert.ToInt32(Leitor["NIF"].ToString());
            this.Nome = Leitor["Nome"].ToString();
            this.Sobrenome = Leitor["Sobrenome"].ToString();
            this.CPF = Leitor["CPF"].ToString();
            this.Cargo = Leitor["Cargo"].ToString();
            this.Telefone = Leitor["Telefone"].ToString();
            this.DataNascimento = Convert.ToDateTime(Leitor["DataNascimento"].ToString());
            this.Email = Leitor["Email"].ToString();
            this.Senha = Leitor["Senha"].ToString();

            Conexao.Close();
        }

        /******************************************************************** AUTENTICAR USUARIO ********************************************************************/
        public void Login(Int32 NIF, String Senha)
        {
            SqlConnection Conexao = new SqlConnection("Server = ESN509VMSSQL; Database = DLEstoque; User Id = Aluno; Password = Senai1234; ");
            //SqlConnection Conexao = new SqlConnection("Server = DESKTOP-VQGMLTC\\SQLSERVER; Database = DLEstoque; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Usuario WHERE NIF = @NIF and Senha = @Senha";
            Comando.Parameters.AddWithValue("@NIF", NIF);
            Comando.Parameters.AddWithValue("@Senha", Senha);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            this.NIF = Convert.ToInt32(Leitor["NIF"].ToString());
            this.Senha = Leitor["Senha"].ToString();

            //UnicodeEncoding UE = new UnicodeEncoding();
            //byte[] HashValue, MessageBytes = UE.GetBytes(Senha);
            //SHA1Managed SHhash = new SHA1Managed();
            //string strHex = "";
            //HashValue = SHhash.ComputeHash(MessageBytes);
            //foreach (byte b in HashValue)
            //{
            //    strHex += String.Format("{0:x2}", b);
            //}
            Leitor.Close();

            Conexao.Close();

            // return strHex;
        }

        /******************************************************************** CADASTRAR USUARIO ********************************************************************/
        public Boolean Cadastrar()
        {
            SqlConnection Conexao = new SqlConnection("Server=ESN509VMSSQL; Database=DLEstoque; User Id=Aluno; Password=Senai1234;");
            //SqlConnection Conexao = new SqlConnection("Server = DESKTOP-VQGMLTC\\SQLSERVER; Database = DLEstoque; Trusted_Connection = True;");

            Conexao.Open();


            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "INSERT INTO Usuario (NIF,Nome,Sobrenome,CPF,Cargo,Telefone,DataNascimento,Email,Senha) VALUES (@NIF,@Nome,@Sobrenome,@CPF,@Cargo,@Telefone,@DataNascimento,@Email,@Senha);";

            Comando.Parameters.AddWithValue("NIF", this.NIF);
            Comando.Parameters.AddWithValue("Nome", this.Nome);
            Comando.Parameters.AddWithValue("Sobrenome", this.Sobrenome);
            Comando.Parameters.AddWithValue("CPF", this.CPF);
            Comando.Parameters.AddWithValue("Cargo", this.Cargo);
            Comando.Parameters.AddWithValue("Telefone", this.Telefone);
            Comando.Parameters.AddWithValue("DataNascimento", this.DataNascimento);
            Comando.Parameters.AddWithValue("Email", this.Email);
            Comando.Parameters.AddWithValue("Senha", this.Senha);

            Int32 Resultado = Comando.ExecuteNonQuery();


            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }
        /******************************************************************** ALTERAR USUARIO ********************************************************************/
        public Boolean Alterar()
        {
            SqlConnection Conexao = new SqlConnection("Server=ESN509VMSSQL; Database=DLEstoque; User Id=Aluno; Password=Senai1234;");
            //SqlConnection Conexao = new SqlConnection("Server = DESKTOP-VQGMLTC\\SQLSERVER; Database = DLEstoque; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Usuario SET NIF = @NIF, Nome = @Nome, Sobrenome = @Sobrenome, CPF = @CPF, Cargo = @Cargo, Telefone = @Telefone, DataNascimento = @DataNascimento, Email = @Email, Senha = @Senha WHERE NIF = @NIF;";

            Comando.Parameters.AddWithValue("@NIF", this.NIF);
            Comando.Parameters.AddWithValue("@Nome", this.Nome);
            Comando.Parameters.AddWithValue("@Sobrenome", this.Sobrenome);
            Comando.Parameters.AddWithValue("@CPF", this.CPF);
            Comando.Parameters.AddWithValue("@Cargo", this.Cargo);
            Comando.Parameters.AddWithValue("@Telefone", this.Telefone);
            Comando.Parameters.AddWithValue("@DataNascimento", this.DataNascimento);
            Comando.Parameters.AddWithValue("@Email", this.Email);
            Comando.Parameters.AddWithValue("@Senha", this.Senha);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** ALTERAR USUARIO ********************************************************************/
        public List<Usuario> ListarUsuarios()
        {
            SqlConnection Conexao = new SqlConnection("Server=ESN509VMSSQL; Database=DLEstoque; User Id=Aluno; Password=Senai1234;");
            //SqlConnection Conexao = new SqlConnection("Server = DESKTOP-VQGMLTC\\SQLSERVER; Database = DLEstoque; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Usuario";

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Usuario> Usuarios = new List<Usuario>();

            while (Leitor.Read())
            {

                Usuario U = new Usuario();

                U.NIF = Convert.ToInt32(Leitor["NIF"].ToString());
                U.Nome = Leitor["Nome"].ToString();
                U.Sobrenome = Leitor["Sobrenome"].ToString();
                U.CPF = Leitor["CPF"].ToString();
                U.Cargo = Leitor["Cargo"].ToString();
                U.Telefone = Leitor["Telefone"].ToString();
                U.DataNascimento = Convert.ToDateTime(Leitor["DataNascimento"].ToString());
                U.Email = Leitor["Email"].ToString();
                U.Senha = Leitor["Senha"].ToString();

                Usuarios.Add(U);
            }

            Conexao.Close();

            return Usuarios;
        }
    }
}