using Microsoft.AspNetCore.Mvc;
using ApiEventos.Services;
using ApiEventos.Data;

namespace ApiEventos.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var result = await _service.Login(loginRequest);
                if (result == null) return Unauthorized(new { message = "Correo electrónico o contraseña incorrectos." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        private IActionResult MsgError(Exception ex){
            return StatusCode(500, new {Mensaje = "A ocurrido un error inesperado", Error = ex.Message});
        }
    }
}
