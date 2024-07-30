using System;
using System.Collections.Generic;

namespace ApiEventos.Data.Models;

public partial class Evento
{
    public int IdEvento { get; set; }

    public string NombreEvento { get; set; } = null!;

    public DateOnly FechaEvento { get; set; }

    public TimeOnly HoraEvento { get; set; }

    public string DescripcionEvento { get; set; } = null!;

    public decimal CostoEvento { get; set; }

    public int IdLugarEvento { get; set; }

    public string EstadoEvento { get; set; } = null!;

    public int UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Lugare IdLugarEventoNavigation { get; set; } = null!;

    public virtual ICollection<Invitacione> Invitaciones { get; set; } = new List<Invitacione>();

    public virtual ICollection<RegistroEvento> RegistroEventos { get; set; } = new List<RegistroEvento>();

    public virtual Usuario UsuarioRegistroNavigation { get; set; } = null!;
}
