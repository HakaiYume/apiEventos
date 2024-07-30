using ApiEventos.Data;

namespace ApiEventos.Services
{
    public interface IEventoService
    {
        Task<List<EventoResponse>> Get(int idEvento, string? nombreEvento, DateOnly? fechaEvento, string? estadoEvento, int page, int pageSize);
        Task<EventoResponse> Create(EventoRequest eventoRequest, int usuario);
        Task<bool> Update(int id, EventoRequest eventoRequest);
        Task<bool> Delete(int id);
        Task<int> UsuariosRegistrados(int id);
    }
}