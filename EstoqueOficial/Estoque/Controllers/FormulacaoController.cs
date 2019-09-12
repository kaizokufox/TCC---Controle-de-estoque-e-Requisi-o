using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;



namespace Estoque.Controllers
{
    public class FormulacaoController : Controller
    {
        Formulacao F = new Formulacao();
        Ingrediente I = new Ingrediente();
        public Double Resultado;
        public String FormulacaoAtual;
        // GET: Formulacao

        /******************************************************************** CADASTRAR FORMULAÇÃO ********************************************************************/
        public ActionResult CadastrarFormulacao()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    if (Request.Form["nomeFormulacao"].Equals(""))
                    {
                        ViewBag.mensagemAtencao = "Campo Nome deve ser preenchido!";
                    }
                    else
                    {
                        try
                        {
                            F.NomeFormulacao = Request.Form["nomeFormulacao"].ToString();
                            F.CadastrarFormulacao();
                            ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";
                            return RedirectToAction("ListarFormulacoes", "Formulacao");
                        }
                        catch
                        {
                            ViewBag.MensagemAtencao = "Erro ao tentar alterar o nome da formulação, possivelmente o nome é muito extenso.";
                        }

                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();

        }

        /******************************************************************** ALTERAR FORMULAÇÃO ********************************************************************/
        public ActionResult AlterarFormulacao(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        F.NomeFormulacao = Request.Form["nomeFormulacao"].ToString();
                        F.AlterarFormulacao(ID);
                        ViewBag.MensagemSucesso = "Alteração realizada com sucesso!";
                        return RedirectToAction("ListarFormulacoes", "Formulacao");
                    }
                    catch
                    {
                        ViewBag.MensagemAtencao = "Erro ao tentar alterar o nome da formulação, possivelmente o nome é muito extenso.";
                    }
                    
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Formulacao = (ID == null) ? new Formulacao() : new Formulacao(ID);
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** EXCLUIR FORMULAÇÕES ********************************************************************/
        public ActionResult ExcluirFormulacao(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (!Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        F.ExcluirFormulacao(ID);
                        return RedirectToAction("ListarFormulacoes", "Formulacao");
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao Excluir a formulação!";
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTAR FORMULAÇÕES ********************************************************************/
        public ActionResult ListarFormulacoes()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                ViewBag.Formulacoes = F.ListarFormulacoes();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** FORMULAÇÕES DETALHES ********************************************************************/
        public ActionResult DetalhesFormulacao(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (!Request.HttpMethod.Equals("POST"))
                {
                    Session["@CodigoFormulacao"] = ID;
                    ViewBag.DetalhesFormulacoes = F.DetalhesFormulacao(ID);
                }
                Session["CodigoFormulacao"] = ID;
                //FormulacaoAtual = Request.QueryString["ID"];
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** CADASTRAR INGREDINETE NA FORMULAÇÃO ********************************************************************/
        public ActionResult CadastrarIngredientesF(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        F.CodigoIngrediente = Convert.ToInt32(Request.Form["codigoIngrediente"].ToString());
                        F.Porcentagem = Convert.ToDouble(Request.Form["porcentagem"].ToString());

                        F.CadastrarIngredienteF(ID);
                        ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";
                        return RedirectToAction("DetalhesFormulacao", "Formulacao", new { ID });
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao cadastrar! Verifique todos os campos.";
                    }

                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Ingredientes = I.ListarIngredientes();
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** ALTERAR INGRDIENTE DA FORMULAÇÃO ********************************************************************/
        public ActionResult AlterarIngredientesF(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        F.CodigoIngrediente = Convert.ToInt32(Request.Form["codigoIngrediente"].ToString());
                    F.Porcentagem = Convert.ToDouble(Request.Form["porcentagem"].ToString());

                    F.AlterarIngredienteF(ID, Session["CodigoFormulacao"]);
                    ViewBag.MensagemSucesso = "Alteração realizada com sucesso!";
                    return RedirectToAction("ListarFormulacoes", "Formulacao");

                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao tentar alterar! Verifique todos os campos";
                    }

                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            ViewBag.Ingredientes = I.IngredienteF();
            ViewBag.Formulacao = F.DetalhesIngrediente(ID, Session["CodigoFormulacao"]);
            return View();
        }

        /******************************************************************** EXCLUIR INGREDIENTE DA FORMULAÇÕES ********************************************************************/
        public ActionResult ExcluirIngredienteF(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (!Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        F.ExcluirIngredienteF(ID, Session["CodigoFormulacao"]);
                        return RedirectToAction("ListarFormulacoes", "Formulacao", false);
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao tentar excluir o ingrediente!";
                    }

                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }
    }
}