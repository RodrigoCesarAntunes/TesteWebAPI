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
    public class PessoaJuridicaController : ApiController
    {

        private VeterinarioServiceEntities vetDb = new VeterinarioServiceEntities();
        private Usuario usuarioObjeto = new Usuario();

        [HttpGet]
        public HttpResponseMessage GetPJ(string email, string senha)
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

        [HttpPost]
        public HttpResponseMessage AddUser(string nome, string email, string cnpj, string cell, int idade, string endereco, string cep, string senha)
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
                    Cpf_Cnpj = cnpj,
                    Celular = cell,
                    Idade = idade,
                    Endereco = endereco,
                    Cep = cep,
                    Autenticacao = autenticacao,
                };

                Cliente_Comercio PJ = new Cliente_Comercio()
                {
                    Usuario = novoUsuario,
                    Usuario_Email = email,
                    Usuario_id = novoUsuario.ID,
                };
                novoUsuario.Cliente_Comercio.Add(PJ);

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
                return Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
            }
        }

    }
}
