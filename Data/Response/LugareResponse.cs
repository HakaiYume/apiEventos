using System;

namespace ApiEventos.Data
{
    public partial class LugareResponse
    {
        public int IdLugarEvento { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }
}
