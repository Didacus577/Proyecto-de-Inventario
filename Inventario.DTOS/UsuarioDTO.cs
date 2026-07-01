using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.DTOS
{
    public  class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public int IdRol { get; set; }

        public string NombreRol { get; set; }

    }
}
