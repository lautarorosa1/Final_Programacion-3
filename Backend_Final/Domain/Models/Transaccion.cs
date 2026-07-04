namespace Backend_Final.Domain.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public string CodigoCripto { get; set; } = "";
        public string TipoTransaccion { get; set; } = "";
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public decimal CantidadCripto { get; set; }
        public decimal MontoARS { get; set; }
        public string? Exchange { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
