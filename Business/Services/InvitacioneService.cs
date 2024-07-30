using ApiEventos.Data;
using ApiEventos.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEventos.Services
{
    public class InvitacioneService : IInvitacioneService
    {
        private readonly DwiApieventosContext _context;
        private IConfiguration config;

        public InvitacioneService(DwiApieventosContext context, IConfiguration configuration)
        {
            _context = context;
            config = configuration;
        }

        public async Task<List<InvitacioneResponse>> Get(int idInvitacion, int? idEvento, int? idInvitado, int page, int pageSize)
        {
            var result = await _context.Invitaciones
                .Include(i => i.UsuarioRegistroNavigation)
                .Where(i =>
                    (idInvitacion == default || i.IdInvitacion == idInvitacion) &&
                    (idEvento == default || i.IdEvento == idEvento) &&
                    (idInvitado == default || i.IdInvitado == idInvitado))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var list = new List<InvitacioneResponse>();
            foreach (var item in result)
            {
                list.Add(MapInvitacioneResponse(item));
            }

            return list;
        }

        public async Task<InvitacioneResponse> Create(InvitacioneRequest invitacioneRequest, int usuario)
        {
            var invitacione = MapInvitacione(invitacioneRequest, usuario);

            _context.Invitaciones.Add(invitacione);
            await _context.SaveChangesAsync();

            return await Task.FromResult(MapInvitacioneResponse(invitacione));
        }

        public async Task<bool> Update(int id, InvitacioneRequest invitacioneRequest)
        {
            var invitacione = await GetById(id);
            if (invitacione == null) return false;

            invitacione.IdEvento = invitacioneRequest.IdEvento;
            invitacione.IdInvitado = invitacioneRequest.IdInvitado;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var invitacione = await GetById(id);
            if (invitacione == null) return false;

            _context.Invitaciones.Remove(invitacione);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<Invitacione?> GetById(int? id)
        {
            var result = await _context.Invitaciones.FirstOrDefaultAsync(i => i.IdInvitacion == id);
            if (result == null) return null;

            return await Task.FromResult(result);
        }

        private InvitacioneResponse MapInvitacioneResponse(Invitacione invitacione)
        {
            var usuarioresgistro = new object();
            if (invitacione.UsuarioRegistroNavigation != null){
                usuarioresgistro = new {
                    IdUsuario = invitacione.UsuarioRegistroNavigation.IdUsuario,
                    Nombre = invitacione.UsuarioRegistroNavigation.Nombre + " " + invitacione.UsuarioRegistroNavigation.Apellido,
                    CorreoElectronico = invitacione.UsuarioRegistroNavigation.CorreoElectronico
                };
            }

            return new InvitacioneResponse
            {
                IdInvitacion = invitacione.IdInvitacion,
                IdEvento = invitacione.IdEvento,
                IdInvitado = invitacione.IdInvitado,
                UsuarioRegistro = usuarioresgistro,
                FechaRegistro = invitacione.FechaRegistro
            };
        }

        private Invitacione MapInvitacione(InvitacioneRequest invitacione, int usuario)
        {
            return new Invitacione
            {
                IdEvento = invitacione.IdEvento,
                IdInvitado = invitacione.IdInvitado,
                UsuarioRegistro = usuario,
                FechaRegistro = DateTime.Now
            };
        }
    }
}
