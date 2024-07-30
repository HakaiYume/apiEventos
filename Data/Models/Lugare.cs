using System;
using System.Collections.Generic;

namespace ApiEventos.Data.Models;

public partial class Lugare
{
    public int IdLugarEvento { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
