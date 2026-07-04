namespace Backend_Final.Application.DTOs.Transaccion
{
    public class ResponseTransaccionDto
    {
        public int Id { get; set; }
        public string CodigoCripto { get; set; } = string.Empty;
        public string TipoTransaccion { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public decimal CantidadCripto { get; set; }
        public decimal MontoARS { get; set; }
        public string? Exchange { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
