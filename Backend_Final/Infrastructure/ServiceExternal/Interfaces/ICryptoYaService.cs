using Backend_Final.Application.Common;
using Backend_Final.Application.DTOs.Cripto;

namespace Backend_Final.Infrastructure.ServiceExternal.Interfaces
{
    public interface ICryptoYaService
    {
        Task<List<PrecioCriptoResponseDto>> ObtenerPreciosTodos(string cryptoCode);
        Task<Result<List<RankingResponseDto>>> ObtenerRanking(string cryptoCode, string tipo);
    }
}
