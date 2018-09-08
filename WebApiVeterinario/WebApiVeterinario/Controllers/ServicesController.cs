using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiVeterinario.Models;

namespace WebApiVeterinario.Controllers
{
    public class ServicesController : ApiController
    {
        private VeterinarioEntities vetDb = new VeterinarioEntities();
        private Usuario usuarioObjeto = new Usuario();

        [HttpPost]
        public HttpResponseMessage AddService(string email, string senha ,string nome, string descricao, decimal preco)
        {
            try
            {
                if (usuarioObjeto.TestarSenha(senha, email) != true)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuário e/ou senha invalido(s)");
                }

                cliente_comercio pj = (from comercio in vetDb.cliente_comercio
                                       where comercio.usuario.email == email
                                       select comercio).Single();

                Services service = new Services()
                {
                    nome = nome,
                    descricao = descricao,
                    Preco = preco,
                    Cliente_Comercio_CNPJ = pj.usuario_cpf_cnpj,
                    Cliente_Comercio_ID = pj.id
                };

                pj.Services.Add(service);
                vetDb.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Serviço adicionado");
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
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeletarServico(string senha, string email, int id)
        {

            if (usuarioObjeto.TestarSenha(senha, email) != true)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acesso negado!");

            }
            try
            {
                Services servico = vetDb.Services.Find(id);
                vetDb.Services.Remove(servico);
                vetDb.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Serviço removido com sucesso");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message);
            }

        }

    }
}
