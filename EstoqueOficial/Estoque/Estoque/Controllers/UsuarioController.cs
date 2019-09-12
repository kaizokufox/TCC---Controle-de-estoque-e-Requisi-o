using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;

namespace Estoque.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario

        /******************************************************************** CADASTRO USUARIO ********************************************************************/
        public ActionResult CadastrarUsuario()
        {
            Usuario U = new Usuario();

            if (Request.HttpMethod.Equals("POST"))
            {
                try
                {
                    U.NIF = Convert.ToInt32(Request.Form["nif"].ToString());
                    U.Nome = Request.Form["nome"].ToString();
                    U.Sobrenome = Request.Form["sobrenome"].ToString();
                    U.CPF = Request.Form["cpf"].ToString();
                    U.Cargo = Request.Form["cargo"].ToString();
                    U.Telefone = Request.Form["telefone"].ToString();
                    U.DataNascimento = Convert.ToDateTime(Request.Form["datanascimento"].ToString());
                    U.Email = Request.Form["email"].ToString();
                    U.Senha = Request.Form["senha"].ToString();

                    /**** VERIFICA SENHA *****/
                    if (U.Senha.Equals(""))
                    {
                        ViewBag.Mensagem = "A senha deve ser preenchida!";
                    }
                    else if (U.Senha.Length < 6)
                    {
                        ViewBag.Mensagem = "A senha deve conter no minimo 6 caracteres!";
                    }
                    else /**** AÇÃO CADASTRAR *****/
                    {
                        ViewBag.Mensagem = U.Cadastrar() ? "Cadastro realizado com sucesso!" : "Erro ao Cadastrar!";
                    }

                }
                /**** MESNAGEM DE ERRO *****/
                catch
                {
                    ViewBag.Mensagem = "Campo(s) Preenchidos incorretamente! verifique todos os campos antes de cadastrar.";
                }

            }


            return View();
        }

        /******************************************************************** LISTAR USUARIO ********************************************************************/
        public ActionResult ListarUsuarios()
        {
            Usuario U = new Usuario();

            ViewBag.Usuarios = U.ListarUsuarios();
            return View();
        }

        /******************************************************************** ALTERAR USUARIO ********************************************************************/
        public ActionResult AlterarUsuario(String ID)
        {
            Usuario U = new Usuario();

            if (Request.HttpMethod.Equals("POST"))
            {
                try
                {
                    U.NIF = Convert.ToInt32(Request.Form["nif"].ToString());
                    U.Nome = Request.Form["nome"].ToString();
                    U.Sobrenome = Request.Form["sobrenome"].ToString();
                    U.CPF = Request.Form["cpf"].ToString();
                    U.Cargo = Request.Form["cargo"].ToString();
                    U.Telefone = Request.Form["telefone"].ToString();
                    U.DataNascimento = Convert.ToDateTime(Request.Form["datanascimento"].ToString());
                    U.Email = Request.Form["email"].ToString();
                    U.Senha = Request.Form["senha"].ToString();

                    /**** VERIFICA SENHA *****/
                    if (U.Senha.Equals(""))
                    {
                        ViewBag.Mensagem = "A senha deve ser preenchida!";
                    }
                    else if (U.Senha.Length < 6)
                    {
                        ViewBag.Mensagem = "A senha deve conter no minimo 6 caracteres!";
                    }
                    /***** AÇÃO ALTERAR *****/
                    else if (U.NIF > 0)
                    {
                        ViewBag.Mensagem = U.Alterar() ? "Dados do usuário alterado com sucesso!" : "Erro ao alterar! Verifique os campos preenchidos e tente novamente.";
                    }
               }
                /**** MESNAGEM DE ERRO *****/
                catch
                {
                    ViewBag.Mensagem = "Campo(s) Preenchidos incorretamente! verifique todos os campos antes de cadastrar.";
                }
            }

            ViewBag.Usuario = (ID == null) ? new Usuario() : new Usuario(ID);

            return View();
        }
    }
}