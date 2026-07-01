using System;
using System.ComponentModel.DataAnnotations;

namespace Inventario.AppWeb.Models.ViewModels
{
    public class VMMovimiento
    {
        [Key]
        public int IdMovimiento { get; set; }

        [Required(ErrorMessage = "La fecha del movimiento es obligatoria.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha y Hora")]
        public DateTime FechaMovimiento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un producto válido.")]
        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        [Display(Name = "Nombre del Producto")]
        public string? NombreProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
        [RegularExpression("^(Entrada|Salida)$", ErrorMessage = "El tipo de movimiento debe ser 'Entrada' o 'Salida'.")]
        [Display(Name = "Tipo de Movimiento")]
        public string? TipoMovimiento { get; set; }
       
        public int IdUsuario { get; set; }

        [Display(Name = "Usuario que registró")]
        public string? NombreUsuario { get; set; }
    }
}