using System;

namespace ApiEventos.Data
{
    public partial class InvitacioneResponse
    {
        public int IdInvitacion { get; set; }
        public int IdEvento { get; set; }
        public int IdInvitado { get; set; }
        public object UsuarioRegistro { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
