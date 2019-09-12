using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Estoque.Models
{
    public class Requisicao
    {
        //Variaveis Item_Empresa
        public Int32 FK_CodigoEmpresa { get; set; }
        public String NomeEmpresa { get; set; }

        //Variaveis Orçamento-Item
        public Int32 CodigoitemOrcamento { get; set; }
        public Int32 FK_CodigoItem { get; set; }
        public Int32 FK_CodigoOrcamento { get; set; }

        //Variaveis Orçamento
        public Int32 CodigoOrcamento { get; set; }
        public Double ValorUnitario { get; set; }
        public Double ValorTotal { get; set; }

        // Variaveis Status
        public Int32 CodigoStatus { get; set; }
        public String NomeStatus { get; set; }

        // Variaveis TipoRequisicao
        public Int32 CodigoTipoRequisicao { get; set; }
        public String NomeTipoRequisicao { get; set; }

        // Variaveis HistoricoRequisicao
        public Int32 CodigoHistoricoRequisicao { get; set; }
        public String DataEntrada { get; set; }

        //Variavei Requisicao
        public Int32 CodigoRequisicao { get; set; }
        public DateTime DataRequisicao { get; set; }
        public String CentroCusto { get; set; }
        public Int32 ContaContabil { get; set; }
        public Int32 ContaMemo { get; set; }
        public Int32 FK_NIFUsuario { get; set; }
        public Int32 FK_TipoRequisicao { get; set; }
        public Int32 FK_CodigoStatus { get; set; }


        //VARIAVEIS ADD ITEM
        public Int32 FK_CodigoRequisicao { get; set; }
        public Int32 Codigoitem { get; set; }
        public String Especificacao { get; set; }
        public String Unidade { get; set; }
        public Int32 QtdItem { get; set; }
        public String Finalidade { get; set; }

        //OUTRAS 
        List<Requisicao> Orcamentos = new List<Requisicao>();
        public List<Requisicao> Valores = new List<Requisicao>();
        Int32[] Valor = new Int32[10];
        public String Fone { get; set; }
        public String Contato { get; set; }

        /******************************************************************** METODOS CONSTRUTORES ********************************************************************/
        public Requisicao() { }


        /******************************************************************** LISTAR TIPO REQUISIÇÃO ********************************************************************/
        public List<Requisicao> ListarTipoRequisicao()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT * FROM TipoRequisicao;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Requisicao> TiposRequisicao = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();
                R.CodigoTipoRequisicao = Convert.ToInt32(Leitor["CodigoTipoRequisicao"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();

                TiposRequisicao.Add(R);
            }

            Conexao.Close();
            return TiposRequisicao;
        }

        /******************************************************************** CADASTRAR REQUISIÇÃO ********************************************************************/
        public Boolean CadastrarRequisicao()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Requisicao (FK_NIFUsuario, FK_TipoRequisicao, FK_CodigoStatus, CentroCusto, ContaMemo, DataRequisicao, ContaContabil) OUTPUT Inserted.CodigoRequisicao AS CodigoRequisicao VALUES (@FK_NIFUsuario, @FK_TipoRequisicao, @FK_CodigoStatus, @CentroCusto, @ContaMemo, @DataRequisicao, @ContaContabil);";
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", this.FK_NIFUsuario);
            Comando.Parameters.AddWithValue("@FK_TipoRequisicao", this.FK_TipoRequisicao);
            Comando.Parameters.AddWithValue("@FK_CodigoStatus", this.FK_CodigoStatus);
            Comando.Parameters.AddWithValue("@CentroCusto", this.CentroCusto);
            Comando.Parameters.AddWithValue("@ContaMemo", this.ContaMemo);
            Comando.Parameters.AddWithValue("@DataRequisicao", this.DataRequisicao);
            Comando.Parameters.AddWithValue("@ContaContabil", this.ContaContabil);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            this.CodigoHistoricoRequisicao = Leitor.GetInt32(0);

            Int32 Resultado = Leitor.GetInt32(0);

            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "INSERT INTO HistoricoRequisicao (DataEntrada, FK_CodigoRequisicao) VALUES (@DataEntrada, @FK_CodigoRequisicao);";
            Comando.Parameters.AddWithValue("@DataEntrada", this.DataRequisicao);
            Comando.Parameters.AddWithValue("@FK_CodigoRequisicao", this.CodigoHistoricoRequisicao);

            Int32 Resultado2 = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** LISTAR REQUISICAO - SOMENTE DO USUARIO LOGADO ********************************************************************/
        public List<Requisicao> ListarRequisicao(Object NIF)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "select R.CodigoRequisicao,R.FK_NIFUsuario, TR.NomeTipoRequisicao,S.NomeStatus,R.DataRequisicao from Requisicao R join TipoRequisicao TR on CodigoTipoRequisicao = FK_TipoRequisicao join Status S on CodigoStatus = FK_CodigoStatus where FK_NIFUsuario = @FK_NIFUsuario; ";
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", NIF);

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> ListaRequisicao = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
                R.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());
                ListaRequisicao.Add(R);
            }

            Conexao.Close();
            return ListaRequisicao;
        }

        /******************************************************************** LISTAR TODAS AS REQUISICÕES - STATUS PENDENTE ********************************************************************/
        public List<Requisicao> Requisicoes()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "select R.CodigoRequisicao,R.FK_NIFUsuario, TR.NomeTipoRequisicao,S.NomeStatus,R.DataRequisicao from Requisicao R join TipoRequisicao TR on CodigoTipoRequisicao = FK_TipoRequisicao join Status S on CodigoStatus = FK_CodigoStatus WHERE NomeStatus = 'Pendente'";

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> ListaRequisicao = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
                R.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());
                ListaRequisicao.Add(R);
            }

            Conexao.Close();
            return ListaRequisicao;
        }

        /******************************************************************** LISTAR TODAS AS REQUISICÕES - STATUS APROVADO PELO COORDENADOR ********************************************************************/
        public List<Requisicao> ListarOrcamentos()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT R.CodigoRequisicao, TR.NomeTipoRequisicao, S.NomeStatus, R.DataRequisicao from Requisicao R join TipoRequisicao TR on CodigoTipoRequisicao = FK_TipoRequisicao join Status S on CodigoStatus = FK_CodigoStatus WHERE NomeStatus = 'Aprovada pelo Coordenador'";

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> Orcamentos = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());
                Orcamentos.Add(R);
            }
            Conexao.Close();
            return Orcamentos;
        }

        /******************************************************************** ADICIONAR ITEM NA REQUISIÇÃO ********************************************************************/
        public Boolean AddItem(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Item (FK_CodigoRequisicao, Especificacao, Unidade, QtdItem, Finalidade) VALUES (@FK_CodigoRequisicao, @Especificacao, @Unidade, @QtdItem, @Finalidade);";

            Comando.Parameters.AddWithValue("@FK_CodigoRequisicao", ID);
            Comando.Parameters.AddWithValue("@Especificacao", this.Especificacao);
            Comando.Parameters.AddWithValue("@Unidade", this.Unidade);
            Comando.Parameters.AddWithValue("@QtdItem", this.QtdItem);
            Comando.Parameters.AddWithValue("@Finalidade", this.Finalidade);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** DETALHES REQUISIÇÃO ********************************************************************/
        public List<Requisicao> DetalhesRequisicao(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "select R.CodigoRequisicao, R.FK_NIFUsuario, R.CentroCusto, R.ContaMemo, R.ContaContabil, R.DataRequisicao, TR.NomeTipoRequisicao,S.NomeStatus from Requisicao R  join TipoRequisicao TR on CodigoTipoRequisicao = FK_TipoRequisicao join Status S on CodigoStatus = FK_CodigoStatus WHERE CodigoRequisIcao = @CodigoRequiscao";
            Comando.Parameters.AddWithValue("@CodigoRequiscao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> DetalhesRequisicao = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisIcao"].ToString());
                R.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                R.CentroCusto = Leitor["CentroCusto"].ToString();
                R.ContaMemo = Convert.ToInt32(Leitor["ContaMemo"].ToString());
                R.ContaContabil = Convert.ToInt32(Leitor["ContaContabil"].ToString());
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                DetalhesRequisicao.Add(R);
            }
            Conexao.Close();
            return DetalhesRequisicao;
        }

        /******************************************************************** DETALHES ITENS ********************************************************************/
        public List<Requisicao> Detalhesitens(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "select CodigoItem,Especificacao,Unidade,QtdItem,Finalidade from Item WHERE FK_CodigoRequisicao = @FK_CodigoRequisicao";
            Comando.Parameters.AddWithValue("@FK_CodigoRequisicao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> DetalhesItens = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.Codigoitem = Convert.ToInt32(Leitor["CodigoItem"].ToString());
                R.Especificacao = Leitor["Especificacao"].ToString();
                R.Unidade = Leitor["Unidade"].ToString();
                R.QtdItem = Convert.ToInt32(Leitor["QtdItem"].ToString());
                R.Finalidade = Leitor["Finalidade"].ToString();
                DetalhesItens.Add(R);
            }

            Conexao.Close();
            return DetalhesItens;
        }

        /******************************************************************** AUTORIZAR REQUISIÇÃO ********************************************************************/
        public Boolean AutorizarRequisicao(Object NIF, String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Requisicao SET FK_CodigoStatus =  @FK_CodigoStatus WHERE CodigoRequisicao = @Codigorequiscao";
            Comando.Parameters.AddWithValue("@FK_CodigoStatus", this.CodigoStatus);
            Comando.Parameters.AddWithValue("@Codigorequiscao", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "INSERT INTO Assinatura (FK_NIFUsuario, FK_CodigoRequisicao) VALUES (@FK_NIFUsuario, @FK_CodigoRequisicao);";
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", NIF);
            Comando.Parameters.AddWithValue("@FK_CodigoRequisicao", ID);

            Int32 Resultado2 = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** VERIFACAR REQUISIÇÃO ********************************************************************/
        public Boolean VerificarRequisicao(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT CodigoRequisicao FROM requisicao WHERE FK_CodigoStatus = 6 AND CodigoRequisicao = @CodigoRequisicao;";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();
            try
            {
                CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
            }
            catch
            {
                CodigoRequisicao = 0;
            }

            Conexao.Close();
            return (CodigoRequisicao > 0) ? true : false;
        }

        /******************************************************************** LISTAR ITENS ********************************************************************/
        public List<Requisicao> ListarItens(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT CodigoItem, Especificacao, Unidade,QtdItem, Finalidade FROM Item WHERE FK_CodigoRequisicao = @FK_CodigoRequisicao;";
            Comando.Parameters.AddWithValue("@FK_CodigoRequisicao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> ListaItens = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();
                R.Codigoitem = Convert.ToInt32(Leitor["CodigoItem"].ToString());
                R.Especificacao = Leitor["Especificacao"].ToString();
                R.Unidade = Leitor["Unidade"].ToString();
                R.QtdItem = Convert.ToInt32(Leitor["QtdItem"].ToString());
                R.Finalidade = Leitor["Finalidade"].ToString();

                ListaItens.Add(R);
            }

            Conexao.Close();
            return ListaItens;
        }

        /******************************************************************** FAZER ORÇAMENTO ********************************************************************/
        public Boolean FazerOrcamento(String CodigoItem)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "INSERT INTO Orcamento (FK_CodigoEmpresa, Unitario, Total) OUTPUT Inserted.CodigoOrcamento  VALUES (@CodigoEmpresa, @Unitario, @Total);";
            Comando.Parameters.AddWithValue("@CodigoEmpresa", this.FK_CodigoEmpresa);
            Comando.Parameters.AddWithValue("@Unitario", this.ValorUnitario);
            Comando.Parameters.AddWithValue("@Total", this.ValorTotal);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();
            this.FK_CodigoOrcamento = Leitor.GetInt32(0);
            Int32 Resultado = Leitor.GetInt32(0);

            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "INSERT INTO Item_Orcamento (FK_CodigoItem, FK_CodigoOrcamento) VALUES (@FK_CodigoItem, @FK_CodigoOrcamento);";
            Comando.Parameters.AddWithValue("@FK_CodigoItem", CodigoItem);
            Comando.Parameters.AddWithValue("@FK_CodigoOrcamento", this.FK_CodigoOrcamento);

            Int32 Resultado2 = Comando.ExecuteNonQuery();
            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "INSERT INTO Item_Empresa (FK_CodigoItem, FK_CodigoEmpresa) VALUES (@FK_CodigoItem2, @FK_CodigoEmpresa2);";
            Comando.Parameters.AddWithValue("@FK_CodigoItem2", CodigoItem);
            Comando.Parameters.AddWithValue("@FK_CodigoEmpresa2", this.FK_CodigoEmpresa);

            Int32 Resultado3 = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0 && Resultado3 > 0) ? true : false;
        }

        /********************************************************************  ALTERAR STATUS - STATUS ANÁLISE DE COMPRA ********************************************************************/
        public Boolean AnaliseCompra(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Requisicao SET FK_CodigoStatus = 3 WHERE CodigoRequisicao = @CodigoRequisicao;";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** VERIFICAR STATUS - ANALISE DE COMPRA ********************************************************************/
        public Int32 VerificarStatus(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT FK_CodigoStatus FROM Requisicao WHERE CodigoRequisicao = @CodigoRequisicao;";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Int32 Codigo;

            try
            {
                Leitor.Read();
                Codigo = Convert.ToInt32(Leitor["FK_CodigoStatus"].ToString());
            }
            catch
            {
                Codigo = 0;
            }
            Conexao.Close();
            return Codigo;
        }

        /********************************************************************  LISTAR REQUISIÇÃO - NÍVEL DIRETOR ********************************************************************/
        public List<Requisicao> ListarRequisicaoD()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT R.CodigoRequisicao,R.FK_NIFUsuario,TR.NomeTipoRequisicao, S.NomeStatus,R.DataRequisicao from Requisicao R join TipoRequisicao TR on FK_TipoRequisicao = CodigoTipoRequisicao join Status S on FK_CodigoStatus = CodigoStatus WHERE CodigoStatus = 3;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Requisicao> Requisicoes = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
                R.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());

                Requisicoes.Add(R);
            }
            Conexao.Close();
            return Requisicoes;
        }

        /********************************************************************  DETALHES REQUISIÇÃO - NÍVEL DIRETOR ********************************************************************/
        public List<Requisicao> DetalhesGeral(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "select R.CodigoRequisicao,R.FK_NIFUsuario,TR.NomeTipoRequisicao, S.NomeStatus, R.CentroCusto,R.ContaContabil,R.DataRequisicao from Requisicao R join TipoRequisicao TR on FK_TipoRequisicao = CodigoTipoRequisicao join Status S on FK_CodigoStatus = CodigoStatus where CodigoRequisicao = @CodigoRequisicao";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> ListaRequisicao = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();
                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
                R.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                R.CentroCusto = Leitor["CentroCusto"].ToString();
                R.ContaContabil = Convert.ToInt32(Leitor["ContaContabil"].ToString());
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());
                ListaRequisicao.Add(R);
            }
            Conexao.Close();
            return ListaRequisicao;
        }

        /********************************************************************  PEGA CODIGO DO ITEM ********************************************************************/
        public Int32[] PegaCodigoItem(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "select CodigoItemOrcamento from Item_Orcamento where FK_CodigoItem = @CodigoItem;";
            Comando.Parameters.AddWithValue("@CodigoItem", ID);

            SqlDataReader leitor = Comando.ExecuteReader();
            try
            {
                Int32 I = 0;
                while (leitor.Read())
                {
                    Requisicao R = new Requisicao();
                    Valor[I] = R.CodigoitemOrcamento = Convert.ToInt32(leitor["CodigoItemOrcamento"].ToString());
                    I++;
                   
                }
               
            }
            catch
            {
                this.CodigoitemOrcamento = 0;
            }

            Conexao.Close();
            return Valor;
        }

        /********************************************************************  VER ORÇAMENTOS - NÍVEL DIRETOR ********************************************************************/
        public List<Requisicao> VerOrcamentos(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "select FK_CodigoEmpresa, E.NomeEmpresa, E.Fone, E.Contato from Item_Empresa join Empresa E on CodigoEmpresa = FK_CodigoEmpresa where FK_CodigoItem = @FK_CodigoItem;";
            Comando.Parameters.AddWithValue("@FK_CodigoItem", ID);

            SqlDataReader Leitor2 = Comando.ExecuteReader();
            Int32[] teste  = PegaCodigoItem(ID);
            Int32 N = 0;
            Int32 numero;

            while (Leitor2.Read())
            {
                Requisicao R = new Requisicao();

                R.FK_CodigoEmpresa = Convert.ToInt32(Leitor2["FK_CodigoEmpresa"].ToString());
                R.NomeEmpresa = Leitor2["NomeEmpresa"].ToString();
                R.Fone = Leitor2["Fone"].ToString();
                R.Contato = Leitor2["Contato"].ToString();
                Orcamentos.Add(R);
                numero = teste[N];
                Pegavalores(numero);
                N++;
            }

            Conexao.Close();
            return Orcamentos;
        }

        /********************************************************************  LISTAR VALORES DO ORÇAMENO DO METODO ACIMA 'VerOrcamentos' - NÍVEL DIRETOR ********************************************************************/
        public void Pegavalores(Int32 ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
           

                Comando.CommandText = "select  I_O.*, O.FK_CodigoEmpresa, O.Unitario, O.Total from Item_Orcamento I_O join Orcamento O on CodigoOrcamento = FK_CodigoOrcamento where CodigoItemOrcamento = @CodigoItemOrcamento";
                Comando.Parameters.AddWithValue("@CodigoItemOrcamento", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            while (Leitor.Read())
                {
                    Requisicao R = new Requisicao();
                    R.FK_CodigoEmpresa = Convert.ToInt32(Leitor["FK_CodigoEmpresa"].ToString());
                    R.ValorUnitario = Convert.ToDouble(Leitor["Unitario"].ToString());
                    R.ValorTotal = Convert.ToDouble(Leitor["Total"].ToString());
                    Valores.Add(R);
                }

            Conexao.Close();
        }

        /******************************************************************** APROVAR REQUISIÇÃO ********************************************************************/
        public Boolean AprovarRequisicao(String ID, Object NIF)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Requisicao SET FK_CodigoStatus = 4 WHERE CodigoRequisicao = @CodigoRequisicao;";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "INSERT INTO Assinatura (FK_NIFUsuario, FK_CodigoRequisicao) VALUES (@FK_NIFUsuario, @FK_CodigoRequisicao);";
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", NIF);
            Comando.Parameters.AddWithValue("@FK_CodigoRequisicao", ID);

            Int32 Resultado2 = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** CANCELAR REQUISIÇÃO ********************************************************************/
        public Boolean NegarRequisicao(String ID, Object NIF)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "UPDATE Requisicao SET FK_CodigoStatus = 5 WHERE CodigoRequisicao = @CodigoRequisicao;";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            Conexao.Open();

            Comando.CommandText = "INSERT INTO Assinatura (FK_NIFUsuario, FK_CodigoRequisicao) VALUES (@FK_NIFUsuario, @FK_CodigoRequisicao);";
            Comando.Parameters.AddWithValue("@FK_NIFUsuario", NIF);
            Comando.Parameters.AddWithValue("@FK_CodigoRequisicao", ID);

            Int32 Resultado2 = Comando.ExecuteNonQuery();
            Conexao.Close();

            return (Resultado > 0 && Resultado2 > 0) ? true : false;
        }

        /******************************************************************** NOTIFICAÇÃO DIRETOR ********************************************************************/
        public Int32 NotificacaoDiretor()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT COUNT(CodigoRequisicao) AS Contador FROM Requisicao WHERE FK_CodigoStatus = 3;";

            Int32 Contador;
            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();
            try
            {
                Contador = Convert.ToInt32(Leitor["Contador"].ToString());
            }
            catch
            {
                Contador = 0;
            }

            Conexao.Close();
            return Contador;
        }


        /******************************************************************** NOTIFICAÇÃO COORDENADOR ********************************************************************/
        public Int32 NotificacaoCoordenador()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT COUNT(CodigoRequisicao) AS Contador FROM Requisicao WHERE FK_CodigoStatus = 1;";

            Int32 Contador;
            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();
            try
            {
                Contador = Convert.ToInt32(Leitor["Contador"].ToString());
            }
            catch
            {
                Contador = 0;
            }

            Conexao.Close();
            return Contador;
        }

        /******************************************************************** HISTORICO - LISTAR REQUISIÇÕES ********************************************************************/
        public List<Requisicao> HistoricoListarRequisicao()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT R.CodigoRequisicao,R.FK_NIFUsuario,TR.NomeTipoRequisicao, S.NomeStatus,R.DataRequisicao from Requisicao R join TipoRequisicao TR on FK_TipoRequisicao = CodigoTipoRequisicao join Status S on FK_CodigoStatus = CodigoStatus;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            List<Requisicao> Requisicoes = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
                R.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());

                Requisicoes.Add(R);
            }
            Conexao.Close();
            return Requisicoes;
        }

        /******************************************************************** LISTAR REQUISIÇÕES APROVADA PELO DIRETOR - ANALISE DE COMPRA ********************************************************************/
        public List<Requisicao> ListarRequisiçõesAprovadas()
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT R.CodigoRequisicao,R.FK_NIFUsuario,TR.NomeTipoRequisicao, S.NomeStatus,R.DataRequisicao from Requisicao R join TipoRequisicao TR on FK_TipoRequisicao = CodigoTipoRequisicao join Status S on FK_CodigoStatus = CodigoStatus WHERE CodigoStatus = 4";

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Requisicao> ListaRequisicoes = new List<Requisicao>();

            while (Leitor.Read())
            {
                Requisicao R = new Requisicao();

                R.CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
                R.FK_NIFUsuario = Convert.ToInt32(Leitor["FK_NIFUsuario"].ToString());
                R.NomeTipoRequisicao = Leitor["NomeTipoRequisicao"].ToString();
                R.NomeStatus = Leitor["NomeStatus"].ToString();
                R.DataRequisicao = Convert.ToDateTime(Leitor["DataRequisicao"].ToString());
                ListaRequisicoes.Add(R);
            }

            Conexao.Close();
            return ListaRequisicoes;
        }

        /******************************************************************** TROCA STATUS DA REQUISICAO PARA PENDENTE ********************************************************************/
        public Boolean FinalizarRequisicao(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Requisicao SET FK_CodigoStatus = 1 WHERE CodigoRequisicao = @CodigoRequisicao";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            return (Resultado > 0) ? true : false;
        }

        /******************************************************************** VERIFACAR REQUISIÇÃO FINALIZADA ********************************************************************/
        public Boolean VerificaRequisicaoFinalizada(String ID)
        {
            //SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["BancoEstoque"].ConnectionString);
            SqlConnection Conexao = new SqlConnection("Server = DESKTOP-PHTQI5U\\SQLEXPRESS; Database = DRLTCC; Trusted_Connection = True;");

            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;

            Comando.CommandText = "SELECT CodigoRequisicao FROM requisicao WHERE FK_CodigoStatus = 6 AND CodigoRequisicao = @CodigoRequisicao;";
            Comando.Parameters.AddWithValue("@CodigoRequisicao", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();
            try
            {
                CodigoRequisicao = Convert.ToInt32(Leitor["CodigoRequisicao"].ToString());
            }
            catch
            {
                CodigoRequisicao = 0;
            }

            Conexao.Close();
            return (CodigoRequisicao > 0) ? true : false;
        }
    }
}