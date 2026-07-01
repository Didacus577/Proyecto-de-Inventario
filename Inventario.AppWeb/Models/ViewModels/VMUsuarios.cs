using System.ComponentModel.DataAnnotations;

namespace Inventario.AppWeb.Models.ViewModels
{
    public class VMUsuarios
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre completo")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede exceder los {1} caracteres.")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Clave { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un rol para el usuario.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un rol válido.")]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [Display(Name = "Nombre del Rol")]
        public string? NombreRol { get; set; }
    }
}