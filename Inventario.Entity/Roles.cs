using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class Roles
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
