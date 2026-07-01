using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class UnidadesMedida
{
    public int IdUnidad { get; set; }

    public string NombreUnidad { get; set; } = null!;

    public string Abreviatura { get; set; } = null!;

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
