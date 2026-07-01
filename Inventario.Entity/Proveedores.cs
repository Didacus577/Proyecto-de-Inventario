using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class Proveedores
{
    public int IdProveedor { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
