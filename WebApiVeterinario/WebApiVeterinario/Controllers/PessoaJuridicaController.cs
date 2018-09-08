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

        private VeterinarioEntities vetDb = new VeterinarioEntities();
        private Usuario usuarioObjeto = new Usuario();

        [HttpGet]
        public HttpResponseMessage GetPJ(string email, string senha)
        {

            if (usuarioObjeto.TestarSenha(senha, email) != true)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuário e/ou senha invalido(s)");
            }

            var pessoa = (from user in vetDb.usuario
                          where user.email == email
                          select user).SingleOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, pessoa);
        }

        [HttpPost]
        public HttpResponseMessage AddUser(string nome, string email, string cnpj, string cell, int age, string address, string cep, string senha)
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

                usuario novoUsuario = new usuario()
                {
                    nome = nome,
                    email = email,
                    cpf_cnpj = cnpj,
                    cellphone = cell,
                    age = age,
                    address = address,
                    cep = cep,
                    Autenticacao = autenticacao,
                };

                cliente_comercio PJ = new cliente_comercio()
                {
                    usuario = novoUsuario,
                    usuario_cpf_cnpj = cnpj,
                    usuario_id = novoUsuario.id,
                };
                novoUsuario.cliente_comercio.Add(PJ);

                vetDb.usuario.Add(novoUsuario);
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
