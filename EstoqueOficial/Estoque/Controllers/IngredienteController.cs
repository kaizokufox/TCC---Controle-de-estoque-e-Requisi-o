using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;

namespace Estoque.Controllers
{
    public class IngredienteController : Controller
    {
        Ingrediente I = new Ingrediente();
        Lote L = new Lote();
        LoteController LC = new LoteController();

        public Int32 calculo { get; set; }
        public Int32 teste { get; set; }
        public Int32 quantidade { get; set; }
        public Int32 Risco { get; set; }
        public Int32 Total { get; set; }


        public String Pesquisa;
        // GET: Ingrediente

        /******************************************************************** CADASTRAR INGREDIENTE ********************************************************************/
        public ActionResult CadastrarIngrediente(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    if (ID == null)
                    {
                        L.SelecionaLote(Session["NomeLote"].ToString());
                        L.VerificaLote(L.CodigoLote.ToString());
                    }
                    else
                    {
                        L.VerificaLote(ID);
                    }

                    if (L.Contador < L.Quantidadeitens)
                    {
                        try
                        {
                            I.NomeIngrediente = Request.Form["nomeIngrediente"].ToString();
                            I.QtdIngrediente = Convert.ToInt32(Request.Form["QtdIngrediente"].ToString());

                            if (Request.Form["unidadeMedida"].Equals("Selecione uma opção"))
                            {
                                ViewBag.MensagemAtencao = "Selecione uma unidade de medida para continuar.";
                            }
                            else
                            {
                                I.UnidadeMedida = Request.Form["unidadeMedida"].ToString();
                                I.NivelRisco = Convert.ToInt32(Request.Form["nivelRisco"].ToString());

                                if (ID == null)
                                {
                                    I.CadastrarIngrediente(L.CodigoLote.ToString());
                                    Session["CodigoLote"] = ID;
                                    String CodigoLote = Session["CodigoLote"].ToString();
                                    Session["CodigoLote"] = null;
                                    ViewBag.MensagemmSucesso = "Cadastro realizado com sucesso!";
                                    return RedirectToAction("ListarLotes", "Lote", CodigoLote);
                                }
                                else
                                {
                                    I.CadastrarIngrediente(ID);
                                    Session["CodigoLote"] = ID;
                                    String CodigoLote = Session["CodigoLote"].ToString();
                                    Session["CodigoLote"] = null;
                                    ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";
                                    return RedirectToAction("ListarLotes", "Lote", CodigoLote);
                                }
                            }
                        }
                        catch
                        {
                            ViewBag.MensagemErro = "Erro ao tentar cadastrar! Verifique todos os campos.";
                        }
                    }
                    else
                    {
                        ViewBag.MensagemAtencao = "Número máximo de itens deste lote atigido! Tente alterar a quantidade de itens deste lote, ou crie um novo lote.";
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

        /******************************************************************** ALTERAR INGREDIENTE ********************************************************************/
        public ActionResult AlterarIngrediente(String ID)
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
                        I.CodigoIngrediente = Convert.ToInt32(ID.ToString());
                        I.NomeIngrediente = Request.Form["nomeIngrediente"].ToString();
                        I.QtdIngrediente = Convert.ToInt32(Request.Form["QtdIngrediente"].ToString());
                        I.NivelRisco = Convert.ToInt32(Request.Form["nivelRisco"].ToString());

                        if (Request.Form["unidadeMedida"].Equals("Selecione uma opção"))
                        {
                            ViewBag.MensagemAtencao = "Selecione uma unidade de medida para continuar.";
                        }
                        else
                        {
                            I.UnidadeMedida = Request.Form["unidadeMedida"].ToString();
                            I.AlterarIngrediente(ID);
                            ViewBag.MensagemSucesso = "Alterado com Sucesso!";
                            Response.Redirect("/Lote/DetalhesLote/" + LC.LoteAtual, false);
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao tentar alterar! Verifique todos os campos.";
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Ingrediente = (ID == null) ? new Ingrediente() : new Ingrediente(ID);
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** EXCLUIR INGREDIENTE ********************************************************************/
        public ActionResult ExcluirIngrediente(String ID)
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
                        I.ExcluirIngrediente(ID);
                        String CodigoLote = Session["CodigoLote"].ToString();
                        return RedirectToAction("DetalhesLote", "Lote", LC.LoteAtual);
                    }
                    catch
                    {
                        //
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

        /******************************************************************** LISTAR INGREDIENTE ********************************************************************/
        public ActionResult ListarIngredientes()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                ViewBag.Ingredientes = I.ListarIngredientes();

            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** RETIRAR INGREDIENTE ********************************************************************/
        public ActionResult RetirarIngrediente(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    I.SelecionaIngrediente(ID);

                    try
                    {
                        DateTime Hora = DateTime.UtcNow;
                        TimeZoneInfo NossaZona = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                        DateTime Databrasilia = TimeZoneInfo.ConvertTimeFromUtc(Hora, NossaZona);

                        this.quantidade = Convert.ToInt32(Request.Form["quantidade"].ToString());
                        I.DataRetirada = Databrasilia;

                        if (this.quantidade.ToString().Contains("-"))
                        {
                            ViewBag.MensagemAtencao = "Caracterer Inválido" + " -";
                        }
                        else
                        {
                            if (I.QtdIngrediente >= quantidade)
                            {
                                Total = I.QtdIngrediente - quantidade;

                                if (I.RetirarIngrediente(ID, Total, Session["NIF"], quantidade) != false)
                                {
                                    ViewBag.MensagemSucesso = "Ingrediente Retirado com sucesso!";
                                }
                            }
                            else
                            {
                                ViewBag.MensagemAtencao = "Valor solicitado é maior que a quantidade do ingrediente em estoque";
                            }
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao tentar retirar o produto!";
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

        /******************************************************************** LISTA INGREDIENTES RETIRADOS ********************************************************************/
        public ActionResult ListarRetirados()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (!Request.HttpMethod.Equals("POST"))
                {

                    ViewBag.ListaRetirados = I.ListaIngredientesRetirados();
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }
    }
}
