using System;
using System.Collections.Generic;

namespace ApiEventos.Data.Models;

public partial class Invitacione
{
    public int IdInvitacion { get; set; }

    public int IdEvento { get; set; }

    public int IdInvitado { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual InvitadosEspeciale IdInvitadoNavigation { get; set; } = null!;

    public virtual Usuario UsuarioRegistroNavigation { get; set; } = null!;
}
