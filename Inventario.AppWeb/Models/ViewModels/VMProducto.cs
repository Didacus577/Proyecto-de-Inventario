using System.ComponentModel.DataAnnotations;

namespace Inventario.AppWeb.Models.ViewModels
{
    public class VMProducto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no debe tenes mas de {1} caracteres.")]
        [Display(Name = "Nombre del Producto")]
        public string NombreProducto { get; set; } = null!;

        [StringLength(250, ErrorMessage = "La descripción no puede exceder los {1} caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser un número negativo.")]
        [Display(Name = "Stock Actual")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El stock mínimo es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser un número negativo.")]
        [Display(Name = "Stock Mínimo")]
        public int StockMinimo { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]        
        public bool Estado { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        [Display(Name = "Categoría")]
        public int IdCategoria { get; set; }

        [Display(Name = "Categoría")]
        public string? NombreCategoria { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una marca.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una marca válida.")]
        [Display(Name = "Marca")]
        public int IdMarca { get; set; }

        [Display(Name = "Marca")]
        public string? NombreMarca { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un proveedor válido.")]
        [Display(Name = "Proveedor")]
        public int IdProveedor { get; set; }

        [Display(Name = "Proveedor")]
        public string? NombreProveedor { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una unidad de medida.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una unidad válida.")]
        [Display(Name = "Unidad de Medida")]
        public int IdUnidad { get; set; }

        [Display(Name = "Unidad")]
        public string? NombreUnidad { get; set; }

        [Display(Name = "Fecha de Expiración")]
        // Si lo manejas como String (por ejemplo, para recibir "yyyy-MM-dd" desde el input de HTML)
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "El formato de fecha debe ser AAAA-MM-DD.")]
        public string? FechaExpiracion { get; set; }
    }
}