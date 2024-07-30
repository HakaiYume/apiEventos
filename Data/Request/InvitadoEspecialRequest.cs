using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEventos.Data
{
    public partial class InvitadoEspecialRequest
    {
        [Required(ErrorMessage = "El campo 'Nombre' es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo 'Nombre' no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Descripcion' es requerido.")]
        [MaxLength(200, ErrorMessage = "El campo 'Descripcion' no puede exceder los 200 caracteres.")]
        public string Descripcion { get; set; } = null!;
    }
}
