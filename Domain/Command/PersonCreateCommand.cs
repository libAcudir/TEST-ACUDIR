using System.ComponentModel.DataAnnotations;

namespace Domain.Command
{
    public class PersonCreateCommand
    {

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string NombreCompleto { get; set; }
        public string Edad { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Domicilio { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 7)]
        public string Telefono { get; set; }
        public string Profesion { get; set; }
    }
}