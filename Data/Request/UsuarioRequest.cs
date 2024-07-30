using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEventos.Data
{
    public partial class UsuarioRequest
    {
        [Required(ErrorMessage = "El campo 'Nombre' es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo 'Nombre' no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Apellido' es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo 'Apellido' no puede exceder los 50 caracteres.")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'CorreoElectronico' es requerido.")]
        [EmailAddress(ErrorMessage = "El campo 'CorreoElectronico' no es una dirección de correo electrónico válida.")]
        [MaxLength(100, ErrorMessage = "El campo 'CorreoElectronico' no puede exceder los 100 caracteres.")]
        public string CorreoElectronico { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Contraseña' es requerido.")]
        [MinLength(6, ErrorMessage = "El campo 'Contraseña' debe tener al menos 6 caracteres.")]
        [MaxLength(100, ErrorMessage = "El campo 'Contraseña' no puede exceder los 100 caracteres.")]
        public string Contraseña { get; set; } = null!;
    }
}
