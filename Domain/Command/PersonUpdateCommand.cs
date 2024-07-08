using System.ComponentModel.DataAnnotations;

namespace Domain.Command
{
    public class PersonUpdateCommand
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string PersonId { get; set; }

        public string? NombreCompleto { get; set; }
        public string? Edad { get; set; }
        public string? Domicilio { get; set; }
        public string? Telefono { get; set; }
        public string? Profesion { get; set; }
    }
}