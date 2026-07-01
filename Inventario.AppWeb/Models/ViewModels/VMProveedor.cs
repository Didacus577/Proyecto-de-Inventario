using System.ComponentModel.DataAnnotations;

namespace Inventario.AplicacionWeb.Models.ViewModels
{
    public class VMProveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [StringLength(100,  ErrorMessage = "El nombre no debe tenes mas de {1} caracteres.")]
        [Display(Name = "Nombre del proveedor")]
        public string NombreProveedor { get; set; } = null!;

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los {1} caracteres.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede exceder los {1} caracteres.")]
        [Display(Name = "Correo Electrónico")]
        public string? Correo { get; set; }

        [StringLength(250, ErrorMessage = "La dirección no puede exceder los {1} caracteres.")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }
    }
}