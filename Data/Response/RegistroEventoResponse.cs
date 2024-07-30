using System;

namespace ApiEventos.Data
{
    public partial class RegistroEventoResponse
    {
        public int IdRegistro { get; set; }
        public int IdEvento { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
