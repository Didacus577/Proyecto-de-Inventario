using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class MovimientosInventario
{
    public int IdMovimiento { get; set; }

    public int IdProducto { get; set; }

    public int IdUsuario { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public int Cantidad { get; set; }

    public DateTime FechaMovimiento { get; set; }

    public virtual Productos IdProductoNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
