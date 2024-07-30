using System;

namespace ApiEventos.Data
{
    public partial class InvitadoEspecialResponse
    {
        public int IdInvitado { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public object UsuarioRegistro { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
