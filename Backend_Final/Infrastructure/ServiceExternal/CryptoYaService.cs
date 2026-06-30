using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs;
using Backend_Final.Infrastructure.ServiceExternal.Models;
using Microsoft.Extensions.Caching.Memory;

public class CriptoYaService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;

    private readonly string[] _exchanges =
    {
        "satoshitango",
        "buenbit",
        "ripio",
        "lemoncash",
        "argenbtc",
        "binance",
        "fiwind",
        "belo",
        "letsbit",
        "tiendacrypto"
    };

    public CriptoYaService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
    }

    public async Task<List<PrecioCriptoResponseDto>> ObtenerPreciosTodos(string cryptoCode)
    {
        cryptoCode = cryptoCode.ToLower();

        if (_cache.TryGetValue(cryptoCode, out List<PrecioCriptoResponseDto> cached))
            return cached;

        var client = _httpClientFactory.CreateClient("CriptoYa");

        var tasks = _exchanges.Select(ex => ObtenerPrecio(client, ex, cryptoCode));

        var resultados = await Task.WhenAll(tasks);

        var lista = resultados
            .Where(r => r != null)
            .Select(r => r!)
            .ToList();

        // cache 2 minutos
        _cache.Set(cryptoCode, lista, TimeSpan.FromMinutes(2));

        return lista;
    }

    public async Task<Result<List<RankingResponseDto>>> ObtenerRanking(string cryptoCode, string tipo)
    {
        var precios = await ObtenerPreciosTodos(cryptoCode);

        var ranking = precios.Select(p => new RankingResponseDto
        {
            Exchange = p.Exchange,
            PrecioCompra = p.Ask,
            PrecioVenta = p.Bid
        });

        ranking = tipo.ToLower() switch
        {
            "purchase" => ranking.OrderBy(p => p.PrecioCompra),
            "sale" => ranking.OrderByDescending(p => p.PrecioVenta),
            _ => ranking
        };

        return Result<List<RankingResponseDto>>.Ok(ranking.ToList());
    }

    private async Task<PrecioCriptoResponseDto?> ObtenerPrecio(
        HttpClient client,
        string exchange,
        string cryptoCode)
    {
        try
        {
            var url = $"https://criptoya.com/api/{exchange}/{cryptoCode}/ars";

            var data = await client.GetFromJsonAsync<CriptoYaApi>(url);

            if (data == null) return null;

            return new PrecioCriptoResponseDto
            {
                Exchange = exchange,
                Ask = data.ask,
                Bid = data.bid
            };
        }
        catch
        {
            return null;
        }
    }
}