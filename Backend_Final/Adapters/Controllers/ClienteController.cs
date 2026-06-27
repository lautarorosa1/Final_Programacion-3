using Backend_Final.Application.Common.Extensions;
using Backend_Final.Application.DTOs.Cliente;
using Backend_Final.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Final.Adapters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteDto dto)
        {
            var result = await _clienteService.CrearClienteAsync(dto);

            if (result.Success)
                return CreatedAtAction(nameof(ObtenerClienteID), new { id = result.Data!.Id }, result.Data);

            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerClientes()
        {
            var result = await _clienteService.ObtenerClientesAsync();

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerClienteID(int id)
        {
            var result = await _clienteService.ObtenerClienteAsync(id);

            return result.ToActionResult();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditarCliente(int id, [FromBody] EditarClienteDto dto)
        {
            var result = await _clienteService.EditarClienteAsync(id, dto);

            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarCliente(int id)
        {
            var result = await _clienteService.EliminarClienteAsync(id);

            return result.ToActionResult();
        }
    }
}
