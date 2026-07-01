using System;
using System.ComponentModel.DataAnnotations;

namespace Inventario.AppWeb.Models.ViewModels
{
    public class VMMarca
    {
        public int IdMarca { get; set; }

        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre de la Marca")]
        public string NombreMarca { get; set; } = string.Empty;


        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede tener más de {1} caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}