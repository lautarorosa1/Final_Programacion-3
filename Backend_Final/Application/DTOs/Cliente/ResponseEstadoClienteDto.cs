namespace Backend_Final.Application.DTOs.Cliente
{
    public class ResponseEstadoClienteDto
    {
        public List<ResumenCriptoDto> Criptos { get; set; } = new();
        public decimal TotalARS { get; set; }
    }

    //AGREGADO - VER
    public class ResumenCriptoDto
    {
        public string Codigo { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal DineroARS { get; set; }
    }
}
