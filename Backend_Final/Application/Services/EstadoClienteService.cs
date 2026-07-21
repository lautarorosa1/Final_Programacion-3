using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Cliente;
using Backend_Final.Application.DTOs.Cripto;
using Backend_Final.Application.Services.Interfaces;
using Backend_Final.Infrastructure.Repositories.Interfaces;

namespace Backend_Final.Application.Services
{
    public class EstadoClienteService : IEstadoClienteService
    {
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly CriptoYaService _criptoYaService;

        public EstadoClienteService(
            ITransaccionRepository transaccionRepository,
            CriptoYaService criptoYaService)
        {
            _transaccionRepository = transaccionRepository;
            _criptoYaService = criptoYaService;
        }

        public async Task<Result<ResponseEstadoClienteDto>> ObtenerEstadoAsync(int clientId)
        {
            var transacciones = await _transaccionRepository
                .ObtenerTransaccionesAsync(clientId);

            if (!transacciones.Any())
            {
                return Result<ResponseEstadoClienteDto>.Ok(new ResponseEstadoClienteDto
                {
                    Criptos = new List<ResumenCriptoDto>(),
                    TotalARS = 0
                });
            }

            var resultado = new ResponseEstadoClienteDto
            {
                Criptos = new List<ResumenCriptoDto>()
            };

            // Agrupar por cripto
            var agrupadas = transacciones.GroupBy(t => t.CodigoCripto);

            // Diccionario para guardar precios (evita repetir llamadas)
            var preciosPorCrypto = new Dictionary<string, List<PrecioCriptoResponseDto>>();

            // Llamadas paralelas (una por cada cripto)
            var tareasPrecios = agrupadas.Select(async grupo =>
            {
                var crypto = grupo.Key;
                var precios = await _criptoYaService.ObtenerPreciosTodos(crypto);

                preciosPorCrypto[crypto] = precios;
            });

            await Task.WhenAll(tareasPrecios);

            // Procesar resultados
            foreach (var grupo in agrupadas)
            {
                var crypto = grupo.Key;

                var comprado = grupo
                    .Where(t => t.TipoTransaccion == "purchase")
                    .Sum(t => t.CantidadCripto);

                var vendido = grupo
                    .Where(t => t.TipoTransaccion == "sale")
                    .Sum(t => t.CantidadCripto);

                var cantidad = comprado - vendido;

                if (cantidad <= 0)
                    continue;

                var precios = preciosPorCrypto[crypto];

                if (!precios.Any())
                    continue;

                var mejor = precios.OrderByDescending(p => p.Bid).First();

                var valor = cantidad * mejor.Bid;

                resultado.Criptos.Add(new ResumenCriptoDto
                {
                    Codigo = crypto,
                    Cantidad = cantidad,
                    DineroARS = valor
                });
            }

            resultado.TotalARS = resultado.Criptos.Sum(x => x.DineroARS);

            return Result<ResponseEstadoClienteDto>.Ok(resultado);
        }
    }
}
