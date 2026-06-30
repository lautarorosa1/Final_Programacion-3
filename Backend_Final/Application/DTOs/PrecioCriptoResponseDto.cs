namespace Backend_Final.Application.DTOs
{
    public class PrecioCriptoResponseDto
    {
        public string Exchange { get; set; } = string.Empty;
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
    }
}
