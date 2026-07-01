using System;
using System.Collections.Generic;

namespace Inventario.Entity;

public partial class Productos
{
    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Stock { get; set; }

    public int StockMinimo { get; set; }

    public bool Estado { get; set; }

    public int IdCategoria { get; set; }

    public int IdMarca { get; set; }

    public int IdProveedor { get; set; }

    public int IdUnidad { get; set; }

    public DateOnly? FechaExpiracion { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Marcas IdMarcaNavigation { get; set; } = null!;

    public virtual Proveedores IdProveedorNavigation { get; set; } = null!;

    public virtual UnidadesMedida IdUnidadNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
}
