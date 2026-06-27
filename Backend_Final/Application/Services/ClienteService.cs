using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Cliente;
using Backend_Final.Application.Services.Interfaces;
using Backend_Final.Domain.Models;
using Backend_Final.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend_Final.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ResponseClienteDto>> CrearClienteAsync(CrearClienteDto dto)
        {
            // Email unico
            var existe = await _repository.ExisteEmailAsync(dto.Email);

            if (existe) return Result<ResponseClienteDto>.Fail("El email ya está registrado",ResultType.BadRequest);

            var cliente = new Cliente
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim()
            };

            await _repository.CrearClienteAsync(cliente);

            return Result<ResponseClienteDto>.Ok(MapToResponse(cliente));
        }

        public async Task<Result<List<ResponseClienteDto>>> ObtenerClientesAsync()
        {
            var clientes = await _repository.ObtenerClientesAsync();

            var response = clientes.Select(MapToResponse).ToList(); ;

            return Result<List<ResponseClienteDto>>.Ok(response);
        }

        public async Task<Result<ResponseClienteDto>> ObtenerClienteAsync(int id)
        {
            var cliente = await _repository.ObtenerClienteAsync(id);

            if (cliente == null)
                return Result<ResponseClienteDto>.Fail("Cliente no encontrado", ResultType.NotFound);

            var response = MapToResponse(cliente);

            return Result<ResponseClienteDto>.Ok(response);
        }

        private static ResponseClienteDto MapToResponse(Cliente cliente)
        {
            return new ResponseClienteDto
            {
                Id = cliente.Id,
                Name = cliente.Name,
                Email = cliente.Email
            };
        }
    }
}
