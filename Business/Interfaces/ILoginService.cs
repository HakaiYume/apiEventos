using ApiEventos.Data;

namespace ApiEventos.Services
{
    public interface ILoginService
    {
        Task<LoginResponse?> Login(LoginRequest loginRequest);
    }
}
