namespace Backend_Final.Application.DTOs.Cripto
{
    public class PrecioCriptoResponseDto
    {
        public string Exchange { get; set; } = "";
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
    }
}
