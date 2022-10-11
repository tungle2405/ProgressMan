using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class CanBo
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required field")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "SDT is required field")]
        public string Phone { get; set; }

        public string? ImageUrl { get; set; }
    }
}
