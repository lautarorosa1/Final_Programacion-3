using System.ComponentModel.DataAnnotations;

namespace Backend_Final.Application.DTOs.Transaccion
{
    public class CrearTransaccionDto
    {
        [Required(ErrorMessage = "La criptomoneda es obligatoria")]
        public string CodigoCripto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de transacción es obligatorio")]
        public string TipoTransaccion { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "El cliente es obligatorio")]
        public int ClienteId { get; set; }

        [Range(0.00000001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal CantidadCripto { get; set; }

        public string? Exchange { get; set; }
    }
}
