using ApiEventos.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEventos.Services
{
    public interface IInvitacioneService
    {
        Task<List<InvitacioneResponse>> Get(int idInvitacion, int? idEvento, int? idInvitado, int page, int pageSize);
        Task<InvitacioneResponse> Create(InvitacioneRequest invitacioneRequest, int usuario);
        Task<bool> Update(int id, InvitacioneRequest invitacioneRequest);
        Task<bool> Delete(int id);
    }
}
