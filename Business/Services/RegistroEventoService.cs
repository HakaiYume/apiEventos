using ApiEventos.Data;
using ApiEventos.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Services
{
    public class RegistroEventoService: IRegistroEventoService
    {
        private readonly DwiApieventosContext _context;
        private IConfiguration config;

        public RegistroEventoService(DwiApieventosContext context, IConfiguration configuration)
        {
            _context = context;
            config = configuration;
        }

        public async Task<List<RegistroEventoResponse>> Get(int idRegistro, int? idEvento, int? idUsuario, int page, int pageSize)
        {
            var result = await _context.RegistroEventos
                .Where(r =>
                    (idRegistro == default || r.IdRegistro == idRegistro) &&
                    (idEvento == default || r.IdEvento == idEvento) &&
                    (idUsuario == default || r.IdUsuario == idUsuario))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var list = new List<RegistroEventoResponse>();
            foreach (var item in result)
            {
                list.Add(MapRegistroEventoResponse(item));
            }

            return list;
        }

        public async Task<RegistroEventoResponse> Create(RegistroEventoRequest registroEventoRequest)
        {
            var registroEvento = MapRegistroEvento(registroEventoRequest);

            _context.RegistroEventos.Add(registroEvento);
            await _context.SaveChangesAsync();

            return await Task.FromResult(MapRegistroEventoResponse(registroEvento));
        }

        public async Task<bool> Update(int id, RegistroEventoRequest registroEventoRequest)
        {
            var registroEvento = await GetById(id);
            if (registroEvento == null) return false;

            registroEvento.IdEvento = registroEventoRequest.IdEvento;
            registroEvento.IdUsuario = registroEventoRequest.IdUsuario;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var registroEvento = await GetById(id);
            if (registroEvento == null) return false;

            _context.RegistroEventos.Remove(registroEvento);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<RegistroEvento?> GetById(int? id)
        {
            var result = await _context.RegistroEventos.FirstOrDefaultAsync(r => r.IdRegistro == id);
            if (result == null) return null;

            return await Task.FromResult(result);
        }

        private RegistroEventoResponse MapRegistroEventoResponse(RegistroEvento registroEvento)
        {
            return new RegistroEventoResponse
            {
                IdRegistro = registroEvento.IdRegistro,
                IdEvento = registroEvento.IdEvento,
                IdUsuario = registroEvento.IdUsuario,
                FechaRegistro = registroEvento.FechaRegistro
            };
        }

        private RegistroEvento MapRegistroEvento(RegistroEventoRequest registroEventoRequest)
        {
            return new RegistroEvento
            {
                IdEvento = registroEventoRequest.IdEvento,
                IdUsuario = registroEventoRequest.IdUsuario,
                FechaRegistro = DateTime.Now
            };
        }
    }
}
