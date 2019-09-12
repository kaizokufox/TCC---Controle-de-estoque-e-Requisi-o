using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using Estoque.Controllers;

namespace Estoque.Models
{

    public class Usuario
    {
        // Variaveis Usuario 
        public Byte[] Binario           { get; set; }
        public String   NIF             { get; set; }
        public String   Nome            { get; set; }
        public String   Sobrenome       { get; set; }
        public String   CPF             { get; set; }
        public DateTime DataNascimento  { get; set; }
        public String   Email           { get; set; }
        public String   Senha           { get; set; }

        public String NovaSenha         { get; set;  }
        public String RepitaNovaSenha   { get; set; }

        // Variaveis Cargo
        public Int32    CodigoCargo     { get; set; }
        public String   NomeCargo       { get; set; }
        public Int32  Nivel             { get; set; }

        // Variaveis Telefone
        public String   TelefoneFixo    { get; set; }
        public String   TelefoneMovel   { get; set; }

        public Int32    NIFVerifica;

        
        /******************************************************************** METODOS CONSTRUTORES ********************************************************************/

        public Usuario() { }

        public Usuario(Int32 ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT U.*, T.TelefoneFixo,T.TelefoneMovel,C.CodigoCargo FROM Usuario U JOIN Telefone T ON T.FK_NIFUsuario = U.NIF JOIN Cargo C ON C.CodigoCargo = U.FK_CodigoCargo WHERE NIF = @NIF;";
            Comando.Parameters.AddWithValue("@NIF",ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            

            Leitor.Read();

            this.NIF                = Leitor["NIF"].ToString();
            this.Nome               = Leitor["NomeUsuario"].ToString();
            this.Sobrenome          = Leitor["Sobrenome"].ToString();
            this.CPF                = Leitor["CPF"].ToString();
            this.TelefoneFixo       = Leitor["TelefoneFixo"].ToString();
            this.TelefoneMovel      = Leitor["TelefoneMovel"].ToString();
            this.CodigoCargo        = Convert.ToInt32(Leitor["CodigoCargo"].ToString());
            this.DataNascimento     = Convert.ToDateTime(Leitor["DataNascimento"].ToString());
            this.Email              = Leitor["Email"].ToString();
            this.Senha              = Leitor["Senha"].ToString();

            if (Leitor.HasRows)
            {
                this.Binario = (Byte[])Leitor["FotoUsuario"];
            }
           
            
            Conexao.Close();
        }

        /******************************************************************** ALTERAR IMAGEM ********************************************************************/
        public Boolean AlterarImagem(Object NIF)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");
            //SqlConnection Conexao = new SqlConnection("Server=DESKTOP-USOOE2B\\RICARDOSQL; Database=DRLTCC; User Id=sa; Password=Senai1234;");
            //SqlConnection Conexao = new SqlConnection("Server=ricardoinfo.database.windows.net; Database=DRLTCC; User Id=Ricardo; Password=Ric@rdo2017;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Usuario SET FotoUsuario = @Binario WHERE NIF = @NIF;";
            Comando.Parameters.AddWithValue("@NIF", NIF);
            Comando.Parameters.AddWithValue("@Binario", this.Binario);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }


        /******************************************************************** LISTAR IMAGEM ********************************************************************/
        public static List<Int32> ListarImagem(Object NIF)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            List<Int32> Imagens = new List<Int32>();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT NIF FROM Usuario WHERE NIF = @NIF";
            Comando.Parameters.AddWithValue("@NIF", NIF);
            
            SqlDataReader Leitor = Comando.ExecuteReader();

            while (Leitor.Read())
            {
                Imagens.Add(Convert.ToInt32(Leitor["NIF"]));
            }

            Conexao.Close();

            return Imagens;
        }


