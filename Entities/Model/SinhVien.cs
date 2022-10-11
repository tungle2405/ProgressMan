using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class SinhVien
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Msv is required field")]
        public string Msv { get; set; }

        [Required(ErrorMessage = "Name is required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lop is required field")]
        public string Class { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required field")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "SDT is required field")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Khoa is required field")]
        public int Khoa { get; set; }

        [Required(ErrorMessage = "Nganh is required field")]
        public int Nganh { get; set; }

        public List<MonHoc> Studying { get; set; }

        public string? ImageUrl { get; set; }
    }
}