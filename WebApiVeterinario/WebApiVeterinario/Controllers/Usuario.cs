using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiVeterinario.Models;
using WebApiVeterinario.Encrypt;


namespace WebApiVeterinario.Controllers
{
    public class Usuario
    {
        private VeterinarioEntities vetDb = new VeterinarioEntities();

        public bool? TestarSenha(string _senha, string _email)
        {
            try
            {
               // string senhaCriptografada;
                string senha = (from user in vetDb.Autenticacao
                                where user.Email == _email
                                select user.Senha).Single();
                Password password = new Password();
                //senhaCriptografada = password.ComparePassword(_senha);

                return password.ComparePassword(senha, _senha);
            }
            catch (ArgumentNullException e)
            {
                throw;
            }
            catch (InvalidOperationException e)
            {
                throw;
            }
        }

        


    }
}