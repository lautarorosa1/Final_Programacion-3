namespace Backend_Final.Application.DTOs.Cripto
{
    public class RankingResponseDto
    {
        public string Exchange { get; set; } = "";
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}
