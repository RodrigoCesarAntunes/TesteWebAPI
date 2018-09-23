using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiVeterinario.Models;
using WebApiVeterinario.Encrypt;
using System.Data.Entity.Validation;

namespace WebApiVeterinario.Controllers
{
    public class PessoaFisicaController : ApiController
    {
        private VeterinarioServiceEntities vetDb = new VeterinarioServiceEntities();
        private Usuario usuarioObjeto = new Usuario();

        [HttpGet]
        public HttpResponseMessage GetPessoa(string email, string senha)
        {
            try
            {
                if (usuarioObjeto.TestarSenha(senha, email) != true)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuário e/ou senha invalido(s)");
                }

                var pessoa = (from user in vetDb.Usuario
                              where user.Email == email
                              select user).SingleOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, pessoa);
            }
            catch(InvalidOperationException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuário e/ou senha invalido(s)");
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuário e/ou senha invalido(s)");
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUser(string nome, string email, string cpf, string celular, int idade, string endereco, string cep, string senha)
        {
            try
            {
                Password password = new Password();

                string senhaEncriptada = password.EncryptPassword(senha);

                Autenticacao autenticacao = new Autenticacao()
                {
                    Senha = senhaEncriptada,
                    Email = email,
                };

                Models.Usuario novoUsuario = new Models.Usuario()
                {
                    Nome = nome,
                    Email = email,
                    Cpf_Cnpj = cpf,
                    Celular = celular,
                    Idade = idade,
                    Endereco = endereco,
                    Cep = cep,
                    Autenticacao = autenticacao,
                    Data_cadastro = DateTime.Now
                };

                Cliente_Pessoa pessoa = new Cliente_Pessoa()
                {
                    Usuario = novoUsuario,
                    Usuario_Email = email,
                    Usuario_id = novoUsuario.ID,
                };
                novoUsuario.Cliente_Pessoa.Add(pessoa);

                vetDb.Usuario.Add(novoUsuario);
                vetDb.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,"Bem vindo!");
            }
            catch (DbEntityValidationException e)
            {
                String erros = "";
                foreach (var erro in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" Erro \n ", erro.Entry.Entity.GetType().Name, erro.Entry.State);

                    foreach (var ve in erro.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(" - Property: \"{0}\", Erro: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        erros += String.Format(" - Property: \"{0}\", Erro: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, erros);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUser([FromBody]Models.Usuario usuario)
        {
            
            try
            {
                Password password = new Password();

                string senhaEncriptada = password.EncryptPassword(usuario.Autenticacao.Senha);

                Autenticacao autenticacao = new Autenticacao()
                {
                    Senha = senhaEncriptada,
                    Email = usuario.Email,
                };

                Models.Usuario novoUsuario = new Models.Usuario()
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Cpf_Cnpj = usuario.Cpf_Cnpj,
                    Celular = usuario.Celular,
                    Idade = usuario.Idade,
                    Endereco = usuario.Endereco,
                    Cep = usuario.Cep,
                    Autenticacao = autenticacao,
                    Data_cadastro = DateTime.Now
                };

                Cliente_Pessoa pessoa = new Cliente_Pessoa()
                {
                    Usuario = novoUsuario,
                    Usuario_Email = usuario.Email,
                    Usuario_id = novoUsuario.ID,
                };
                novoUsuario.Cliente_Pessoa.Add(pessoa);

                vetDb.Usuario.Add(novoUsuario);
                vetDb.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Bem vindo!");
            }
            catch (DbEntityValidationException e)
            {
                String erros = "";
                foreach (var erro in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" Erro \n ", erro.Entry.Entity.GetType().Name, erro.Entry.State);

                    foreach (var ve in erro.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(" - Property: \"{0}\", Erro: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        erros += String.Format(" - Property: \"{0}\", Erro: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, erros);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, e.StackTrace + "/n" + e.Message);
            }
        }
    }



}
