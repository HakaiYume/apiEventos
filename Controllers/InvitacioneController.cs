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
    public class InvitacioneController : ControllerBase
    {
        private readonly IInvitacioneService _service;

        public InvitacioneController(IInvitacioneService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int IdInvitacion = default, int? IdEvento = null, int? IdInvitado = null, int Page = 1, int PageSize = 10)
        {
            try
            {
                var result = await _service.Get(IdInvitacion, IdEvento, IdInvitado, Page, PageSize);
                if (result.Count == 0) return MsgNotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(InvitacioneRequest invitacione)
        {
            try
            {
                var usuario = GetUsuario();
                if (usuario == null) return MsgUnauthorized();

                var result = await _service.Create(invitacione, usuario.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, InvitacioneRequest invitacione)
        {
            try
            {
                if (GetUsuario() == null) return MsgUnauthorized();

                var result = await _service.Update(id, invitacione);
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
                if (GetUsuario() == null) return MsgUnauthorized();

                var result = await _service.Delete(id);
                if (!result) return MsgNotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        private int? GetUsuario(){
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Replace("Bearer ", "");
            if(token == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role != "Admin") return null;

            return Convert.ToInt32(userId);
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
            return NotFound(new { Mensaje = "Invitación no encontrada." });
        }
    }
}
