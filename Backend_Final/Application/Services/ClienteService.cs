using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Cliente;
using Backend_Final.Application.Services.Interfaces;
using Backend_Final.Domain.Models;
using Backend_Final.Infrastructure.Repositories.Interfaces;

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
