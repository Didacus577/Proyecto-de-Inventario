using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class Marcas
{
    public int IdMarca { get; set; }

    public string NombreMarca { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
