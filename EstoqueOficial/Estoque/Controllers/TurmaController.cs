using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estoque.Models;


namespace Estoque.Controllers
{
    public class TurmaController : Controller
    {
        Turma T = new Turma();
        Curso C = new Curso();
        Usuario U = new Usuario();

        /******************************************************************** CADASTRAR TURMAS ********************************************************************/
        // GET: Turma
        public ActionResult CadastrarTurma()
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
                        T.NomeTurma = Request.Form["nomeTurma"].ToString();
                        T.QtdAlunos = Convert.ToInt32(Request.Form["qtdAluno"].ToString());
                        T.CodigoCurso = Convert.ToInt32(Request.Form["curso"].ToString());
                        T.Periodo = Request.Form["periodo"].ToString();
                        T.DataInicio = Convert.ToDateTime(Request.Form["dataInicio"].ToString());
                        T.DataTermino = Convert.ToDateTime(Request.Form["dataTermino"].ToString());

                        /***** FAZENDO A CAPTURA DOS VALORES DOS CHECKBOX *****/

                        if (Request.Form["segunda"] != null)
                        {
                            T.Aux = Request.Form["segunda"].ToString();
                            T.Soma = T.Soma + 32;
                        }
                        if (Request.Form["terca"] != null)
                        {
                            T.Aux = Request.Form["terca"].ToString();
                            T.Soma = T.Soma + 16;
                        }
                        if (Request.Form["quarta"] != null)
                        {
                            T.Aux = Request.Form["quarta"].ToString();
                            T.Soma = T.Soma + 8;
                        }
                        if (Request.Form["quinta"] != null)
                        {
                            T.Aux = Request.Form["quinta"].ToString();
                            T.Soma = T.Soma + 4;
                        }
                        if (Request.Form["sexta"] != null)
                        {
                            T.Aux = Request.Form["sexta"].ToString();
                            T.Soma = T.Soma + 2;
                        }
                        if (Request.Form["sabado"] != null)
                        {
                            T.Aux = Request.Form["sabado"].ToString();
                            T.Soma = T.Soma + 1;
                        }
                        if (T.Periodo.Equals("Selecione uma opção"))
                        {
                            ViewBag.MensagemAtencao = "É necessário selecionar um Período!";
                        }
                        else if (T.CodigoCurso.Equals("Selecione uma opção"))
                        {
                            ViewBag.MensagemAtencao = "É necessário selecionar um Curso!";
                        }
                        else
                        {
                            if (T.DataInicio > T.DataTermino)
                            {
                                ViewBag.MensagemAtencao = "Data Termino não pode ser menor que a Data Início!";
                            }
                            else
                            {
                                T.CadastrarTurma(Curso.Aux);
                                ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";
                            }
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) Preenchido(s) incorretamente! Verifique todos os campos.";
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Cursos = C.ListarCursos(Session["NIF"].ToString());
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** ALTERAR TURMAS ********************************************************************/
        public ActionResult AlterarTurma(String ID)
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
                        T.CodigoTurma = Convert.ToInt32(Request.Form["codigoTurma"].ToString());
                        T.NomeTurma = Request.Form["nomeTurma"].ToString();
                        T.QtdAlunos = Convert.ToInt32(Request.Form["qtdAluno"].ToString());
                        T.CodigoCurso = Convert.ToInt32(Request.Form["curso"].ToString());
                        T.Periodo = Request.Form["periodo"].ToString();
                        T.DataInicio = Convert.ToDateTime(Request.Form["dataInicio"].ToString());
                        T.DataTermino = Convert.ToDateTime(Request.Form["dataTermino"].ToString());

                        /***** FAZENDO A CAPTURA DOS VALORES DOS CHECKBOX *****/
                        T.Soma = 0;

