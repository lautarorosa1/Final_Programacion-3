using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Cliente;
using Backend_Final.Application.DTOs.Transaccion;
using Backend_Final.Application.Services.Interfaces;
using Backend_Final.Domain.Models;
using Backend_Final.Infrastructure.Repositories.Interfaces;
using Backend_Final.Migrations;

namespace Backend_TrabajoFinal.Application.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _repository;
        private readonly IClienteRepository _clienteRepository;
        private readonly CriptoYaService _criptoYaService;

        private readonly string[] _criptosValidas = { "btc", "eth", "usdc" };

        public TransaccionService(
            ITransaccionRepository repository,
            IClienteRepository clienteRepository,
            CriptoYaService criptoYaService)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _criptoYaService = criptoYaService;
        }

        public async Task<Result<ResponseTransaccionDto>> CrearTransaccionAsync(CrearTransaccionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TipoTransaccion))
                return Result<ResponseTransaccionDto>.Fail("El tipo de transacción es obligatorio", ResultType.BadRequest);

            var tipo = dto.TipoTransaccion.Trim().ToLower();

            if (tipo != "purchase" && tipo != "sale")
                return Result<ResponseTransaccionDto>.Fail("Tipo de transacción inválido", ResultType.BadRequest);

            var cliente = await _clienteRepository.ObtenerClienteAsync(dto.ClienteId);
            if (cliente == null)
                return Result<ResponseTransaccionDto>.Fail("Cliente no encontrado", ResultType.NotFound);

            var crypto = dto.CodigoCripto.Trim().ToLower();

            if (!_criptosValidas.Contains(crypto))
                return Result<ResponseTransaccionDto>.Fail("Criptomoneda inválida", ResultType.BadRequest);

            if (dto.CantidadCripto <= 0)
                return Result<ResponseTransaccionDto>.Fail("La cantidad debe ser mayor a 0", ResultType.BadRequest);

            if (tipo == "sale")
            {
                var saldo = await CalcularSaldo(dto.ClienteId, crypto);

                if (dto.CantidadCripto > saldo)
                    return Result<ResponseTransaccionDto>.Fail($"Saldo insuficiente. Disponible: {saldo} {crypto.ToUpper()}", ResultType.BadRequest);
            }

            var precioResult = await ObtenerMejorPrecio(dto.Exchange, crypto, tipo);
            if (!precioResult.Success)
                return Result<ResponseTransaccionDto>.Fail(precioResult.ErrorMessage, ResultType.BadRequest);

            var (exchange, valor) = precioResult.Data;

            var transaccion = new Transaccion
            {
                CodigoCripto = crypto,
                TipoTransaccion = tipo,
                ClienteId = dto.ClienteId,
                CantidadCripto = dto.CantidadCripto,
                MontoARS = valor * dto.CantidadCripto,
                Exchange = exchange,
                FechaHora = DateTime.UtcNow
            };

            await _repository.CrearAsync(transaccion);

            return Result<ResponseTransaccionDto>.Ok(MapToResponse(transaccion));
        }

        public async Task<Result<List<ResponseTransaccionDto>>> ObtenerTransaccionesAsync()
        {
            var transacciones = await _repository.ObtenerTransaccionesAsync();

            var response = transacciones.Select(MapToResponse).ToList();

            return Result<List<ResponseTransaccionDto>>.Ok(response);
        }

        public async Task<Result<ResponseTransaccionDto>> ObtenerTransaccionIdAsync(int id)
        {
            var transaccion = await _repository.ObtenerTransaccionIdAsync(id);

            if (transaccion == null)
                return Result<ResponseTransaccionDto>.Fail("Transaccion no encontrada", ResultType.NotFound);

            var response = MapToResponse(transaccion);

            return Result<ResponseTransaccionDto>.Ok(response);
        }

        private static ResponseTransaccionDto MapToResponse(Transaccion transaccion)
        {
            return new ResponseTransaccionDto
            {
                Id = transaccion.Id,
                CodigoCripto = transaccion.CodigoCripto,
                TipoTransaccion = transaccion.TipoTransaccion,
                ClienteId = transaccion.ClienteId,
                CantidadCripto = transaccion.CantidadCripto,
                MontoARS = transaccion.MontoARS,
                Exchange = transaccion.Exchange,
                FechaHora = transaccion.FechaHora
            };
        }

        public async Task<Result<ResponseTransaccionDto>> EditarTransaccionAsync(int id, EditarTransaccionDto dto)
        {
            var transaccion = await _repository.ObtenerTransaccionIdAsync(id);

            if (transaccion == null)
                return Result<ResponseTransaccionDto>.Fail("Transaccion no encontrada", ResultType.NotFound);

            if (dto.MontoARS.HasValue)
                transaccion.MontoARS = dto.MontoARS.Value;

            if (dto.CantidadCripto.HasValue)
                transaccion.CantidadCripto = dto.CantidadCripto.Value;

            if (dto.FechaHora.HasValue)
                transaccion.FechaHora = dto.FechaHora.Value;

            if (!string.IsNullOrWhiteSpace(dto.TipoTransaccion))
            {
                var tipo = dto.TipoTransaccion.Trim().ToLower();

                if (tipo != "purchase" && tipo != "sale")
                    return Result<ResponseTransaccionDto>.Fail("Tipo de transacción inválido", ResultType.BadRequest);

                transaccion.TipoTransaccion = tipo;
            }

            var actualizado = await _repository.EditarTransaccionAsync(transaccion);

            if (!actualizado)
                return Result<ResponseTransaccionDto>.Fail("No se pudo actualizar la Transaccion", ResultType.BadRequest);

            var response = MapToResponse(transaccion);

            return Result<ResponseTransaccionDto>.Ok(response);
        }

        public async Task<Result<bool>> EliminarTransaccionAsync(int id)
        {
            var transaccion = await _repository.ObtenerTransaccionIdAsync(id);

            if (transaccion == null)
                return Result<bool>.Fail("Transaccion no encontrada", ResultType.NotFound);

            var eliminado = await _repository.EliminarTransaccionAsync(transaccion);

            if (!eliminado)
                return Result<bool>.Fail("No se pudo eliminar la Transaccion", ResultType.BadRequest);

            return Result<bool>.Ok(true);
        }

        // MÉTODOS PRIVADOS

        private async Task<decimal> CalcularSaldo(int clientId, string crypto)
        {
            var comprado = await _repository.ObtenerTotalComprado(clientId, crypto);
            var vendido = await _repository.ObtenerTotalVendido(clientId, crypto);

            return comprado - vendido;
        }

        private async Task<Result<(string exchange, decimal valor)>> ObtenerMejorPrecio(
            string? exchange,
            string crypto,
            string tipo)
        {
            var precios = await _criptoYaService.ObtenerPreciosTodos(crypto);

            if (!precios.Any())
                return Result<(string, decimal)>.Fail("No se pudieron obtener precios", ResultType.BadRequest);

            // Si viene exchange específico
            if (!string.IsNullOrWhiteSpace(exchange))
            {
                var seleccionado = precios.FirstOrDefault(p => p.Exchange == exchange);

                if (seleccionado != null)
                {
                    var valor = tipo == "purchase"
                        ? seleccionado.Ask
                        : seleccionado.Bid;

                    return Result<(string, decimal)>.Ok((seleccionado.Exchange, valor));
                }
            }

            // Mejor opción automática
            var mejor = tipo == "purchase"
                ? precios.OrderBy(p => p.Ask).First()
                : precios.OrderByDescending(p => p.Bid).First();

            var valorFinal = tipo == "purchase"
                ? mejor.Ask
                : mejor.Bid;

            return Result<(string, decimal)>.Ok((mejor.Exchange, valorFinal));
        }
    }
}