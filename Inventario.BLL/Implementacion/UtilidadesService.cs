using System.Text;
using System.Security.Cryptography;
using Inventario.BLL.Interfaces;

namespace Inventario.BLL.Implementacion
{
  
    public class UtilidadesService : IUtilidadesService
    {
        public string ConvertirSHA256(string texto)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = hash.ComputeHash(bytes);

                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
