using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PMLecture.Models
{
    public class MaNganhViewModel
    {
        public int MaNganh { get; set; }

        public string? TenNganh { get; set; }

        public string TenVietTat { get; set; }

        public List<MaNganhViewModel> listMN = new List<MaNganhViewModel>();

        public List<MaNganhViewModel> listMMH = new List<MaNganhViewModel>();

        public List<MaNganhViewModel> GetMaNganh()
        {
            //List<MaNganhViewModel> listMN = new List<MaNganhViewModel>()
            //{
            //    new MaNganhViewModel{ MaNganh = 106, TenNganh = "Công nghệ thông tin", TenVietTat = "TH"},
            //    new MaNganhViewModel{ MaNganh = 107, TenNganh = "Hệ thống thông tin",  TenVietTat = "HT"},
            //    new MaNganhViewModel{ MaNganh = 108, TenNganh = "Công nghệ phần mềm", TenVietTat = "PM"},
            //    new MaNganhViewModel{ MaNganh = 230, TenNganh = "Kinh tế", TenVietTat = "KT"},
            //    new MaNganhViewModel{ MaNganh = 421, TenNganh = "Cơ khí", TenVietTat = "CK"}
            //};
            listMN.Add(new MaNganhViewModel { MaNganh = 106, TenNganh = "Công nghệ thông tin", TenVietTat = "TH" });
            listMN.Add(new MaNganhViewModel { MaNganh = 107, TenNganh = "Hệ thống thông tin", TenVietTat = "HT" });
            listMN.Add(new MaNganhViewModel { MaNganh = 108, TenNganh = "Công nghệ phần mềm", TenVietTat = "PM" });
            listMN.Add(new MaNganhViewModel { MaNganh = 111, TenNganh = "Kinh tế", TenVietTat = "KT" });
            listMN.Add(new MaNganhViewModel { MaNganh = 121, TenNganh = "Cơ khí", TenVietTat = "CK" });

            return listMN;
        }

        public List<MaNganhViewModel> GetMaMonHoc()
        {
            //Dùng chung cho mã môn học
            //Mã ngành => Mã định danh môn học
            //Tên ngành => Tên khoa phụ trách môn học
            //Tên viết tắt => Mã của khoa phụ trách môn học
            listMMH.Add(new MaNganhViewModel { MaNganh = 0, TenNganh = "Công nghệ thông tin", TenVietTat = "CSE" });
            listMMH.Add(new MaNganhViewModel { MaNganh = 0, TenNganh = "Toán", TenVietTat = "MATH" });
            listMMH.Add(new MaNganhViewModel { MaNganh = 0, TenNganh = "Tiếng Anh", TenVietTat = "ENGL" });
            listMMH.Add(new MaNganhViewModel { MaNganh = 0, TenNganh = "Chính trị", TenVietTat = "IDEO" });
            listMMH.Add(new MaNganhViewModel { MaNganh = 0, TenNganh = "Thể chất", TenVietTat = "TC" });
            listMMH.Add(new MaNganhViewModel { MaNganh = 0, TenNganh = "Quân sự", TenVietTat = "GDQP" });
            listMMH.Add(new MaNganhViewModel { MaNganh = 0, TenNganh = "Vật lý", TenVietTat = "PHYS" });

            return listMMH;
        }
    }
}
