using ApiEventos.Data;
using ApiEventos.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Services
{
    public class UsuarioService: IUsuarioService
    {
        private readonly DwiApieventosContext _context;
        private IConfiguration config;

        public UsuarioService(DwiApieventosContext context, IConfiguration configuration)
        {
            _context = context;
            config = configuration;
        }

        public async Task<List<UsuarioResponse>> Get(int idUsuario, string? nombre, string? apellido, string? correoElectronico, int page, int pageSize)
        {
            var result = await _context.Usuarios
                .Include(u => u.RegistroEventos)
                .ThenInclude(r => r.IdEventoNavigation)
                .Where(u =>
                    (idUsuario == default || u.IdUsuario == idUsuario) &&
                    (string.IsNullOrEmpty(nombre) || u.Nombre.Contains(nombre)) &&
                    (string.IsNullOrEmpty(apellido) || u.Apellido.Contains(apellido)) &&
                    (string.IsNullOrEmpty(correoElectronico) || u.CorreoElectronico.Contains(correoElectronico)))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var list = new List<UsuarioResponse>();
            foreach (var item in result)
            {
                list.Add(MapUsuarioResponse(item));
            }

            return list;
        }

        public async Task<UsuarioResponse> Create(UsuarioRequest usuarioRequest)
        {
            var usuario = MapUsuario(usuarioRequest);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return await Task.FromResult(MapUsuarioResponse(usuario));
        }

        public async Task<bool> Update(int id, UsuarioRequest usuarioRequest)
        {
            var usuario = await GetById(id);
            if (usuario == null) return false;

            usuario.Nombre = usuarioRequest.Nombre;
            usuario.Apellido = usuarioRequest.Apellido;
            usuario.CorreoElectronico = usuarioRequest.CorreoElectronico;
            usuario.Contraseña = usuarioRequest.Contraseña;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var usuario = await GetById(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<Usuario?> GetById(int? id)
        {
            var result = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (result == null) return null;

            return await Task.FromResult(result);
        }

        private UsuarioResponse MapUsuarioResponse(Usuario usuario)
        {
            var InvitadoEspecial = new object();
            if (usuario.RegistroEventos != null && usuario.RegistroEventos.Any()){
                 InvitadoEspecial = usuario.RegistroEventos.Select(r => new {
                    IdEvento = r.IdEventoNavigation.IdEvento,
                    NombreEvento = r.IdEventoNavigation.NombreEvento,
                    FechaEvento = r.IdEventoNavigation.FechaEvento,
                    HoraEvento = r.IdEventoNavigation.HoraEvento
                }).ToList();
            }

            return new UsuarioResponse
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                CorreoElectronico = usuario.CorreoElectronico,
                Contraseña = usuario.Contraseña,
                EsAdmin = usuario.EsAdmin,
                FechaRegistro = usuario.FechaRegistro,
                EventosRegistrados = InvitadoEspecial
            };
        }

        private Usuario MapUsuario(UsuarioRequest usuario)
        {
            return new Usuario
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                CorreoElectronico = usuario.CorreoElectronico,
                Contraseña = usuario.Contraseña,
                EsAdmin = false,
                FechaRegistro = DateTime.Now
            };
        }
    }
}
