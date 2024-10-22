using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class PersonDTO 
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]

        public string? name { get; set; }

        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
        [DefaultValue(18)]

        public int age { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? address { get; set; }

        [Phone(ErrorMessage = "Phone number is not valid.")]

        public string? phoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Profession cannot exceed 100 characters.")]

        public string? profession { get; set; }
    }
}
