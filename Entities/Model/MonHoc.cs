using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class MonHoc
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Khoa is required field")]
        public int Khoa { get; set; }

        [Required(ErrorMessage = "Nganh is required field")]
        public int Nganh { get; set; }

        public List<SinhVien> Students { get; set; }

        public GiangVien Teacher { get; set; }

        public KhoaHoc KH { get; set; }

        public NganhHoc NH { get; set; }
    }
}
