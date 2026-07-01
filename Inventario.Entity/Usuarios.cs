using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class Usuarios
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Roles IdRolNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
}
