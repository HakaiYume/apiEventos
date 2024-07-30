using Microsoft.AspNetCore.Mvc;
using ApiEventos.Services;
using ApiEventos.Data;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int IdUsuario = default, string? Nombre = null, string? Apellido = null, string? CorreoElectronico = null, int Page = 1, int PageSize = 10)
        {
            try
            {
                var usuario = GetUsuario();
                if (usuario == null) return MsgUnauthorized();

                var authorize = usuario.EsAdmin || usuario.IdUsuario == IdUsuario;
                if (!authorize) return MsgUnauthorized();

                var result = await _service.Get(IdUsuario, Nombre, Apellido, CorreoElectronico, Page, PageSize);
                if (result.Count == 0) return MsgNotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UsuarioRequest usuario)
        {
            try
            {
                var result = await _service.Create(usuario);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UsuarioRequest usuario)
        {
            try
            {
                var _usuario = GetUsuario();
                if (_usuario == null) return MsgUnauthorized();

                var authorize = _usuario.EsAdmin || _usuario.IdUsuario == id;
                if (!authorize) return MsgUnauthorized();

                var result = await _service.Update(id, usuario);
                if (!result) return MsgNotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var _usuario = GetUsuario();
                if (_usuario == null) return MsgUnauthorized();

                var authorize = _usuario.EsAdmin || _usuario.IdUsuario == id;
                if (!authorize) return MsgUnauthorized();
                
                var result = await _service.Delete(id);
                if (!result) return MsgNotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        private _Usuario? GetUsuario(){
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Replace("Bearer ", "");
            if(token == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return new _Usuario{
                IdUsuario = Convert.ToInt32(userId),
                EsAdmin = role == "Admin"? true : false,
            };
        }

        private IActionResult MsgUnauthorized(){
            return Unauthorized(new { message = "No has iniciado sesion o no tienes lo permisos necesarios" });
        }

        private IActionResult MsgError(Exception ex)
        {
            return StatusCode(500, new { Mensaje = "Ha ocurrido un error inesperado", Error = ex.Message });
        }

        private IActionResult MsgNotFound()
        {
            return NotFound(new { message = "Usuario no encontrado." });
        }
    }

    public partial class _Usuario
    {
        public int IdUsuario { get; set; }
        public bool EsAdmin { get; set; }
    }
}
