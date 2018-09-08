using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiVeterinario.Models;
using WebApiVeterinario.Encrypt;

namespace WebApiVeterinario.Controllers
{
    public class PessoaFisicaController : ApiController
    {
        private VeterinarioEntities vetDb = new VeterinarioEntities();
        private Usuario usuarioObjeto = new Usuario();

        [HttpGet]
        public usuario GetPessoa(string email, string senha)
        {
            
            if(usuarioObjeto.TestarSenha(senha,email)!=true)
            {
                throw new UnauthorizedAccessException();
                
            }

            var pessoa = (from user in vetDb.usuario
                            where user.email == email
                            select user).SingleOrDefault();
            return pessoa;
        }

        [HttpPost]
        public void AddUser(string nome, string email, string cpf, string cell, int age, string address, string cep, string senha)
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
                cpf_cnpj = cpf,
                cellphone = cell,
                age = age,
                address = address,
                cep = cep,
                Autenticacao = autenticacao,
            };

            cliente_pessoa pessoa = new cliente_pessoa()
            {
                usuario = novoUsuario,
                usuario_cpf_cnpj = cpf,
                usuario_id = novoUsuario.id,
            };
            novoUsuario.cliente_pessoa.Add(pessoa);

            vetDb.usuario.Add(novoUsuario);
            vetDb.SaveChanges();
        }
    }
}
