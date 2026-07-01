using System.ComponentModel.DataAnnotations;

namespace Inventario.AppWeb.Models.ViewModels
{
    public class VMUnidades
    {
        [Key]
        public int IdUnidad { get; set; }

        [Required(ErrorMessage = "El nombre de la unidad es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de {1} caracteres.")]
        [Display(Name = "Unidad de Medida")]
        public string NombreUnidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La abreviatura es obligatoria.")]
        [StringLength(10, ErrorMessage = "La abreviatura no puede tener más de {1} caracteres.")]
        [Display(Name = "Abreviatura")]
        public string Abreviatura { get; set; } = string.Empty;
    }
}