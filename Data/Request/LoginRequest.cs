using System.ComponentModel.DataAnnotations;

namespace ApiEventos.Data
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El campo 'CorreoElectronico' es requerido.")]
        [EmailAddress(ErrorMessage = "El campo 'CorreoElectronico' debe ser una dirección de correo válida.")]
        public string CorreoElectronico { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Contraseña' es requerido.")]
        public string Contraseña { get; set; } = null!;
    }
}
