using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEventos.Data
{
    public partial class InvitacioneRequest
    {
        [Required(ErrorMessage = "El campo 'IdEvento' es requerido.")]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "El campo 'IdInvitado' es requerido.")]
        public int IdInvitado { get; set; }
    }
}
