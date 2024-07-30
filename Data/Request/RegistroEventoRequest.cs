using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEventos.Data
{
    public partial class RegistroEventoRequest
    {
        [Required(ErrorMessage = "El campo 'IdEvento' es requerido.")]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "El campo 'IdUsuario' es requerido.")]
        public int IdUsuario { get; set; }
    }
}
