using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;

namespace Estoque.Controllers
{
    public class RequisicaoController : Controller
    {
        // GET: Requisicao

        public Int32 Verificado;

        /******************************************************************** CADASTRAR REQUISIÇÃO ********************************************************************/
        public ActionResult CadastrarRequisicao()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        DateTime Hora = DateTime.UtcNow;
                        TimeZoneInfo NossaZona = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                        DateTime Databrasilia = TimeZoneInfo.ConvertTimeFromUtc(Hora, NossaZona);

                        R.FK_NIFUsuario = Convert.ToInt32(Session["NIF"].ToString());
                        R.FK_TipoRequisicao = Convert.ToInt32(Request.Form["tipoRequisicao"].ToString());
                        R.FK_CodigoStatus = 6;
                        R.CentroCusto = Request.Form["centroCusto"].ToString();
                        R.ContaMemo = Convert.ToInt32(Request.Form["contaMemo"].ToString());

                        R.DataRequisicao = Databrasilia;
                        R.ContaContabil = Convert.ToInt32(Request.Form["contaContabil"].ToString());

                        if (R.CadastrarRequisicao() == true)
                        {
                            ViewBag.MensagemSucesso = "Requisição iniciada com sucesso! Vá para a pagina 'Listar Requisições' para adicionar itens na mesma.";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao iniciar requisição! verifique todos os campos, e se mesmo assim o problema persistir, entre em contato com o administrador do sistema.";
                    }

                }
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                ViewBag.TipoRequisicao = R.ListarTipoRequisicao();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTAR REQUISIÇÕES - SOMENTE DO USUARIO ********************************************************************/
        public ActionResult ListarRequisicoes()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
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
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTA DE TODAS AS REQUISICÕES ********************************************************************/
        public ActionResult Requisicoes()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                ViewBag.Requisicoes = R.Requisicoes();
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTA DE ORÇAMENTOS ********************************************************************/
        public ActionResult ListarOrcamentos()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                ViewBag.Orcamentos = R.ListarOrcamentos();
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** ADICIONAR ITENS REQUISIÇÃO ********************************************************************/
        public ActionResult AddItem(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                Session["Requisicao"] = null;

                if (R.VerificarRequisicao(ID) == false)
                {
                    Session["Requisicao"] = "Finalizada";
                    return RedirectToAction("ListarRequisicoes", "Requisicao");
                }
                else
                {
                    if (Request.HttpMethod.Equals("POST"))
                    {
                        if (Session["Requisicao"] == null)
                        {
                            try
                            {
                                R.Especificacao = Request.Form["especificacao"].ToString();
                                R.Unidade = Request.Form["unidade"].ToString();
                                R.QtdItem = Convert.ToInt32(Request.Form["qtdItem"].ToString());
                                R.Finalidade = Request.Form["finalidade"].ToString();

                                if (ViewBag.Requisicao = R.AddItem(ID) == true)
                                {
                                    ViewBag.MensagemSucesso = "item adicionado com sucesso!";
                                }
                            }
                            catch
                            {
                                ViewBag.MensagemErro = "Erro ao adicionar o item! verifique todo os campos.";
                            }
                        }
                    }
                }

                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** DETALHES REQUISIÇÃO ********************************************************************/
        public ActionResult Detalhes(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                if (Request.HttpMethod.Equals("POST"))
                {
                    if (Request.Form["termoAutorizacao"] != null && Request.Form["termoNegar"] != null)
                    {
                        ViewBag.MensagemAtencao = "Somente um CheckBox pode ser verdadeiro!";
                    }
                    else
                    {
                        AutorizarRequisicao(ID);
                    }
                }

                ViewBag.DetahesRequisicao = R.DetalhesRequisicao(ID);
                ViewBag.Detalhesitens = R.Detalhesitens(ID);
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** DETALHES ITENS ********************************************************************/
        public ActionResult DetalhesItens(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                if (Request.HttpMethod.Equals("POST"))
                {
                    if (Request.Form["finalizar"] != null)
                    {

                        if (R.FinalizarRequisicao(ID) == true)
                        {
                            ViewBag.MensagemSucesso = "Requisição Finalizada com sucesso!";
                        }
                    }
                }

                if (R.VerificaRequisicaoFinalizada(ID) == false)
                {
                    ViewBag.vericado = this.Verificado = 0;
                }
                else
                {
                    this.Verificado = 1;
                }

                ViewBag.DetahesRequisicao = R.DetalhesRequisicao(ID);
                ViewBag.Detalhesitens = R.Detalhesitens(ID);
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** AUTORIZAR REQUISIÇÃO ********************************************************************/
        public void AutorizarRequisicao(String ID)
        {
            if (Request.Form["termoAutorizacao"] != null)
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                R.CodigoStatus = 2;

                if (R.AutorizarRequisicao(Session["NIF"], ID) == true)
                {
                    ViewBag.MensagemSucesso = "Requisição autorizada com sucesso!";
                }
                else
                {
                    ViewBag.MensagemErro = "Erro ao autorizar a requisição!";
                }
            }

            if (Request.Form["termoNegar"] != null)
            {
                Requisicao R = new Requisicao();
                R.CodigoStatus = 5;

                if (R.AutorizarRequisicao(Session["NIF"], ID) == true)
                {
                    ViewBag.MensagemSucesso = "Requisição cancelada com sucesso!";
                }
                else
                {
                    ViewBag.MensagemErro = "Erro ao cancelar a requisição!";
                }
            }
        }

        /******************************************************************** LISTAR ITENS ********************************************************************/
        public ActionResult ListaItens(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                if (Request.HttpMethod.Equals("POST"))
                {
                    if (Request.Form["finalizar"] != null)
                    {
                        if (R.AnaliseCompra(ID) == true)
                        {
                            return RedirectToAction("ListarOrcamentos", "Requisicao");
                        }
                    }
                }

                ViewBag.Itens = R.ListarItens(ID);
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** FAZER ORÇAMENTO ********************************************************************/
        public ActionResult FazerOrcamento(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        Requisicao R = new Requisicao();

                        if (Session["NivelAcesso"].Equals(4))
                        {
                            Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                        }
                        else if (Session["NivelAcesso"].Equals(3))
                        {
                            Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                        }

                        if (R.VerificarStatus(ID) > 0)
                        {
                            return RedirectToAction("ListaItens", "Requisicao");
                        }
                        else
                        {
                            R.ValorUnitario = Convert.ToDouble(Request.Form["valorUnitario"].ToString());
                            R.ValorTotal = Convert.ToDouble(Request.Form["valorTotal"].ToString());
                            R.FK_CodigoEmpresa = Convert.ToInt32(Request.Form["codigoEmpresa"].ToString());

                            if (R.FazerOrcamento(ID) == true)
                            {
                                ViewBag.MensagemSucesso = "Orçamento realizado com sucesso!";
                            }
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao fazer orçamento.";
                    }

                }
                Empresa E = new Empresa();
                ViewBag.Empresas = E.ListarEmpresas();
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTA REQUISICAO - NÍVEL DIRETOR ********************************************************************/
        public ActionResult ListaRequisicaoD()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                ViewBag.ListaRequisicao = R.ListarRequisicaoD();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** DETALHES REQUISICAO - NÍVEL DIRETOR ********************************************************************/
        public ActionResult DetalhesGeral(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                Requisicao R = new Requisicao();

                if (Session["NivelAcesso"].Equals(4))
                {
                    Session["NotificacaoDiretor"] = R.NotificacaoDiretor();
                }
                else if (Session["NivelAcesso"].Equals(3))
                {
                    Session["NotificacaoCoordenador"] = R.NotificacaoCoordenador();
                }

                if (Request.HttpMethod.Equals("POST"))
                {

                    if (Request.Form["TBaprova"] != null && Request.Form["TBnegar"] != null)
                    {
                        ViewBag.MensagemAtencao = "Por favor, escolha somente uma das opções!";
                    }
                    else
                    {
                        if (Request.Form["TBaprova"] != null)
                        {
                            if (R.AprovarRequisicao(ID, Session["NIF"]) == true)
                            {
                                ViewBag.MensagemSucesso = "Requisição Aprovada com sucesso!";
                            }
                        }
                        if (Request.Form["TBnegar"] != null)
                        {
                            if (R.NegarRequisicao(ID, Session["NIF"]) == true)
                            {
                                ViewBag.MensagemSucesso = "Requisição cancelada com sucesso!";
                            }
                        }
                    }
                }
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                ViewBag.DetalhesGeral = R.DetalhesGeral(ID);
                ViewBag.Itens = R.ListarItens(ID);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** VER ORÇAMENTO - NÍVEL DIRETOR ********************************************************************/
        public ActionResult VerOrcamentos(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
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
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                ViewBag.Orcamentos = R.VerOrcamentos(ID);
                ViewBag.Valores = R.Valores;
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** HISTÓRICO ********************************************************************/
        public ActionResult Historico()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
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
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                ViewBag.Historico = R.HistoricoListarRequisicao();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** HISTÓRICO DETALHES ********************************************************************/
        public ActionResult HistoricoDetalhes(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(1) || Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
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
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                ViewBag.DetalhesGeral = R.DetalhesGeral(ID);
                ViewBag.Itens = R.ListarItens(ID);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTAR REQUISIÇÕES APROVADA PELO DIRETOR - ANALISE DE COMPRA ********************************************************************/
        public ActionResult ListarRequisicoesAprovadas()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
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
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                ViewBag.ListaRequisicoesAprovadas = R.ListarRequisiçõesAprovadas();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }
    }
}