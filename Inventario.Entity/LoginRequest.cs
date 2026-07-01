using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Entity
{
    public class LoginRequest
    {
        public string Correo { get; set; } = null!;
        public string Clave { get; set; } = null!;
    }
}
