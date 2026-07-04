using Backend_Final.Application.Common.Extensions;
using Backend_Final.Application.DTOs.Transaccion;
using Backend_Final.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Final.Adapters.Controllers
{
    [Route("api/transacciones")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly ITransaccionService _transaccionService;

        public TransaccionController(
            ITransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTransaccion([FromBody] CrearTransaccionDto dto)
        {
            var result = await _transaccionService.CrearTransaccionAsync(dto);

            if (result.Success)
                return CreatedAtAction(nameof(ObtenerTransaccionID), new { id = result.Data!.Id }, result.Data);

            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTransacciones()
        {
            var result = await _transaccionService.ObtenerTransaccionesAsync();

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTransaccionID(int id)
        {
            var result = await _transaccionService.ObtenerTransaccionIdAsync(id);

            return result.ToActionResult();
        }
    }
}
