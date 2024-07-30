using Microsoft.AspNetCore.Mvc;
using ApiEventos.Services;
using ApiEventos.Data;

namespace ApiEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroEventoController : ControllerBase
    {
        private readonly IRegistroEventoService _service;

        public RegistroEventoController(IRegistroEventoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int IdRegistro = default, int? IdEvento = null, int? IdUsuario = null, int Page = 1, int PageSize = 10)
        {
            try
            {
                var result = await _service.Get(IdRegistro, IdEvento, IdUsuario, Page, PageSize);
                if (result.Count == 0) return MsgNotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(RegistroEventoRequest registroEvento)
        {
            try
            {
                var result = await _service.Create(registroEvento);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, RegistroEventoRequest registroEvento)
        {
            try
            {
                var result = await _service.Update(id, registroEvento);
                if (!result) return MsgNotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (!result) return MsgNotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return MsgError(ex);
            }
        }

        private IActionResult MsgError(Exception ex)
        {
            return StatusCode(500, new { Mensaje = "Ha ocurrido un error inesperado", Error = ex.Message });
        }

        private IActionResult MsgNotFound()
        {
            return NotFound(new { message = "Registro de evento no encontrado." });
        }
    }
}