                        /***** CADA DIA E REPRESENTADO POR UM NUMERO *****/
                        if (Request.Form["segunda"] != null)
                        {
                            T.Aux = Request.Form["segunda"].ToString();
                            T.Soma = T.Soma + 32;
                        }
                        if (Request.Form["terca"] != null)
                        {
                            T.Aux = Request.Form["terca"].ToString();
                            T.Soma = T.Soma + 16;
                        }
                        if (Request.Form["quarta"] != null)
                        {
                            T.Aux = Request.Form["quarta"].ToString();
                            T.Soma = T.Soma + 8;
                        }
                        if (Request.Form["quinta"] != null)
                        {
                            T.Aux = Request.Form["quinta"].ToString();
                            T.Soma = T.Soma + 4;
                        }
                        if (Request.Form["sexta"] != null)
                        {
                            T.Aux = Request.Form["sexta"].ToString();
                            T.Soma = T.Soma + 2;
                        }
                        if (Request.Form["sabado"] != null)
                        {
                            T.Aux = Request.Form["sabado"].ToString();
                            T.Soma = T.Soma + 1;
                        }
                        if (T.Periodo.Equals("Selecione uma opção"))
                        {
                            ViewBag.MensagemAtencao = "É necessário selecionar um Período!";
                        }
                        if (T.CodigoCurso.Equals("Selecione uma opção"))
                        {
                            ViewBag.MensagemAtencao = "É necessário selecionar um Curso!";
                        }
                        else
                        {
                            if (T.DataInicio > T.DataTermino)
                            {
                                ViewBag.MensagemAtencao = "Data Termino não pode ser menor que a Data Início!";
                            }
                            else
                            {
                                T.Soma = T.Soma;
                                T.AlterarTurma();
                                ViewBag.MensagemSucesso = "Alteração realizada com sucesso!";
                            }
                        }
                    }
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) Preenchido(s) incorretamente! Verifique todos os campos.";
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }

            ViewBag.Cursos = C.ListarCursos(Session["NIF"].ToString());
            ViewBag.Turma = (ID == null) ? new Turma() : new Turma(ID);
            ViewBag.Dias = CalcularSemanas(T.ListarDiasSemana(ID));
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** REMOVER TURMAS ********************************************************************/
        public ActionResult RemoverTurma(String ID)
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
                        T.RemoverTurma(ID);
                        return RedirectToAction("ListarTurmas", "Turma");
                    }
                    /**** MESNAGEM DE ERRO *****/
                    catch
                    {
                        ViewBag.MensagemErro = "Campo(s) Preenchidos incorretamente! Verifique todos os campos antes de cadastrar.";
                    }
                }
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        /******************************************************************** LISTAR TURMAS ********************************************************************/
        public ActionResult ListarTurmas()
        {
            if (Session["UsuarioLogado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if (Session["NivelAcesso"].Equals(2) || Session["NivelAcesso"].Equals(3) || Session["NivelAcesso"].Equals(4))
            {
                ViewBag.Turmas = T.ListarTurmas();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
            ViewBag.Imagens = Usuario.ListarImagem(Session["NIF"]);
            return View();
        }

        /******************************************************************** METODO CALCULAR RADIOBOX SELECIONADOS ********************************************************************/
        public List<Turma> CalcularSemanas(Int32 Soma)
        {
            /***** ALGORITMO PARA IDENTIFICAR OS CHECKBOX QUE FORAM MARCADOS, FUNCIONA DE SABADO PARA SEGUNDA *****/
            /***** QUANDO O RETORNO FOR 1, ENTÃO O CHECKBOX CORRESPONDENTE DO INDICE DA MATRIZ FOI SELECIONADO *****/
            int[] sem = new int[6];
            int n = 0, valor;

            // INICIA O VETOR, ZERANDO O MESMO // 
            for (int x = 0; x <= 5; x++)
            {
                sem[x] = 0;
            }
            // VALOR DO BD É ATRIBUIDO EM UMA VARIAVEL //
            valor = Soma;

            do
            {
                if (valor >= 2)
                {
                    sem[n] = valor % 2;
                    valor /= 2;
                    n++;
                }
                else
                {
                    sem[n] = valor;
                    break;
                }
            } while (valor > -1);

            List<Turma> DiasSemanas = new List<Turma>();
            Turma T = new Turma();

            if (sem[0].Equals(1))
            {
                ViewBag.Sabado = "checked";
                DiasSemanas.Add(T);
            }
            if (sem[1].Equals(1))
            {
                ViewBag.Sexta = "checked";
                DiasSemanas.Add(T);
            }
            if (sem[2].Equals(1))
            {
                ViewBag.Quinta = "checked";
                DiasSemanas.Add(T);
            }
            if (sem[3].Equals(1))
            {
                ViewBag.Quarta = "checked";
                DiasSemanas.Add(T);
            }
            if (sem[4].Equals(1))
            {
                ViewBag.Terca = "checked";
                DiasSemanas.Add(T);
            }
            if (sem[5].Equals(1))
            {
                ViewBag.Segunda = "checked";
                DiasSemanas.Add(T);
            }
            return DiasSemanas;
        }
    }
}