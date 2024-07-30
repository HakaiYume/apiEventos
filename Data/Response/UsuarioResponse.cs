using System;

namespace ApiEventos.Data
{
    public partial class UsuarioResponse
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public bool EsAdmin { get; set; }
        public DateTime FechaRegistro { get; set; }
        public object EventosRegistrados { get; set; } = new object();
    }
}
