using System.ComponentModel.DataAnnotations;

namespace Backend_Final.Application.DTOs.Cliente
{
    public class EditarClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }
    }
}
