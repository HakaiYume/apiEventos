using ApiEventos.Data;

namespace ApiEventos.Services
{
    public interface IRegistroEventoService
    {
        Task<List<RegistroEventoResponse>> Get(int idRegistro, int? idEvento, int? idUsuario, int page, int pageSize);
        Task<RegistroEventoResponse> Create(RegistroEventoRequest registroEventoRequest);
        Task<bool> Update(int id, RegistroEventoRequest registroEventoRequest);
        Task<bool> Delete(int id);
    }
}
