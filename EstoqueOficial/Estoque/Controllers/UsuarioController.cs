using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;
using System.IO;
using System.Threading;

namespace Estoque.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Home()
        {
            return View();
        }
        Usuario U = new Usuario();
        public static byte[] Binario2 { get; set; }


        /******************************************************************** CADASTRO USUARIO ********************************************************************/
        public ActionResult CadastrarUsuario()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
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
                        Binario2 = br.ReadBytes((int)StreamImagem.BaseStream.Length);

                        /**** VERIFICA SENHA *****/
                        if (U.Senha.Equals(""))
                        {
                            ViewBag.MensagemErro = "A senha deve ser preenchida!";
                        }
                        else if (U.Senha.Length < 6)
                        {
                            ViewBag.MensagemAtencao = "A senha deve conter no mínimo 6 caracteres!";
                        }
                        /***** AÇÃO VERIFICA USUARIO JÁ CADASTRAO NO SISTEMA *****/
                        else if (U.VerificarUsuario(U.NIF).Equals(false))
                        {
                            if (ValidaCPF(U.CPF) == true)
                            {
                                /***** AÇÃO CADASTRAR *****/
                                U.Senha = LoginController.CalcularSha1(U.Senha);
                                U.CadastrarUsuario();
                                ViewBag.Mensagem = "Cadastro realizado com sucesso!";

                                Response.Redirect("/Usuario/ListarUsuarios", false);
                            }
                            else
                            {
                                ViewBag.MensagemAtencao = "CPF Inválido! Insira um CPF válido.";
                            }
                        }
                        else
                        {
                            ViewBag.MensagemAtencao = "Já existe um usuário cadastrado no sistema com este NIF!";
                        }
                    }
                    /***** MESNAGEM DE ERRO *****/
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) Preenchido(s) incorretamente! Verifique todos os campos.";
                    }
                }
                ViewBag.Cargos = U.ListarCargos();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** ALTERAR USUARIO ********************************************************************/
        public ActionResult AlterarUsuario(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
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

                        /**** VERIFICA SENHA *****/
                        if (U.Senha.Equals(""))
                        {
                            ViewBag.MensagemAtencao = "A senha deve ser preenchida!";
                        }
                        else if (U.Senha.Length < 6)
                        {
                            ViewBag.MensagemAtencao = "A senha deve conter no mínimo 6 caracteres!";
                        }
                        /***** AÇÃO ALTERAR *****/
                        else if (U.NIF.Length > 0)
                        {
                            U.Senha = LoginController.CalcularSha1(U.Senha);
                            U.Alterar();
                            ViewBag.MensagemSucesso = "Alteração realizada com sucesso!";
                        }
                    }
                    ///**** MESNAGEM DE ERRO *****/
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) Preenchidos incorretamente! Verifique todos os campos.";
                    }
                }
                ViewBag.Cargos = U.ListarCargos();
                ViewBag.Usuario = (ID == null) ? new Usuario() : new Usuario(Convert.ToInt32(ID));
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** REMOVER USUARIO ********************************************************************/
        public ActionResult RemoverUsuario(String ID)
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                if (!Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        U.Remover(ID);
                        Response.Redirect("/Usuario/ListarUsuarios", false);
                    }
                    catch
                    {
                        ViewBag.Mensagem = "Erro ao Excluir!";
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** CONFERE USUARIO ********************************************************************/

        public void ConfereUsuario()
        {
            if ((Session["UsuarioLogado"] == null))
            {
                Response.Redirect("/Login/Index", false);
            }
            else if (Session["NivelAcesso"].Equals(1))
            {
                ViewBag.Mensagem = "teste";
                Response.Redirect("/Home/Home", false);

            }
            else
            {
                Response.Redirect("/Usuario/ListarUsuarios", false);
            }
        }


        /******************************************************************** LISTAR USUARIO ********************************************************************/
        public ActionResult ListarUsuarios()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                ViewBag.Usuarios = U.ListarUsuarios();
                ViewBag.Cargo = U.ListarCargos();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }


        /******************************************************************** PRIMEIRA VEZ ********************************************************************/
        public ActionResult PrimeiraVez()
        {
            if (Session["PrimeiraVez"] != null)
            {
                ViewBag.Mensagem = U.CadastrarUsuario();
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        /******************************************************************** ALTERAR SENHA ********************************************************************/
        public ActionResult AlterarSenha()
        {
            if (Session["UsuarioLogado"] == null)
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
                                ViewBag.MensagemAtencao = "Erro ao tentar salvar nova senha, a senha deve conter no minimo 6 caracteres";
                            }
                            else
                            {
                                U.NovaSenha = LoginController.CalcularSha1(U.NovaSenha);

                                if (U.AlterarSenha(Session["NIF"]) != false)
                                {
                                    ViewBag.MensagemSucesso = "Senha alterada com sucesso!";
                                }
                                else
                                {
                                    ViewBag.MensagemErro = "Erro ao Alterar senha.";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.MensagemAtencao = "Erro ao tentar salvar nova senha, as senhas devem ser iguais";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao Alterar senha.";
                    }
                }
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                return View();
            }
        }

        /******************************************************************** ALTERAR DADOS PESSOAIS ********************************************************************/
        public ActionResult AlterarDadosPessoais()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    Usuario U = new Usuario();
                    try
                    {
                        U.NIF = Request.Form["nif"].ToString();
                        U.Nome = Request.Form["nome"].ToString();
                        U.Sobrenome = Request.Form["sobrenome"].ToString();
                        U.CPF = Request.Form["cpf"].ToString();
                        U.DataNascimento = Convert.ToDateTime(Request.Form["dataNascimento"].ToString());

                        if (ValidaCPF(U.CPF) == true)
                        {
                            /***** AÇÃO ALTERAR *****/
                            if (U.AlterarDadosPessoais(Session["NIF"]) != false)
                            {
                                ViewBag.MensagemSucesso = "Dados Pessoais alterado com sucesso!";
                            }
                            else
                            {
                                ViewBag.MensagemErro = "Erro ao alterar os dados pessoais!";
                            }
                        }
                        else
                        {
                            ViewBag.MensagemAtencao = "CPF Inválido! Insira um CPF válido.";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao salvar! Preencha o(s) campo(s) Corretamente.";
                    }
                }
                ViewBag.DadosPessoais = U.ListarDadospessoais(Session["NIF"]);
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                return View();
            }
        }

        /******************************************************************** ALTERAR DADOS DE CONTATO ********************************************************************/
        public ActionResult AlterarContato()
        {
            if (Session["UsuarioLogado"] == null)
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
                        U.Email = Request.Form["email"].ToString();
                        U.TelefoneFixo = Request.Form["telefoneFixo"].ToString();
                        U.TelefoneMovel = Request.Form["telefoneMovel"].ToString();

                        if (U.AlterarContato(Session["NIF"]) != false)
                        {
                            ViewBag.MensagemSucesso = "Dados de contato alterado com sucesso!";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao alterar o(s) dado(s) de contato!";
                    }
                }
                ViewBag.DadosPessoais = U.ListarDadospessoais(Session["NIF"]);
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                return View();
            }
        }

        /******************************************************************** CONVERTER BINARIO EM IMAGEM ********************************************************************/
        public ActionResult Ver(String id)
        {
            Usuario U = new Usuario(Convert.ToInt32(id));

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = "image/png";
            Response.BinaryWrite(U.Binario);
            Response.End();

            return View();
        }

        /******************************************************************** ALTERAR IMAGEM ********************************************************************/
        public ActionResult AlterarImagem()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Request.HttpMethod.Equals("POST"))
                {
                    try
                    {
                        Stream StreamImagem = Request.Files["Imagem"].InputStream;
                        Byte[] BufferImagem = new Byte[StreamImagem.Length];
                        StreamImagem.Read(BufferImagem, 0, (Int32)StreamImagem.Length);

                        Usuario U = new Usuario();
                        U.Binario = BufferImagem;

                        if (U.AlterarImagem(Session["NIF"]) != false)
                        {
                            ViewBag.MensagemSucesso = "Imagem alterado com sucesso!";
                        }
                        else
                        {
                            ViewBag.MensagemErro = "Erro ao alterar a imagem!";
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Erro ao alterar a imagem!";
                    }
                }
                ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
                return View();
            }
        }

        /******************************************************************** VALIDA CPF ********************************************************************/
        public static bool ValidaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}