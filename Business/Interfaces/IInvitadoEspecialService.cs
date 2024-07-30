using ApiEventos.Data;

namespace ApiEventos.Services
{
    public interface IInvitadoEspecialService
    {
        Task<List<InvitadoEspecialResponse>> Get(int idInvitado, string? nombre, int page, int pageSize);
        Task<InvitadoEspecialResponse> Create(InvitadoEspecialRequest invitadoEspecialRequest, int usuario);
        Task<bool> Update(int id, InvitadoEspecialRequest invitadoEspecialRequest);
        Task<bool> Delete(int id);
    }
}
