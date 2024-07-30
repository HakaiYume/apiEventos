using System;
using System.Collections.Generic;

namespace ApiEventos.Data.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public bool EsAdmin { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual ICollection<Invitacione> Invitaciones { get; set; } = new List<Invitacione>();

    public virtual ICollection<InvitadosEspeciale> InvitadosEspeciales { get; set; } = new List<InvitadosEspeciale>();

    public virtual ICollection<RegistroEvento> RegistroEventos { get; set; } = new List<RegistroEvento>();
}
