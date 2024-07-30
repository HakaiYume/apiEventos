using ApiEventos.Data;
using ApiEventos.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEventos.Services
{
    public class EventoService: IEventoService
    {
        private readonly DwiApieventosContext _context;
        private IConfiguration config;

        public EventoService(DwiApieventosContext context, IConfiguration configuration)
        {
            _context = context;
            config = configuration;
        }

        public async Task<List<EventoResponse>> Get(int idEvento, string? nombreEvento, DateOnly? fechaEvento, string? estadoEvento, int page, int pageSize)
        {
            var result = await _context.Eventos
                .Include(e => e.IdLugarEventoNavigation)
                .Include(e=> e.UsuarioRegistroNavigation)
                .Include(e => e.Invitaciones)
                .ThenInclude(i => i.IdInvitadoNavigation)
                .Where(e =>
                    (idEvento == default || e.IdEvento == idEvento) &&
                    (string.IsNullOrEmpty(nombreEvento) || e.NombreEvento.Contains(nombreEvento)) &&
                    (fechaEvento == default || e.FechaEvento == fechaEvento) &&
                    (string.IsNullOrEmpty(estadoEvento) || e.EstadoEvento.Contains(estadoEvento)))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                var List = new List<EventoResponse>();
                foreach (var Item in result){
                    List.Add(MapEventoResponse(Item));
                }

            return List;
        }

        public async Task<EventoResponse> Create(EventoRequest eventoRequest, int usuario)
        {
            var lugare = MapLugare(eventoRequest);
            var evento = MapEvento(eventoRequest, usuario);
            evento.IdLugarEventoNavigation = lugare;
            
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            if(eventoRequest.InvitadosEspeciales is not null && eventoRequest.InvitadosEspeciales.Any()){
                foreach(int Invitado in eventoRequest.InvitadosEspeciales){
                    var invitacione = MapInvitacione(Invitado, evento.IdEvento, usuario);
                    
                    _context.Invitaciones.Add(invitacione);
                    await _context.SaveChangesAsync();
                }
            }

            return await Task.FromResult(Get(evento.IdEvento, null, null, null,1,1).Result.First());
        }

        public async Task<bool> Update(int id, EventoRequest eventoRequest)
        {
            var evento = await GetById(id);
            if (evento == null) return false;

            evento.NombreEvento = eventoRequest.NombreEvento;
            evento.FechaEvento = eventoRequest.FechaEvento;
            evento.HoraEvento = eventoRequest.HoraEvento;
            evento.DescripcionEvento = eventoRequest.DescripcionEvento;
            evento.CostoEvento = eventoRequest.CostoEvento;
            evento.EstadoEvento = eventoRequest.EstadoEvento;
            evento.IdLugarEventoNavigation.Descripcion = eventoRequest.DescripcionLugar;
            evento.IdLugarEventoNavigation.Latitud = eventoRequest.Latitud;
            evento.IdLugarEventoNavigation.Longitud = eventoRequest.Longitud;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var evento = await GetById(id);
            if (evento == null) return false;

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> UsuariosRegistrados(int id)
        {
            var result = await _context.Eventos
                .Where(e => e.IdEvento == id)
                .Select(e => e.RegistroEventos)
                .ToListAsync();

            return await Task.FromResult(result.Count);
        }

        private async Task<Evento?> GetById(int id)
        {
            var result = await _context.Eventos.Include(e => e.IdLugarEventoNavigation).FirstOrDefaultAsync(e=> e.IdEvento == id);
            if (result == null) return null;

            return await Task.FromResult(result);
        }

        private EventoResponse MapEventoResponse(Evento evento){
            var usuarioresgistro = new object();
            if (evento.UsuarioRegistroNavigation != null){
                usuarioresgistro = new {
                    IdUsuario = evento.UsuarioRegistroNavigation.IdUsuario,
                    Nombre = evento.UsuarioRegistroNavigation.Nombre + " " + evento.UsuarioRegistroNavigation.Apellido,
                    CorreoElectronico = evento.UsuarioRegistroNavigation.CorreoElectronico
                };
            }

            var LugareResponse = new LugareResponse();
            if(evento.IdLugarEventoNavigation != null){
                LugareResponse= new LugareResponse {
                    IdLugarEvento = evento.IdLugarEventoNavigation.IdLugarEvento,
                    Descripcion = evento.IdLugarEventoNavigation.Descripcion,
                    Latitud = evento.IdLugarEventoNavigation.Latitud,
                    Longitud = evento.IdLugarEventoNavigation.Longitud
                };
            }

            var InvitadoEspecial = new List<InvitadoEspecialResponse>();
            if (evento.Invitaciones != null && evento.Invitaciones.Any()){
                InvitadoEspecial = evento.Invitaciones.Select(x => new InvitadoEspecialResponse {
                    IdInvitado = x.IdInvitadoNavigation.IdInvitado,
                    Nombre = x.IdInvitadoNavigation.Nombre,
                    Descripcion = x.IdInvitadoNavigation.Descripcion
                }).ToList();
            }

            return new EventoResponse
            {
                IdEvento = evento.IdEvento,
                NombreEvento = evento.NombreEvento,
                FechaEvento = evento.FechaEvento,
                HoraEvento = evento.HoraEvento,
                DescripcionEvento = evento.DescripcionEvento,
                CostoEvento = evento.CostoEvento,
                Lugare = LugareResponse,
                EstadoEvento = evento.EstadoEvento,
                UsuarioRegistro = usuarioresgistro,
                FechaCreacion = evento.FechaRegistro,
                InvitadosEspeciales = InvitadoEspecial
            };
        }

        private Evento MapEvento(EventoRequest evento, int usuario){
            return new Evento
            {
                NombreEvento = evento.NombreEvento,
                FechaEvento = evento.FechaEvento,
                HoraEvento = evento.HoraEvento,
                DescripcionEvento = evento.DescripcionEvento,
                CostoEvento = evento.CostoEvento,
                EstadoEvento = evento.EstadoEvento,
                UsuarioRegistro = usuario,
                FechaRegistro = DateTime.Now
            };
        }

        private Lugare MapLugare(EventoRequest lugare)
        {
            return new Lugare
            {
                Descripcion = lugare.DescripcionLugar,
                Latitud = lugare.Latitud,
                Longitud = lugare.Longitud
            };
        }

        private Invitacione MapInvitacione(int IdInvitado, int IdEvento, int usuario)
        {
            return new Invitacione
            {
                IdEvento = IdEvento,
                IdInvitado = IdInvitado,
                UsuarioRegistro = usuario,
                FechaRegistro = DateTime.Now
            };
        }
    }
}
