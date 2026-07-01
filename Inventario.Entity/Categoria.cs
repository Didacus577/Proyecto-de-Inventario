using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
