using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class NganhHoc
    {
        public Guid Id { get; set; }

        public int MaNganh { get; set; }

        [Required(ErrorMessage = "Name is required field")]
        public string Name { get; set; }

    }
}
