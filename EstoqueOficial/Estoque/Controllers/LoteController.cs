using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;

namespace Estoque.Controllers
{
    public class LoteController : Controller
    {
        Lote L = new Lote();
        public string LoteAtual = "";

        /******************************************************************** CADASTRAR LOTE ********************************************************************/
        public ActionResult CadastrarLote()
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
                        DateTime Hora = DateTime.UtcNow;
                        TimeZoneInfo NossaZona = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                        DateTime Databrasilia = TimeZoneInfo.ConvertTimeFromUtc(Hora, NossaZona);

                        L.NomeLote = Request.Form["nomeLote"].ToString();
                        L.DataEntrada = Databrasilia;
                        L.DataValidade = Convert.ToDateTime(Request.Form["dataValidade"].ToString());
                        L.Quantidadeitens = Convert.ToInt32(Request.Form["quantidadeitens"].ToString());

                        if (L.DataValidade > L.DataEntrada)
                        {
                            L.CadastrarLote(Session["NIF"].ToString());


                            /***** CHAMA ESSE METODO PARA DESCOBRIR QUAL E O CODIGO DO LOTE PARA PODER ATRIBUIR O CODIGO NA VARIAVEL 'CodigoLote'
                            PODENDO CADASTRAR UM INGREDIENTE A PARTIR DO LINK DIRETO DE 'CADASTRAR UM LOTE' *****/
                            L.SelecionaLote(L.NomeLote);
                            Session["NomeLote"] = L.NomeLote;
                            Session["Mensagem"] = ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";

                            Response.Redirect("/Ingrediente/CadastrarIngrediente/" + L.CodigoLote, false);
                        }
                        else
                        {
                            ViewBag.MensagemAtencao = "Data de validade não pode ser menor que a data de entrada.";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) preenchido(s) incorretamente! Verifique todos os campos.";
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

        /******************************************************************** ALTERAR LOTE ********************************************************************/
        public ActionResult AlterarLote(String ID)
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
                        L.CodigoLote = Convert.ToInt32(Request.Form["codigoLote"].ToString());
                        L.NomeLote = Request.Form["nomeLote"].ToString();
                        L.DataEntrada = Convert.ToDateTime(Request.Form["dataEntrada"].ToString());
                        L.DataValidade = Convert.ToDateTime(Request.Form["dataValidade"].ToString());
                        L.Quantidadeitens = Convert.ToInt32(Request.Form["quantidadeitens"].ToString());

                        L.AlterarLote();
                        ViewBag.MensagemSucesso = "Alteração realizada com sucesso!";
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) preenchido(s) incorretamente! Verifique todos os campos.";
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Lote = (ID == null) ? new Lote() : new Lote(ID);
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** DELETAR LOTE ********************************************************************/
        public ActionResult DeletarLote(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (Session["UsuarioLogado"] == null)
                {
                    Response.Redirect("/Login/Index", false);
                }
                else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
                {
                    if (!Request.HttpMethod.Equals("POST"))
                    {
                        if (Session["Mensagem"] != null)
                        {
                            Session["Mensagem"] = null;
                        }
                        try
                        {
                            L.DeletarLote(ID);
                            Session["Mensagem"] = ViewBag.MensagemSucesso = "Lote excluido com sucesso!";
                            return RedirectToAction("ListarLotes", "Lote");
                        }
                        catch
                        {
                            Session["MensagemErro"] = ViewBag.MensagemErro = "Erro ao excluir o lote. Verifique se não existe nenhum ingrediente cadastrado utilizando este lote.";
                            return RedirectToAction("ListarLotes", "Lote");
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTAR LOTE ********************************************************************/
        public ActionResult ListarLotes()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                try
                {
                    ViewBag.Lotes = L.ListarLotes();
                }
                catch
                {
                    ViewBag.MensagemErro = "Erro ao tentar listar os lotes.";
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** DETALHES DO  LOTE ********************************************************************/
        public ActionResult DetalhesLote(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (ID == null)
                {
                    String CodigoLote = Session["CodigoLote"].ToString();
                    ViewBag.DetalhesLote = L.DetalhesLote(CodigoLote);
                }
                else if (Session["CodigoLote"] == null)
                {
                    Session["CodigoLote"] = ID;
                    ViewBag.DetalhesLote = L.DetalhesLote(ID);
                }
                else
                {
                    Session["CodigoLote"] = ID;
                    String CodigoLote = Session["CodigoLote"].ToString();
                    ViewBag.DetalhesLote = L.DetalhesLote(CodigoLote);
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            LoteAtual = Request.QueryString["ID"];
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }
    }
}