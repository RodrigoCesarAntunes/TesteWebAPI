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
        private VeterinarioEntities vetDb = new VeterinarioEntities();

        [HttpPost]
        public void AddPet(string email, string senha, string nome, string breed, decimal peso, string tamanho, string descricao,string genero, string idade, string especie)
        {
        
            if (usuarioObjeto.TestarSenha(senha, email) != true)
            {
                throw new UnauthorizedAccessException();

            }

            cliente_pessoa dono = (from pessoa in vetDb.cliente_pessoa
                                   where pessoa.usuario.email == email
                                   select pessoa).Single();

            pets pet = new pets()
            {
                nome = nome,
                what_pet = especie,
                breed = breed,
                wheight = peso,
                size = tamanho,
                description = descricao,
                age = 4,
                cliente_pessoa_cpf = dono.usuario_cpf_cnpj,
                cliente_pessoa_id = dono.id,
            };

            dono.pets.Add(pet);
            try
            {
                vetDb.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var erro in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" Erro \n ", erro.Entry.Entity.GetType().Name, erro.Entry.State);

                    foreach (var ve in erro.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(" - Property: \"{0}\", Erro: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }    
        }   
    }
}
