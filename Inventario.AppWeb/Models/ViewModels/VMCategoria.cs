using System.ComponentModel.DataAnnotations;

namespace Inventario.AppWeb.Models.ViewModels
{
    public class VMCategoria
    {

        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre de la Categoría")]
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede tener más de {1} caracteres.")]
        [Display(Name = "Descripción")]
        public string DescripcionCategoria { get; set; }

    }
}
