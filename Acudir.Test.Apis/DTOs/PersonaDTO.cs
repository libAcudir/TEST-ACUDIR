using System.ComponentModel.DataAnnotations;

namespace Acudir.Test.Apis.DTOs
{
    public class PersonaDTO
    {
        public string NombreCompleto { get; set; }
        public string Edad { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Profesion { get; set; }
    }
}
