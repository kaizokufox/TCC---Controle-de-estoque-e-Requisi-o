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
        public Boolean Validacao;

        /******************************************************************** AUTENTICAR USUARIO ********************************************************************/
        public ActionResult Index()
        {
            Usuario U = new Usuario();

            if (U.VerificaUsuario() <= 0)
            {
                Response.Redirect("/Home/PrimeiraVez", false);
            }
            else
            {
                Validacao = false;
            }

            if (Request.HttpMethod.Equals("POST"))
            {
                try
                {
                    String NIF = Request.Form["nif"].ToString();
                    String Senha = Request.Form["senha"].ToString();
                    Senha = CalcularSha1(Senha);

                    U.Login(NIF, Senha);

                    if (NIF.Equals(U.NIF) && Senha == U.Senha)
                    {
                        ViewBag.Mensagem = "Autenticação feita com sucesso!";
                        Session["UsuarioLogado"] = true;
                        Session["NivelAcesso"] = U.Nivel;
                        Session["NIF"] = U.NIF;
                        Session["NomeUsuario"] = U.Nome;
                        Session["NomeCargo"] = U.NomeCargo;

                        Response.Redirect("/Home/Home", false);
                    }
                }
                catch
                {
                    ViewBag.MensagemErro = "NIF e/ou Senha incorreto(s)!";
                }
            }
            Session["Autenticado"] = "";
            return View();
        }

        /******************************************************************** RECUPERAR SENHA ********************************************************************/
        public ActionResult RecuperarSenha()
        {
            if (Request.HttpMethod.Equals("POST"))
            {
                try
                {
                    Usuario U = new Usuario();

                    U.NIF = Request.Form["nif"].ToString();
                    U.CPF = Request.Form["cpf"].ToString();
                    U.DataNascimento = Convert.ToDateTime(Request.Form["datanascimento"].ToString());
                    U.Email = Request.Form["email"].ToString();
                    Session["NIF"] = U.NIF;
                    U.RecuperarSenha();

                    if (U.RecuperarSenha() >= 1)
                    {
                        Session["Autenticado"] = "ok";
                        Response.Redirect("/Login/TrocarSenha", false);
                    }
                    else
                    {
                        Session["Autenticado"] = "!ok";
                        ViewBag.Mensagem = "Não existe nenhum usuário cadastrado no sistema com esses dados informados.";
                    }
                }
                catch
                {
                    Session["Autenticado"] = "!ok";
                    ViewBag.Mensagem = "Não existe nenhum usuário cadastrado no sistema com esses dados informados.";
                }
            }
            return View();
        }

        /******************************************************************** TROCAR SENHA ********************************************************************/
        public ActionResult TrocarSenha()
        {
            if (Session["Autenticado"].Equals(""))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        Usuario U = new Usuario();

                        U.NovaSenha = Request.Form["novaSenha"].ToString();
                        U.RepitaNovaSenha = Request.Form["repitaNovasenha"].ToString();

                        if (U.NovaSenha == U.RepitaNovaSenha)
                        {
                            if (U.NovaSenha.Length < 6)
                            {
                                ViewBag.SenhaMinima = "Erro ao tentar salvar nova senha, a senha deve conter no minimo 6 caracteres";
                            }
                            else
                            {
                                U.NovaSenha = LoginController.CalcularSha1(U.NovaSenha);
                                if (U.AlterarSenha(Session["NIF"]) != false)
                                {
                                    Session["NIF"] = "";
                                    ViewBag.MensagemSucesso = "Senha alterada com sucesso!";
                                }
                                else
                                {
                                    ViewBag.Mensagem = "Erro ao Alterar senha.";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SenhaNaoConfere = "Erro ao tentar salvar nova senha, as senhas devem ser iguais";
                        }
                    }
                    catch
                    {
                        ViewBag.Mensagem = "Erro ao Tentar salvar senha.";
                    }
                }
                return View();
            }
        }

        /******************************************************************** SAIR ********************************************************************/
        public void Logout()
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("/Login/Index", false);
        }

        /******************************************************************** CRIPTOGRAFIA DA SENHA ********************************************************************/
        public static String CalcularSha1(String Text)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(Text);
                System.Security.Cryptography.SHA1CryptoServiceProvider CryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                String hash = BitConverter.ToString(CryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
                return hash;
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }
    }
}


