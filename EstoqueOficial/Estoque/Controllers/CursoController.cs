using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;

namespace Estoque.Controllers
{
    public class CursoController : Controller
    {
        Curso C = new Curso();
        // GET: Curso

        /******************************************************************** CADASTRAR CURSOS ********************************************************************/
        public ActionResult CadastrarCurso()
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
                        C.NomeCurso = Request.Form["nomeCurso"].ToString();
                        C.Duracao = Convert.ToInt32(Request.Form["duracao"].ToString());
                        C.Cadastrar(Session["NIF"].ToString());
                        ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";

                        Response.Redirect("/Curso/ListarCursos", false);
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) Preenchido(s) incorretamente! Verifique todos os campos.";
                    }
                }
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** ALTERAR CURSOS ********************************************************************/
        public ActionResult AlterarCurso(String ID)
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
                        C.CodigoCurso = Convert.ToInt32(Request.Form["codigoCurso"].ToString());
                        C.NomeCurso = Request.Form["nomeCurso"].ToString();
                        C.Duracao = Convert.ToInt32(Request.Form["duracao"].ToString());

                        /***** AÇÃO ALTERAR *****/
                        C.AlterarCurso();
                        ViewBag.MensagemSucesso = "Alteração realizada com sucesso!";
                    }
                    /**** MESNAGEM DE ERRO *****/
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) Preenchidos incorretamente! Verifique todos os campos.";
                    }
                }
                ViewBag.Cursos = C.ListarCursos(Session["NIF"].ToString());
                ViewBag.Curso = (ID == null) ? new Curso() : new Curso(ID);
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** REMOVER CURSOS ********************************************************************/
        public ActionResult RemoverCurso(String ID)
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
                        if (C.VerificaCurso(ID) == true)
                        {
                            Session["Atencao"] = "Existem turmas cadastradas nesse curso.";
                            return RedirectToAction("ListarCursos", "Curso");
                        }
                        else
                        {
                            if (C.RemoverCurso(ID) == true)
                            {
                                ViewBag.MensagemSucesso = "Curso Excluido com sucesso!"; 
                                return RedirectToAction("ListarCursos", "Curso");
                            }
                            else
                            {
                                ViewBag.MensagemAtencao = "Ação inválida, pois existe um ou mais turmas cadastrado com este curso.";
                            }
                        }
                    }
                    /**** MENSAGEM DE ERRO *****/
                    catch (Exception Exception)
                    {
                        ViewBag.Mensagem = Exception;
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTAR CURSOS ********************************************************************/
        public ActionResult ListarCursos()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                ViewBag.Cursos = C.ListarCursos(Session["NIF"].ToString());
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }
    }
}
