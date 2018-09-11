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
    public class PetController : ApiController
    {
        private Usuario usuarioObjeto = new Usuario();
        private VeterinarioServiceEntities vetDb = new VeterinarioServiceEntities();

        [HttpPost]
        public HttpResponseMessage AddPet(string email, string senha, string nome, string raca, decimal peso, string tamanho,string descricao,string genero, int idade, string especie)
        {
        
            if (usuarioObjeto.TestarSenha(senha, email) != true)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuário e/ou senha invalido(s)");

            }
            
            Cliente_Pessoa dono = (from pessoa in vetDb.Cliente_Pessoa
                                   where pessoa.Usuario_Email == email
                                   select pessoa).Single();

            Pets pet = new Pets()
            {
                Nome = nome,
                What_pet = especie,
                Raca = raca,
                Peso = peso,
                Tamanho = tamanho,
                Descricao = descricao,
                Genero = genero,
                Idade = idade,
                Cliente_pessoa_Email = dono.Usuario_Email,
                Cliente_pessoa_id = dono.ID,
            };

            dono.Pets.Add(pet);
            try
            {
                vetDb.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Pet adicionado com sucesso!");
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
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message);
            }
        }   

        [HttpDelete]
        public HttpResponseMessage DeletarPet(string senha, string email, int id)
        {
            if (usuarioObjeto.TestarSenha(senha, email) != true)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acesso negado!");

            }
            try
            {
                Pets pets = vetDb.Pets.Find(id);
                vetDb.Pets.Remove(pets);
                vetDb.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Pet removido com sucesso");
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message);
            }
        }
    }
}
