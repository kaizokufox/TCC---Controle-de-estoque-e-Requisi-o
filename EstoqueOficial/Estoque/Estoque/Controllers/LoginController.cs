using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Estoque.Models;

namespace Estoque.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Request.HttpMethod.Equals("POST"))
            {
                Usuario U = new Usuario();


                Int32 NIF = Convert.ToInt32(Request.Form["nif"].ToString());
                String Senha = Request.Form["senha"].ToString();

                U.Login(NIF, Senha);

                if (NIF == U.NIF && Senha == U.Senha)
                {
                    ViewBag.Mensagem = "Autenticação feita com sucesso!";
                }
                else
                {
                    ViewBag.Mensagem = "Falha na autenticação! NIF e/ou Senha estão incorretos ";
                }



            }
            ViewBag.Usuario = new Usuario();

            return View();
        }

    }
}