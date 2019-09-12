using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;
using System.IO;

namespace Estoque.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        public String Pesquisa { get; set; }
        
        /******************************************************************** HOME ********************************************************************/
        public ActionResult Home()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Requisicao R = new Requisicao();

            if(Session["NivelAcesso"].Equals(4)){
                Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
            }
            else if (Session["NivelAcesso"].Equals(3))
            {
                Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
            }
            
            UsuarioController UC = new UsuarioController();
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            return View();
        }

        /******************************************************************** HOME Estoque********************************************************************/
        public ActionResult HomeEstoque()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Requisicao R = new Requisicao();

            if (Session["NivelAcesso"].Equals(4))
            {
                Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
            }
            else if (Session["NivelAcesso"].Equals(3))
            {
                Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
            }

            Curso C = new Curso();
            ViewBag.Curso = C.ListarCursos(Session["NIF"].ToString());

            Turma T = new Turma();
            ViewBag.Turmas = T.ListarTurmas();

            Lote L = new Lote();
            ViewBag.Lotes = L.ListarLotes();

            Formulacao F = new Formulacao();
            ViewBag.Formulacoes = F.ListarFormulacoes();

            Ingrediente I = new Ingrediente();
            ViewBag.Ingredientes = I.ListarIngredientes();

            UsuarioController UC = new UsuarioController();
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            return View();
        }

        /******************************************************************** HOME Requisição********************************************************************/
        public ActionResult HomeRequisicao()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Requisicao R = new Requisicao();

            if (Session["NivelAcesso"].Equals(4))
            {
                Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
            }
            else if (Session["NivelAcesso"].Equals(3))
            {
                Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
            }
            
            ViewBag.ListaRequisicao = R.ListarRequisicao(Session["NIF"]);

            Requisicao RH = new Requisicao();
            ViewBag.Historico = R.HistoricoListarRequisicao();

            UsuarioController UC = new UsuarioController();
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            return View();
        }

        /******************************************************************** Suporte ********************************************************************/
        public ActionResult Suporte()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Requisicao R = new Requisicao();

            if (Session["NivelAcesso"].Equals(4))
            {
                Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
            }
            else if (Session["NivelAcesso"].Equals(3))
            {
                Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
            }

            UsuarioController UC = new UsuarioController();
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            return View();
        }

        /******************************************************************** VER PERFIL ********************************************************************/
        public ActionResult VerPerfil()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Requisicao R = new Requisicao();

            if (Session["NivelAcesso"].Equals(4))
            {
                Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
            }
            else if (Session["NivelAcesso"].Equals(3))
            {
                Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
            }

            Usuario U = new Usuario();
            ViewBag.Perfil = U.VerPerfil(Session["NIF"]);
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** PRIMEIRA VEZ ********************************************************************/
        public ActionResult PrimeiraVez()
        {
            Usuario U = new Usuario();

            if (Session["UsuarioLogado"] != null)
            {
                Session["PrimeiraVez"] = 1;
                return RedirectToAction("Home", "Home");
            }
            else if (U.VerificaUsuario() > 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Request.HttpMethod.Equals("POST"))
            {
                try
                {
                    U.NIF = Request.Form["nif"].ToString();
                    U.Nome = Request.Form["nome"].ToString();
                    U.Sobrenome = Request.Form["sobrenome"].ToString();
                    U.CPF = Request.Form["cpf"].ToString();
                    U.CodigoCargo = Convert.ToInt32(Request.Form["cargo"].ToString());
                    U.TelefoneFixo = Request.Form["telefoneFixo"].ToString();
                    U.TelefoneMovel = Request.Form["telefoneMovel"].ToString();
                    U.DataNascimento = Convert.ToDateTime(Request.Form["datanascimento"].ToString());
                    U.Email = Request.Form["email"].ToString();
                    U.Senha = Request.Form["senha"].ToString();

                    //Busca o arquivo na pasta                    
                    StreamReader StreamImagem = new StreamReader(Server.MapPath("~/images/user.png"));
                    //Converte o arquivo para binário
                    BinaryReader br = new BinaryReader(StreamImagem.BaseStream);
                    //Leitura do arquivo gerando a String em binário para passar para o banco de dados
                    UsuarioController.Binario2 = br.ReadBytes((int)StreamImagem.BaseStream.Length);

                    /**** VERIFICA SENHA *****/
                    if (U.Senha.Equals(""))
                    {
                        ViewBag.MensagemAtencao = "A senha deve ser preenchida!";
                    }
                    else if (U.Senha.Length < 6)
                    {
                        ViewBag.MensagemAtencao = "A senha deve conter no mínimo 6 caracteres!";
                    }
                    /***** AÇÃO VERIFICA USUARIO JÁ CADASTRAO NO SISTEMA *****/
                    else if (U.VerificarUsuario(U.NIF).Equals(false))
                    {
                        /***** AÇÃO CADASTRAR *****/
                        U.Senha = LoginController.CalcularSha1(U.Senha);
                        U.CadastrarUsuario();
                        ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";

                        Response.Redirect("/Login/Index", false);
                    }
                    else
                    {
                        ViewBag.MensagemSucesso = "já existe um usuário cadastrado no sistema com este NIF!";
                    }
            }
                /***** MESNAGEM DE ERRO *****/
                catch
            {
                ViewBag.MensagemErro = "Campo(s) Preenchido(s) incorretamente! Verifique todos os campos.";
            }
        }
            return View();
        }
    }
}