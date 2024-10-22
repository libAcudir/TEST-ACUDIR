using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class FilterQueryDTO
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        public int Age { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Phone number is not valid.")]
        public string? PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Profession cannot exceed 100 characters.")]
        public string? Profession { get; set; }
    }
}
