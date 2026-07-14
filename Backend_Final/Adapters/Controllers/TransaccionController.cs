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
        private readonly CriptoYaService _criptoYaService;

        public TransaccionController(
            ITransaccionService transaccionService,
            CriptoYaService criptoYaService)
        {
            _transaccionService = transaccionService;
            _criptoYaService = criptoYaService;
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
        public async Task<IActionResult> ObtenerTransacciones([FromQuery] ObtenerTransaccionesFiltroDto dto)
        {
            var result = await _transaccionService.ObtenerTransaccionesAsync(dto);

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTransaccionID(int id)
        {
            var result = await _transaccionService.ObtenerTransaccionIdAsync(id);

            return result.ToActionResult();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditarTransaccion(int id, [FromBody] EditarTransaccionDto dto)
        {
            var result = await _transaccionService.EditarTransaccionAsync(id, dto);

            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarTransaccion(int id)
        {
            var result = await _transaccionService.EliminarTransaccionAsync(id);

            return result.ToActionResult();
        }

        [HttpGet("ranking")]
        public async Task<IActionResult> ObtenerRanking(string crypto, string tipo)
        {
            var result = await _criptoYaService.ObtenerRanking(crypto, tipo);

            return result.ToActionResult();
        }
    }
}
