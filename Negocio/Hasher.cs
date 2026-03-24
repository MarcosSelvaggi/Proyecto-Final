using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Hasher
    {
        public static string HashPassword(string password)
        {
            //Retorna un hash diferente cada vez que se usa, haciendo que sea imposible logearse
            /*
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)       
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
            */
            return "";

        }
    }
}
