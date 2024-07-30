using ApiEventos.Data;
using ApiEventos.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Services
{
    public class InvitadoEspecialService : IInvitadoEspecialService
    {
        private readonly DwiApieventosContext _context;
        private IConfiguration config;

        public InvitadoEspecialService(DwiApieventosContext context, IConfiguration configuration)
        {
            _context = context;
            config = configuration;
        }

        public async Task<List<InvitadoEspecialResponse>> Get(int idInvitado, string? nombre, int page, int pageSize)
        {
            var result = await _context.InvitadosEspeciales
                .Include(i => i.UsuarioRegistroNavigation)
                .Where(i =>
                    (idInvitado == default || i.IdInvitado == idInvitado) &&
                    (string.IsNullOrEmpty(nombre) || i.Nombre.Contains(nombre)))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var list = new List<InvitadoEspecialResponse>();
            foreach (var item in result)
            {
                list.Add(MapInvitadoEspecialResponse(item));
            }

            return list;
        }

        public async Task<InvitadoEspecialResponse> Create(InvitadoEspecialRequest invitadoEspecialRequest, int usuario)
        {
            var invitadoEspecial = MapInvitadoEspecial(invitadoEspecialRequest, usuario);

            _context.InvitadosEspeciales.Add(invitadoEspecial);
            await _context.SaveChangesAsync();

            return await Task.FromResult(MapInvitadoEspecialResponse(invitadoEspecial));
        }

        public async Task<bool> Update(int id, InvitadoEspecialRequest invitadoEspecialRequest)
        {
            var invitadoEspecial = await GetById(id);
            if (invitadoEspecial == null) return false;

            invitadoEspecial.Nombre = invitadoEspecialRequest.Nombre;
            invitadoEspecial.Descripcion = invitadoEspecialRequest.Descripcion;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var invitadoEspecial = await GetById(id);
            if (invitadoEspecial == null) return false;

            _context.InvitadosEspeciales.Remove(invitadoEspecial);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<InvitadosEspeciale?> GetById(int? id)
        {
            var result = await _context.InvitadosEspeciales.FirstOrDefaultAsync(i => i.IdInvitado == id);
            if (result == null) return null;

            return await Task.FromResult(result);
        }

        private InvitadoEspecialResponse MapInvitadoEspecialResponse(InvitadosEspeciale invitadoEspecial)
        {
            var usuarioresgistro = new object();
            if (invitadoEspecial.UsuarioRegistroNavigation != null){
                usuarioresgistro = new {
                    IdUsuario = invitadoEspecial.UsuarioRegistroNavigation.IdUsuario,
                    Nombre = invitadoEspecial.UsuarioRegistroNavigation.Nombre + " " + invitadoEspecial.UsuarioRegistroNavigation.Apellido,
                    CorreoElectronico = invitadoEspecial.UsuarioRegistroNavigation.CorreoElectronico
                };
            }

            return new InvitadoEspecialResponse
            {
                IdInvitado = invitadoEspecial.IdInvitado,
                Nombre = invitadoEspecial.Nombre,
                Descripcion = invitadoEspecial.Descripcion,
                UsuarioRegistro = usuarioresgistro,
                FechaRegistro = invitadoEspecial.FechaRegistro
            };
        }

        private InvitadosEspeciale MapInvitadoEspecial(InvitadoEspecialRequest invitadoEspecialRequest, int usuario)
        {
            return new InvitadosEspeciale
            {
                Nombre = invitadoEspecialRequest.Nombre,
                Descripcion = invitadoEspecialRequest.Descripcion,
                UsuarioRegistro = usuario,
                FechaRegistro = DateTime.Now
            };
        }
    }
}
