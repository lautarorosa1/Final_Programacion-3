namespace Backend_Final.Application.DTOs.Cliente
{
    public class ResponseClienteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int CantidadTransacciones { get; set; } //traigo las transacciones de cada cliente
    }
}
