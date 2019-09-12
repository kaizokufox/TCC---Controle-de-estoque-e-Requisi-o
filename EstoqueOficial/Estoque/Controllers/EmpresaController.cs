using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;

namespace Estoque.Controllers
{
    public class EmpresaController : Controller
    {
        // GET: Empresa

        /******************************************************************** CADASTRAR EMPRESA ********************************************************************/
        public ActionResult CadastrarEmpresa()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) || Session["NivelAcesso"].Equals(4))
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    Empresa E = new Empresa();

                    try
                    {
                        E.NomeEmpresa = Request.Form["nomeEmpresa"].ToString();
                        E.Fone = Request.Form["Fone"].ToString();
                        E.Contato = Request.Form["contato"].ToString();

                        if (E.CadastrarEmpresa() == true)
                        {
                            ViewBag.MensagemSucesso = "Empresa cadastrada com sucesso!";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao cadastrar empresa!";
                    }
                }
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }
        /******************************************************************** LISTAR EMPRESAS ********************************************************************/
        public ActionResult ListarEmpresas()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) ||  Session["NivelAcesso"].Equals(4))
            {
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

        /******************************************************************** ALTERAR EMPRESA ********************************************************************/
        public ActionResult AlterarEmpresa(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(0) ||  Session["NivelAcesso"].Equals(4))
            {
                Empresa E = new Empresa();

                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        E.NomeEmpresa = Request.Form["nomeEmpresa"].ToString();
                        E.Fone = Request.Form["fone"].ToString();
                        E.Contato = Request.Form["contato"].ToString();

                        if (E.AlterarEmpresa(ID) == true)
                        {
                            ViewBag.MensagemSucesso = "Empresa alterada com sucesso!";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao alterar empresa!";
                    }

                }
                UsuarioController UC = new UsuarioController();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);//Comando para lista a imagem com a resolução correta
                ViewBag.Empresa = E.ListarEmpresa(ID);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        
        
    }
}