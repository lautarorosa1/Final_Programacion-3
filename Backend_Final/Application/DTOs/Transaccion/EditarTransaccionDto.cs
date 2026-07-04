using System.ComponentModel.DataAnnotations;

namespace Backend_Final.Application.DTOs.Transaccion
{
    public class EditarTransaccionDto
    {
        public decimal? MontoARS { get; set; }

        [Range(0.00000001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal? CantidadCripto { get; set; }
        public string? TipoTransaccion { get; set; }
        public DateTime? FechaHora { get; set; }
    }
}
