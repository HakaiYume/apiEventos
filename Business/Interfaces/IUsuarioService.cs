using ApiEventos.Data;

namespace ApiEventos.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioResponse>> Get(int idUsuario, string? nombre, string? apellido, string? correoElectronico, int page, int pageSize);
        Task<UsuarioResponse> Create(UsuarioRequest usuarioRequest);
        Task<bool> Update(int id, UsuarioRequest usuarioRequest);
        Task<bool> Delete(int id);
    }
}
