using System;
using System.Collections.Generic;

namespace ApiEventos.Data.Models;

public partial class InvitadosEspeciale
{
    public int IdInvitado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Invitacione> Invitaciones { get; set; } = new List<Invitacione>();

    public virtual Usuario UsuarioRegistroNavigation { get; set; } = null!;
}
