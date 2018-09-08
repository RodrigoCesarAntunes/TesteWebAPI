using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace WebApiVeterinario.Encrypt
{
    public class Password
    {
        public string EncryptPassword(string senha)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(senha, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string senhaSalvaHash = Convert.ToBase64String(hashBytes);

            return senhaSalvaHash;
        }

        public bool ComparePassword(string senhaSalva, string senha)
        {
            byte[] hBytes = Convert.FromBase64String(senhaSalva);

            //get Salt
            byte[] salt = new byte[16];
            Array.Copy(hBytes, 0, salt, 0, 16);

            //compute the hash on the password

            var pdkdf2 = new Rfc2898DeriveBytes(senha, salt, 1000);
            byte[] hash = pdkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hBytes[i + 16] != hash[i])
                {
                    return false;
                }
            return true;
        }
    }
}