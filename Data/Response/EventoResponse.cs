using System;

namespace ApiEventos.Data
{
    public partial class EventoResponse
    {
        public int IdEvento { get; set; }
        public string NombreEvento { get; set; } = null!;
        public DateOnly FechaEvento { get; set; }
        public TimeOnly HoraEvento { get; set; }
        public string DescripcionEvento { get; set; } = null!;
        public decimal CostoEvento { get; set; }
        public LugareResponse Lugare { get; set; } = null!;
        public string EstadoEvento { get; set; } = null!;
        public object UsuarioRegistro { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public object? InvitadosEspeciales { get; set; }
    }
}