        /******************************************************************** VERIFICAR USUARIO ********************************************************************/
        public Boolean VerificarUsuario(String NIF)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT NIF FROM Usuario WHERE NIF = @NIF;";
            Comando.Parameters.AddWithValue("@NIF", NIF);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            try
            {
             this.NIFVerifica = Convert.ToInt32(Leitor["NIF"].ToString());
            }
            catch
            {
              this.NIFVerifica = 0;
            }                  
            Conexao.Close();
            return (NIFVerifica > 0) ? true : false;
        }

        /******************************************************************** AUTENTICAR USUARIO ********************************************************************/
        public void Login(String NIF, String Senha)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT U.NIF, U.Senha,U.NomeUsuario, C.NivelCargo, C.NomeCargo FROM Usuario U JOIN Cargo C ON C.CodigoCargo = U.FK_CodigoCargo WHERE NIF = @NIF and Senha = @Senha;"; 
            Comando.Parameters.AddWithValue("@NIF", NIF);
            Comando.Parameters.AddWithValue("@Senha", Senha);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            this.NIF          = Leitor["NIF"].ToString();
            this.Nivel        = Convert.ToInt32(Leitor["NivelCargo"].ToString());
            this.Senha        = Leitor["Senha"].ToString();
            this.Nome         = Leitor["NomeUsuario"].ToString();
            this.NomeCargo    = Leitor["Nomecargo"].ToString();

            Leitor.Close();
            Conexao.Close();
                   
        }

        /******************************************************************** CADASTRAR USUARIO ********************************************************************/
        public Boolean CadastrarUsuario()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
                       
            Conexao.Open();
           
            //Comando.CommandText = "INSERT INTO Usuario (NIF,FK_CodigoCargo,NomeUsuario,CPF,DataNascimento,Email,Senha,Sobrenome, FotoUsuario) VALUES (@NIF,@FK_CodigoCargo,@NomeUsuario,@CPF,@DataNascimento,@Email,@Senha,@Sobrenome, Convert(varbinary(max),@foto));";
            Comando.CommandText = "INSERT INTO Usuario (NIF,FK_CodigoCargo,NomeUsuario,CPF,DataNascimento,Email,Senha,Sobrenome, FotoUsuario) VALUES (@NIF,@FK_CodigoCargo,@NomeUsuario,@CPF,@DataNascimento,@Email,@Senha,@Sobrenome,@foto);";

            Comando.Parameters.AddWithValue("@NIF",              this.NIF);
            Comando.Parameters.AddWithValue("@FK_CodigoCargo",   this.CodigoCargo);
            Comando.Parameters.AddWithValue("@NomeUsuario",      this.Nome);
            Comando.Parameters.AddWithValue("@Sobrenome",        this.Sobrenome);
            Comando.Parameters.AddWithValue("@CPF",              this.CPF);
            Comando.Parameters.AddWithValue("@DataNascimento",   this.DataNascimento);
            Comando.Parameters.AddWithValue("@Email",            this.Email);
            Comando.Parameters.AddWithValue("@Senha",            this.Senha);
            Comando.Parameters.AddWithValue("@foto",             UsuarioController.Binario2);


            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            Conexao.Open();
            Comando.CommandText = "INSERT INTO Telefone (FK_NIFUsuario, TelefoneFixo, TelefoneMovel) VALUES (@FK_NIFUsuario, @TelefoneFixo, @TelefoneMovel);";

            Comando.Parameters.AddWithValue("@FK_NIFUsuario", this.NIF);
            Comando.Parameters.AddWithValue("@TelefoneFixo",  this.TelefoneFixo);
            Comando.Parameters.AddWithValue("@TelefoneMovel", this.TelefoneMovel);

            Int32 Resultado2 = Comando.ExecuteNonQuery();

            Conexao.Close();
            return (Resultado > 0 && Resultado2 > 0) ? true : false;

        }
        /******************************************************************** ALTERAR USUARIO ********************************************************************/
        public Boolean Alterar()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Usuario SET NIF = @NIF,FK_CodigoCargo = @FK_CodigoCargo, NomeUsuario = @NomeUsuario, Sobrenome = @Sobrenome, CPF = @CPF, DataNascimento = @DataNascimento, Email = @Email, Senha = @Senha WHERE NIF = @NIF;";

            Comando.Parameters.AddWithValue("@NIF",                 this.NIF);
            Comando.Parameters.AddWithValue("@FK_CodigoCargo",      this.CodigoCargo);
            Comando.Parameters.AddWithValue("@NomeUsuario",         this.Nome);
            Comando.Parameters.AddWithValue("@Sobrenome",           this.Sobrenome);
            Comando.Parameters.AddWithValue("@CPF",                 this.CPF);
            Comando.Parameters.AddWithValue("@DataNascimento",      this.DataNascimento);
            Comando.Parameters.AddWithValue("@Email",               this.Email);
            Comando.Parameters.AddWithValue("@Senha",               this.Senha);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            Conexao.Open();
            Comando.CommandText = "UPDATE Telefone SET FK_NIFUsuario = @FK_NIFUsuario, TelefoneFixo = @TelefoneFixo, TelefoneMovel = @TelefoneMovel WHERE FK_NIFUsuario = @FK_NIFUsuario;";

            Comando.Parameters.AddWithValue("FK_NIFUsuario", this.NIF);
            Comando.Parameters.AddWithValue("TelefoneFixo",  this.TelefoneFixo);
            Comando.Parameters.AddWithValue("TelefoneMovel", this.TelefoneMovel);

            Int32 Resultado2 = Comando.ExecuteNonQuery();

            Conexao.Close();
            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** LISTAR USUARIO ********************************************************************/
        public List<Usuario> ListarUsuarios()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText =   "SELECT U.NIF, U.NomeUsuario, U.Sobrenome, C.NomeCargo, U.Email " +
                                    "FROM Usuario U " +
                                    "JOIN Cargo C on C.CodigoCargo = U.FK_CodigoCargo";

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Usuario> Usuarios = new List<Usuario>();

            while (Leitor.Read())
            {
                Usuario U = new Usuario();
                
                U.NIF               = Leitor["NIF"].ToString();
                U.Nome              = Leitor["NomeUsuario"].ToString();
                U.Sobrenome         = Leitor["Sobrenome"].ToString();
                U.NomeCargo         = Leitor["NomeCargo"].ToString();
                U.Email             = Leitor["Email"].ToString();
             
                Usuarios.Add(U);
            }
            Conexao.Close();
            return Usuarios;
        }

        /******************************************************************** LISTAR CARGO ********************************************************************/
        public List<Usuario> ListarCargos()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Cargo";

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Usuario> Cargos = new List<Usuario>();

            while (Leitor.Read())
            {
                Usuario U = new Usuario();

                U.CodigoCargo = Convert.ToInt32(Leitor["CodigoCargo"].ToString());
                U.NomeCargo = Leitor["NomeCargo"].ToString();

                Cargos.Add(U);
            }
            Conexao.Close();
            return Cargos;
        }
        /******************************************************************** REMOVER USUARIO ********************************************************************/
        public Boolean Remover(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "DELETE FROM Telefone WHERE FK_NIFUsuario = @FK_NIFUsuario";
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();
            
            Conexao.Open();

            Comando.CommandText = "DELETE FROM Usuario WHERE NIF = @NIF";
            Comando.Parameters.AddWithValue("@NIF", ID);

            Int32 Resultado2 = Comando.ExecuteNonQuery();
                       
            Conexao.Close();
            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** VERIFICA USUARIO ********************************************************************/
        public Int32 VerificaUsuario()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Usuario;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            try
            {
                this.NIF = Leitor["NIF"].ToString();
            }
            catch
            {
                this.NIF = "0";
            }
           

            Leitor.Close();
            Conexao.Close();

            return Convert.ToInt32(NIF);
        }

        /******************************************************************** RECUPERAR SENHA ********************************************************************/
        public Int32 RecuperarSenha()
        {
            Int32 Resultado = 0;

            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM Usuario WHERE NIF = @NIF AND CPF = @CPF AND DataNascimento = @DataNascimento AND Email = @Email;";
            Comando.Parameters.AddWithValue("@NIF", this.NIF);
            Comando.Parameters.AddWithValue("@CPF", this.CPF);
            Comando.Parameters.AddWithValue("@DataNascimento", this.DataNascimento);
            Comando.Parameters.AddWithValue("@Email", this.Email);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            try
            {
                if (!Leitor.IsDBNull(0))
                {
                    Usuario U = new Usuario();

                    U.NIF = Leitor["NIF"].ToString();
                    U.CPF = Leitor["CPF"].ToString();
                    U.DataNascimento = Convert.ToDateTime(Leitor["DataNascimento"].ToString());
                    U.Email = Leitor["Email"].ToString();

                    Resultado = 1;
                }
            }
            catch
            {
                Resultado = 0;
            }

            Conexao.Close();
            return Resultado;
        }

        /******************************************************************** ALTERAR SENHA ********************************************************************/
        public Boolean AlterarSenha(Object ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Usuario SET Senha = @NovaSenha WHERE NIF = @NIF";
            Comando.Parameters.AddWithValue("@NovaSenha", this.NovaSenha);
            Comando.Parameters.AddWithValue("@NIF", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            return (Resultado > 0) ? true : false;
        }


        /******************************************************************** VER PERFIL ********************************************************************/
        public List<Usuario> VerPerfil(Object ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT U.*, C.NomeCargo,T.TelefoneFixo, T.telefoneMovel FROM Usuario U JOIN Cargo C on C.CodigoCargo = U.FK_CodigoCargo JOIN Telefone T on FK_NIFUsuario = NIF WHERE NIF = @NIF";

            Comando.Parameters.AddWithValue("@NIF", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            List<Usuario> Perfil = new List<Usuario>();

            Usuario U = new Usuario();

            U.NIF = Leitor["NIF"].ToString();
            U.NomeCargo = Leitor["NomeCargo"].ToString();
            U.Nome = Leitor["NomeUsuario"].ToString();
            U.Sobrenome = Leitor["Sobrenome"].ToString();
            U.CPF = Leitor["CPF"].ToString();
            U.DataNascimento = Convert.ToDateTime(Leitor["DataNascimento"].ToString());
            U.Email = Leitor["Email"].ToString();
            U.Senha = Leitor["Senha"].ToString();
            U.TelefoneFixo = Leitor["TelefoneFixo"].ToString();
            U.TelefoneMovel = Leitor["TelefoneMovel"].ToString();

            Perfil.Add(U);

            Conexao.Close();
            return Perfil;
        }

        /******************************************************************** ALTERAR DADOS PESSOAIS ********************************************************************/
        public Boolean AlterarDadosPessoais(Object ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Usuario SET NIF = @NIF, NomeUsuario = @NomeUsuario, CPF = @CPF, DataNascimento = @DataNascimento, Sobrenome = @Sobrenome WHERE NIF = @NIF";

            Comando.Parameters.AddWithValue("@NIF", ID);
            Comando.Parameters.AddWithValue("@NomeUsuario", this.Nome);
            Comando.Parameters.AddWithValue("@CPF", this.CPF);
            Comando.Parameters.AddWithValue("@DataNascimento", this.DataNascimento);
            Comando.Parameters.AddWithValue("@Sobrenome", this.Sobrenome);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** LISTAR DADOS PESSOAIS ********************************************************************/
        public List<Usuario> ListarDadospessoais(Object ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT NIF,NomeUsuario,Sobrenome,CPF,Datanascimento,Email, T.telefoneMovel, T.TelefoneFixo FROM Usuario JOIN Telefone T on FK_NIFUsuario = NIF WHERE NIF = @NIF";

            Comando.Parameters.AddWithValue("@NIF", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Usuario> DadosPessoais = new List<Usuario>();

            Usuario U = new Usuario();
            while (Leitor.Read())
            {
                U.NIF = Leitor["NIF"].ToString();
                U.Nome = Leitor["NomeUsuario"].ToString();
                U.Sobrenome = Leitor["Sobrenome"].ToString();
                U.CPF = Leitor["CPF"].ToString();
                U.DataNascimento = Convert.ToDateTime(Leitor["DataNascimento"].ToString());
                U.Email = Leitor["Email"].ToString();
                U.TelefoneFixo = Leitor["TelefoneFixo"].ToString();
                U.TelefoneMovel = Leitor["TelefoneMovel"].ToString();

                DadosPessoais.Add(U);
            }

            Conexao.Close();
            return DadosPessoais;
        }

        /******************************************************************** ALTERAR DADOS DE CONTATO ********************************************************************/

        public Boolean AlterarContato(Object ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Usuario SET Email = @Email WHERE NIF = @NIF";

            Comando.Parameters.AddWithValue("@NIF", ID);
            Comando.Parameters.AddWithValue("@Email", this.Email);
            
            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "UPDATE Telefone SET TelefoneFixo = @TelefoneFixo, TelefoneMovel = @TelefoneMovel WHERE FK_NIFUsuario = @FK_NIFUsuario";

            Comando.Parameters.AddWithValue("@FK_NIFUsuario", ID);
            Comando.Parameters.AddWithValue("@TelefoneFixo", this.TelefoneFixo);
            Comando.Parameters.AddWithValue("@TelefoneMovel", this.TelefoneMovel);


            Int32 Resultado2 = Comando.ExecuteNonQuery();

            Conexao.Close();
            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }
    }
}