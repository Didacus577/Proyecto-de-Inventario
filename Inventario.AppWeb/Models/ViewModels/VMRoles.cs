using System.ComponentModel.DataAnnotations;

namespace Inventario.AppWeb.Models.ViewModels
{
    public class VMRoles
    {
        [Key]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre del Rol")]
        public string NombreRol { get; set; } = null!;
    }
}