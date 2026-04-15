using System.ComponentModel.DataAnnotations;

namespace ProyectoPrograAvanzada.Models.ViewModels
{
    public class UsuarioFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Contrasena { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarContrasena { get; set; }
    }
}