using System;
using System.Collections.Generic;

namespace ApiEventos.Data.Models;

public partial class RegistroEvento
{
    public int IdRegistro { get; set; }

    public int IdEvento { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
